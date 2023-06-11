using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace AdvAlgFeleves_ZN8VJ5
{
    public static class Algorithms
    {
        public static object RunGeneticAlgorithm(
                                        IGASolvableProblem problem,
                                        int popSize, int crossoverPopSize, Stopwatch sw,
                                        int maxIteration = 5000, float returnFitness = float.MinValue)
        {
            sw.Restart();

            var population = Enumerable.Repeat(0, popSize).Select(i => problem.CreateNewSolution()).ToArray();
            population = population.OrderBy(solution => problem.Fitness(solution)).ToArray();
            var bestSolution = population.First();
            int i = 0;
            
            while (i < maxIteration && problem.Fitness(bestSolution) > returnFitness)
            {
                var crossoverPopulation = population.Take(crossoverPopSize).ToArray();
                var nextPopulation = Enumerable.Repeat(0, popSize)
                    .Select(i => problem.Mutate(problem.Crossover(crossoverPopulation))).ToArray();

                population = nextPopulation.Concat(population)
                    .OrderBy(solution => problem.Fitness(solution)).Take(popSize).ToArray();
                bestSolution = population.First();
                ++i;
            }

            sw.Stop();
            return problem.GenotypePhenotypeMapping(bestSolution);
        }

        public static object RunHillClimbing(IHCSolvableProblem problem, Stopwatch sw,
                                            int maxIteration = 5000, float returnFitness = float.MinValue)
        {
            sw.Restart();

            var p = problem.CreateNewSolution();
            int i = 0;

            while (i < maxIteration && problem.Fitness(p) > returnFitness)
            {
                var q = problem.RandomNeighbour(p);
                if (problem.Fitness(q) < problem.Fitness(p))
                {
                    p = q;
                }
                ++i;
            }

            sw.Stop();
            return problem.GenotypePhenotypeMapping(p);
        }

        public static object RunSimulatedAnnealing(ISASolvableProblem problem, float kB,
                                                    float T_base, float epsilon, Stopwatch sw,
                                                    int maxIteration = 5000, float returnFitness = float.MinValue)
        {            
            sw.Restart();

            Random rnd = new Random();
            var p = problem.CreateNewSolution();
            var p_opt = problem.DeepCopy(p);
            int t = 0;

            while (t < maxIteration && problem.Fitness(p) > returnFitness)
            {
                ++t;

                var q = problem.RandomNeighbour(p);
                float dE = problem.Fitness(q) - problem.Fitness(p);

                if (dE <= 0)
                {
                    p = q;
                    if (problem.Fitness(p) < problem.Fitness(p_opt))
                    {
                        p_opt = problem.DeepCopy(p);
                    }
                }
                else
                {
                    float T = problem.Temperature(t, T_base, epsilon);
                    float P = MathF.Exp(-dE / kB / T);
                    if (rnd.NextDouble() < P)
                    {
                        p = problem.DeepCopy(q);
                    }
                }

            }

            sw.Stop();
            return problem.GenotypePhenotypeMapping(p_opt);
        }
    }
}
