using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace GaussAlgorithm;

public class Solver
{
    public double[] Solve(double[][] matrix, double[] freeMembers)
    {
        double[][] matrix2 = matrix.Select(x => x.Take(x.Length).ToArray()).ToArray();
        double[] freeMembers2 = freeMembers.Take(freeMembers.Length).ToArray();
        List<int> eachvalue = new List<int>();
        List<int> usedstrings = new List<int>();
        for (int i = 0; i < matrix2.Length - 1; i++)
        {
            for (int j = i + 1; j < matrix2.Length; j++)
            {
                if (matrix2[i].Length != matrix2[j].Length) throw new NoSolutionException("");
                if (AreEquationsProposed(matrix2[i], matrix2[j]) != double.NaN
                    && Math.Abs(AreEquationsProposed(matrix2[i], matrix2[j]) - freeMembers2[i]
                    / freeMembers2[j]) >= 1e-6) throw new NoSolutionException("");
            }
        }

        for (int k = 0; k < matrix2[0].Length; k++)
        {
            int u = 0;
            for (int p = 0; p < matrix2.Length; p++)
            {
                if (Math.Abs(matrix2[p][k]) >= 1e-6 && !usedstrings.Contains(p))
                {
                    u++;
                    usedstrings.Add(p);
                    for (int i = 0; i < matrix2.Length; i++)
                    {
                        if (i != p && Math.Abs(matrix2[i][k]) >= 1e-6)
                        {
                            var curr = matrix2[i][k] / matrix2[p][k];
                            matrix2[i] = matrix2[i].Zip(matrix2[p], (x, y) => x -= curr * y).ToArray();
                            freeMembers2[i] -= curr * freeMembers2[p];
                        }
                    }
                    break;
                }
            }

            if (u == 0)
                eachvalue.Add(k);
        }

        foreach (var e in eachvalue)
        {
            for (int i = 0; i < matrix2.Length; i++)
                matrix2[i][e] = 0;
        }
        double[] solutions = new double[matrix2[0].Length];
        for (int n = 0; n < matrix2[0].Length; n++)
        {
            int number = 0;
            for (int k = 0; k < matrix2.Length; k++)
            {
                if (Math.Abs(matrix2[k][n]) >= 1e-6)
                {
                    number++;
                    solutions[n] = freeMembers2[k] / matrix2[k][n];
                }
            }
            if (number == 0) solutions[n] = 0;
        }
        return solutions;
    }

    public static double AreEquationsProposed(double[] a, double[] b)
    {
        var c = a.Zip(b, (a1, b1) => a1 / b1).ToList();
        for (int y = 1; y < c.Count; y++)
        {
            if (Math.Abs(c[y] - c[y - 1]) >= 1e-6) return double.NaN;
        }
        return c[0];
    }
}