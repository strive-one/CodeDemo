using Godot;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;

public class HouseMatrix
{
	public int kernel_size { get; set; }
	public int matrix_size { get; set; }
	public int[,] matrix { get; set; }
	public int[,] intersection_matrix { get; set; }
	List<(int, int)> intersection { get; set; }



	public HouseMatrix(int _matrix_size, int _kernel_size)
	{
		matrix_size = _matrix_size;
		kernel_size = _kernel_size;
		matrix = new int[_matrix_size, _matrix_size];
	}

	public void AddHouse(int x, int y)
	{
		matrix[x, y] = 1;
	}

	public void RemoveHouse(int x, int y)
	{
		matrix[x, y] = 0; 
	}

	/// <summary>
	/// Get the intersections of the area around all houses.
	/// </summary>
	public List<(int, int)> FindIntersection()
	{
		intersection = new();
		intersection_matrix = new int[matrix_size, matrix_size];

		// Iterate trough house matrix
		for (int i = 0; i < matrix_size; i++)
		{
			for (int j = 0; j < matrix_size; j++)
			{
				if (matrix[i, j] == 1)
				{
					IntersectDiamond(i, j);
				}
			}
		}

		return intersection;
	}

	/// <summary>
	/// Fill diamond centered on [x, y] in intersection_matrix.
	/// </summary>
	private void IntersectDiamond(int x, int y)
	{
		int count = 1;
		for (int i = 0; i < kernel_size; i++)
		{
			IntersectLine(i, x, y, count);
			count += 2;
		}

		IntersectLine(kernel_size, x, y, count);

		count = 1;
		for (int i = kernel_size * 2; i > kernel_size; i--)
		{
			IntersectLine(i, x, y, count);
			count += 2;
		}
	}

	/// <summary>
	/// Fill line centered on [x, y] in intersection_matrix.
	/// When a filled cell is encountered add it to intersection.
	/// </summary>
	private void IntersectLine(int p, int _x, int _y, int count)
	{
		int offset = (kernel_size * 2 + 1 - count) / 2;

		for (int i = offset; i < offset + count; i++)
		{
			int y = _y + i - kernel_size;
			int x = _x + p - kernel_size;

			// Skip iteration when out of bounds
			if (x < 0 | x >= matrix_size | y < 0 | y >= matrix_size) { continue; }

			if (intersection_matrix[x, y] == 1)
			{
				intersection.Add((x, y));
				intersection_matrix[x, y] = 2;
			}
			else if (matrix[x, y] != 1 && intersection_matrix[x, y] != 2)
			{
				intersection_matrix[x, y] = 1;
			}
		}
	}
}
