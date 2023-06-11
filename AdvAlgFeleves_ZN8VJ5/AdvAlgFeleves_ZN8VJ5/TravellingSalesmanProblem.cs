using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdvAlgFeleves_ZN8VJ5
{
    struct Town
    {
        public float x;
        public float y;
    }

    class TravellingSalesmanProblem : IGASolvableProblem, ISASolvableProblem, IHCSolvableProblem
    {
        private Random rnd = new Random();
        private List<Town> towns = new List<Town>();

        public void LoadTownsFromFile(string fname)
        {
            StreamReader sr = new StreamReader(fname);

            while (!sr.EndOfStream)
            {
                string[] line = sr.ReadLine().Split('\t');
                towns.Add(new Town()
                {
                    x = float.Parse(line[0]),
                    y = float.Parse(line[1])
                });
            }

            sr.Close();
        }

        public float Fitness(Town[] route)
        {
            float sum = 0f;

            for (int i = 0; i < route.Length - 1; i++)
            {
                Town t1 = route[i];
                Town t2 = route[i + 1];
                sum += MathF.Sqrt(MathF.Pow(t1.x - t2.x, 2) + MathF.Pow(t1.y - t2.y, 2));
            }

            return sum;
        }

        public float Temperature(int t, float T_base, float epsilon)
        {
            return T_base * MathF.Pow(1 - epsilon, t);
        }

        public object RandomNeighbour(object solution)
        {
            Town[] route = DeepCopy(solution as Town[]) as Town[];

            int randomIndex = rnd.Next(route.Length);
            float tmp_x = route[randomIndex].x;
            float tmp_y = route[randomIndex].y;

            int randomIndex2 = rnd.Next(route.Length);
            route[randomIndex].x = route[randomIndex2].x;
            route[randomIndex].y = route[randomIndex2].y;

            route[randomIndex2].x = tmp_x;
            route[randomIndex2].y = tmp_y;

            return route;
        }

        public object DeepCopy(object solution)
        {
            Town[] route = solution as Town[];
            Town[] copy = new Town[route.Length];
            Array.Copy(route, copy, copy.Length);
            return copy;
        }

        public object CreateNewSolution()
        {
            return towns.OrderBy(t => rnd.NextDouble()).ToArray();
        }

        public float Fitness(object solution)
        {
            Town[] route = solution as Town[];
            float sum = 0f;

            for (int i = 0; i < route.Length - 1; i++)
            {
                Town t1 = route[i];
                Town t2 = route[i + 1];
                sum += MathF.Sqrt(MathF.Pow(t1.x - t2.x, 2) + MathF.Pow(t1.y - t2.y, 2));
            }

            return sum;
        }

        public object GenotypePhenotypeMapping(object solution)
        {
            return solution;
        }

        public object Crossover(object[] solutions)
        {
            Town[][] towns = new Town[solutions.Length][];
            for (int j = 0; j < solutions.Length; j++)
            {
                towns[j] = solutions[j] as Town[];
            }

            Town[] newSolution = new Town[this.towns.Count];

            Town[] parent1 = towns[rnd.Next(0, towns.Length)]; // domináns szülő
            Town[] parent2 = towns[rnd.Next(0, towns.Length)];
            int i = 0;

            while (i < this.towns.Count && parent1[i].x == parent2[i].x && parent1[i].y == parent2[i].y)
            {
                newSolution[i] = parent1[i];
                i++;
            }

            while (i < this.towns.Count)
            {
                newSolution[i] = parent1[i];
                i++;
            }


            return newSolution;
        }

        public object Mutate(object solution)
        {
            return RandomNeighbour(solution);
        }
    }
}
