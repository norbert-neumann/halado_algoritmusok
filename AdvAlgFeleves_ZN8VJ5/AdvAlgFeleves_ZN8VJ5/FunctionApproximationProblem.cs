using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdvAlgFeleves_ZN8VJ5
{
    class FunctionApproximationProblem : IGASolvableProblem, ISASolvableProblem, IHCSolvableProblem
    {
        private Random rnd = new Random();
        private int upperBound;
        private int lowerBound;
        private List<KeyValuePair<float, float>> knownValues = new List<KeyValuePair<float, float>>();

        public FunctionApproximationProblem(int lowerBound = int.MinValue, int upperBound = int.MaxValue)
        {
            this.upperBound = upperBound;
            this.lowerBound = lowerBound;
        }

        public void LoadKnownValuesFromFile(string fname)
        {
            StreamReader sr = new StreamReader(fname);

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                float x = float.Parse(line.Split('\t')[0]);
                float y = float.Parse(line.Split('\t')[1]);
                knownValues.Add(new KeyValuePair<float, float>(x, y));
            }
        }

        public object CreateNewSolution()
        {
            float[] randomSolution = new float[5];
            for (int i = 0; i < randomSolution.Length; i++)
            {
                randomSolution[i] = (float)rnd.NextDouble() * rnd.Next(lowerBound, upperBound);
            }
            return randomSolution;
        }

        public object Crossover(object[] solutions)
        {
            float[][] coefficients = new float[solutions.Length][];
            for (int i = 0; i < solutions.Length; i++)
            {
                coefficients[i] = solutions[i] as float[];
            }


            float[] newSolution = new float[coefficients[0].Length];

            for (int i = 0; i < newSolution.Length; i++)
            {
                newSolution[i] = coefficients[rnd.Next(solutions.Length)][i];
            }

            return newSolution;
        }

        public float Fitness(object solution)
        {
            float sumDiff = 0;
            float[] coefficients = solution as float[];

            foreach (KeyValuePair<float, float> pair in knownValues)
            {
                float x = pair.Key;
                float yPrime = coefficients[0] * MathF.Pow(x - coefficients[1], 3) +
                    coefficients[2] * MathF.Pow(x - coefficients[3], 2) +
                    coefficients[4] * x;

                float diff = MathF.Pow(yPrime - pair.Value, 2);
                sumDiff += diff;
            }

            if (float.IsInfinity(sumDiff))
            {
                sumDiff = float.MaxValue;
            }

            return sumDiff;
        }

        public object GenotypePhenotypeMapping(object solution)
        {
            return solution;
        }

        public object Mutate(object solution)
        {
            float[] coefficients = solution as float[];

            coefficients[rnd.Next(coefficients.Length)] = rnd.Next(lowerBound, upperBound);

            return coefficients;
        }

        public float Temperature(int t, float T_base, float epsilon)
        {
            return T_base * MathF.Pow(1 - epsilon, t);
        }

        public object RandomNeighbour(object solution)
        {
            float[] coefficients = DeepCopy(solution as float[]) as float[];

            for (int i = 0; i < coefficients.Length; i++)
            {
                coefficients[i] += (float)rnd.NextDouble() * (rnd.Next(0, 2) == 0 ? -0.1f : 0.1f);
            }


            return coefficients;
        }

        public object DeepCopy(object solution)
        {
            float[] coefficients = solution as float[];
            float[] copy = new float[coefficients.Length];
            Array.Copy(coefficients, copy, coefficients.Length);
            return copy;
        }
    }
}
