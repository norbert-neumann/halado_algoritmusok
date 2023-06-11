using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdvAlgFeleves_ZN8VJ5
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            string[,] timeTable = new string[4, 4];
            string[,] fitnessTable = new string[4, 4];

            // Fejlécek
            fitnessTable[0, 1] = timeTable[0, 1] = "Function Approximation";
            fitnessTable[0, 2] = timeTable[0, 2] = "Travelling Salesman";
            fitnessTable[0, 3] = timeTable[0, 3] = "Work Assigment";
            fitnessTable[1, 0] = timeTable[1, 0] = "Simulated Annealing";
            fitnessTable[2, 0] = timeTable[2, 0] = "Genetic Algorithm";
            fitnessTable[3, 0] = timeTable[3, 0] = "Hill Climbing";

             FunctionApproximationProblem funcApproxProblem = new FunctionApproximationProblem(-5, 5);
             funcApproxProblem.LoadKnownValuesFromFile("function_sample_large.txt");
             Console.WriteLine("Solving function approximation problem...");

             fitnessTable[1, 1] = MathF.Round(funcApproxProblem.Fitness(Algorithms.RunSimulatedAnnealing(funcApproxProblem, 0.1f, 150, 0.03f, sw) as float[]), 2).ToString();
             timeTable[1, 1] = sw.ElapsedMilliseconds.ToString() + " ms";

             fitnessTable[2, 1] = MathF.Round(funcApproxProblem.Fitness(Algorithms.RunGeneticAlgorithm(funcApproxProblem, 50, 10, sw) as float[]), 2).ToString();
             timeTable[2, 1] = sw.ElapsedMilliseconds.ToString() + " ms";

             fitnessTable[3, 1] = MathF.Round(funcApproxProblem.Fitness(Algorithms.RunHillClimbing(funcApproxProblem, sw) as float[]), 2).ToString();
             timeTable[3, 1] = sw.ElapsedMilliseconds.ToString() + " ms";

             TravellingSalesmanProblem tsProblem = new TravellingSalesmanProblem();
             tsProblem.LoadTownsFromFile("towns_large.txt");
             Console.WriteLine("Solving travelling salesman problem...");

             fitnessTable[1, 2] = tsProblem.Fitness(Algorithms.RunSimulatedAnnealing(tsProblem, 0.1f, 150, 0.03f, sw) as Town[]).ToString();
             timeTable[1, 2] = sw.ElapsedMilliseconds.ToString() + " ms";

             fitnessTable[2, 2] = tsProblem.Fitness(Algorithms.RunGeneticAlgorithm(tsProblem, 50, 10, sw) as Town[]).ToString();
             timeTable[2, 2] = sw.ElapsedMilliseconds.ToString() + " ms";

             fitnessTable[3, 2] = tsProblem.Fitness(Algorithms.RunHillClimbing(tsProblem, sw) as Town[]).ToString();
             timeTable[3, 2] = sw.ElapsedMilliseconds.ToString() + " ms";

             WorkAssigmentProblem wsProblem = new WorkAssigmentProblem(0.1f, 10);
             wsProblem.LoadFromFile("workers_large.txt");
             Console.WriteLine("Solving work assigment problem...");

             fitnessTable[1, 3] = wsProblem.Fitness(Algorithms.RunSimulatedAnnealing(wsProblem, 0.1f, 150, 0.03f, sw) as int[]).ToString();
             timeTable[1, 3] = sw.ElapsedMilliseconds.ToString() + " ms";

             fitnessTable[2, 3] = wsProblem.Fitness(Algorithms.RunGeneticAlgorithm(wsProblem, 50, 10, sw) as int[]).ToString();
             timeTable[2, 3] = sw.ElapsedMilliseconds.ToString() + " ms";

             fitnessTable[3, 3] = wsProblem.Fitness(Algorithms.RunHillClimbing(wsProblem, sw) as int[]).ToString();
             timeTable[3, 3] = sw.ElapsedMilliseconds.ToString() + " ms";

             for (int i = 0; i < 4; i++)
             {
                 for (int j = 0; j < 4; j++)
                 {
                     Console.Write("{0, 25}\t", fitnessTable[i, j]);
                 }
                 Console.WriteLine();
             }
             Console.WriteLine();
             for (int i = 0; i < 4; i++)
             {
                 for (int j = 0; j < 4; j++)
                 {
                     Console.Write("{0, 25}\t", timeTable[i, j]);
                 }
                 Console.WriteLine();
             }
            
        }
    }
}
