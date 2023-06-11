using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdvAlgFeleves_ZN8VJ5
{
    class Person
    {
        public float salary;
        public float quality;
    }

    class WorkAssigmentProblem : IGASolvableProblem, ISASolvableProblem, IHCSolvableProblem
    {
        private List<Person> people = new List<Person>();
        private Random rnd = new Random();
        private float salaryWeight;
        private float qualityWeight;

        public WorkAssigmentProblem(float salaryWeight, float qualityWeight)
        {
            this.salaryWeight = salaryWeight;
            this.qualityWeight = qualityWeight;
        }

        public void LoadFromFile(string fname)
        {
            StreamReader sr = new StreamReader(fname);

            while (!sr.EndOfStream)
            {
                string[] line = sr.ReadLine().Split('\t');
                people.Add(new Person() { salary = float.Parse(line[0]), quality = float.Parse(line[1]) });
            }

            sr.Close();
        }

        public float SumSalary(int[] solution)
        {
            float sum = 0f;

            for (int i = 0; i < solution.Length; i++)
            {
                sum += people[i].salary * solution[i];
            }

            return sum;
        }

        public float AvgQuality(int[] solution)
        {
            float sum = 0f;

            for (int i = 0; i < solution.Length; i++)
            {
                sum += people[i].quality * solution[i];
            }

            return sum;
        }

        public object GenotypePhenotypeMapping(object solution)
        {
            return solution;
        }

        public object RandomNeighbour(object solution)
        {
            int[] assigment = new int[people.Count];
            Array.Copy(solution as int[], assigment, people.Count);

            int changeIdx = rnd.Next(assigment.Length);
            assigment[changeIdx] = assigment[changeIdx] == 1 ? 0 : 1;

            return assigment;
        }

        public object CreateNewSolution()
        {
            int[] assigments = new int[people.Count];

            for (int i = 0; i < assigments.Length; i++)
            {
                if (rnd.NextDouble() >= 0.5)
                {
                    assigments[i] = 1;
                }
            }

            return assigments;
        }

        public float Fitness(object solution)
        {
            int[] assigment = solution as int[];

            return SumSalary(assigment) * this.salaryWeight - AvgQuality(assigment) * this.qualityWeight;
        }

        public float Temperature(int t, float T_base, float epsilon)
        {
            return T_base * MathF.Pow(1 - epsilon, t);
        }

        public object DeepCopy(object solution)
        {
            int[] assigments = solution as int[];
            int[] copy = new int[assigments.Length];
            Array.Copy(assigments, copy, copy.Length);
            return copy;
        }

        public object Crossover(object[] solutions)
        {
            int[][] assigments = new int[solutions.Length][];
            for (int i = 0; i < assigments.Length; i++)
            {
                assigments[i] = solutions[i] as int[];
            }

            int[] child = new int[people.Count];
            for (int i = 0; i < child.Length; i++)
            {
                child[i] = assigments[rnd.Next(assigments.Length)][i];
            }

            return child;
        }

        public object Mutate(object solution)
        {
            int[] assigments = solution as int[];

            int rndIndex = rnd.Next(assigments.Length);
            assigments[rndIndex] = assigments[rndIndex] == 1 ? 0 : 1;

            return assigments;
        }
    }
}
