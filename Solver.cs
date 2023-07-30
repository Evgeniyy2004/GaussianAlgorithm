using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace GaussAlgorithm;

public class Solver
{
	public double[] Solve(double[][] matrix, double[] freeMembers)
	{
		double[][] matrix2 = matrix.Select(x=>x.Take(x.Length).ToArray()).ToArray();
		double[] freeMembers2 = freeMembers.Take(freeMembers.Length).ToArray();
		List<int> eachvalue=new List<int>();
		List<int> usedstrings = new List<int>();
		for(int i=0; i<matrix2.Length-1; i++)
		{
			
			for(int j=i+1;j<matrix2.Length;j++)
			{
				if (matrix2[i].Length!=matrix2[j].Length || (matrix2[j].SequenceEqual(matrix2[i]) && freeMembers2[i] != freeMembers2[j])) throw  new NoSolutionException("");
			}
		}

		for(int k=0; k<matrix2[0].Length; k++) 
		{
            int u = 0;
            for (int p=0; p<matrix2.Length; p++) 
			{
				
				if (matrix2[p][k]!=0.0 && !usedstrings.Contains(p))
				{
					u++;
					usedstrings.Add(p);
					for(int i=0;i<matrix2.Length ;i++) 
					{
						if (i != p && matrix2[i][k] != 0.0)
						{
							var curr = matrix2[i][k] / matrix2[p][k];
							for (int y = 0; y < matrix2[i].Length; y++)
							{
								matrix2[i][y] -= curr * matrix2[p][y];
							}
							freeMembers2[i] -= curr * freeMembers2[p];
						}
					}
					break;
				}				
			}
            if (u == 0) 
				eachvalue.Add(k);
        }
		foreach(var e in eachvalue)
		{
			for (int i = 0; i < matrix2.Length; i++) matrix2[i][e] = 0;
		}
		double[] solutions= new double[matrix2[0].Length];
		for(int n=0; n < matrix2[0].Length ; n++)
		{
            int number = 0;
            for (int k=0; k<matrix2.Length ;k++)
			{
				
				if (matrix2[k][n]!=0.0)
				{
					number++;
					solutions[n] = freeMembers2[k] / matrix2[k][n];
				}
			}
			if(number==0) solutions[n] = 0;
		}
		return solutions;
	}
}