﻿using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace Lab02
{
    internal class Program
    {
        static char[,] LoadSudoku(string problem)
        {
            char[,] result = new char[9, 9];

            for(int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    if (problem[i * 9 + j] == '0')
                        result[i, j] = '.';
                    else
                        result[i, j] = problem[i * 9 + j];
                }
            }
            return result;
        }

        static void PrintSudoku(char[,] grid)
        {
            string line = new String('-', 22);
            Console.WriteLine(line);
            for (int row = 0; row < 9; row++)
            {
                Console.Write("|");
                for (int col = 0; col < 9; col++)
                {
                    if (grid[row, col] != 0)
                    {
                        Console.Write($" {grid[row, col]}");
                    }
                    else
                    {
                        Console.Write("  ");
                    }

                    if ((col + 1) % 3 == 0)
                    {
                        Console.Write("|");
                    }
                }
                Console.Write('\n');
                if ((row + 1) % 3 == 0)
                {
                    Console.WriteLine(line);
                }
            }
        }

        static bool IsValid(char[ , ] grid, int row, int col, char value)
        {
            for (int i = 0; i < 9; i++)
            {
                if (grid[i, col] != '.' && grid[i, col] == value)
                    return false;

                if (grid[row, i] != '.' && grid[row, i] == value)
                    return false;

                if (grid[3 * (row / 3) + i / 3, 3 * (col / 3) + i % 3] != '.' && grid[3 * (row / 3) + i / 3, 3 * (col / 3) + i % 3] == value)
                    return false;
            }
            return true;
        }

        static bool SolveSudoku(char[ , ] grid)
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] == '.')
                    {
                        for (char val = '1'; val <= '9'; val++)
                        {
                            if (IsValid(grid, i, j, val))
                            {
                                grid[i, j] = val;

                                if (SolveSudoku(grid))
                                    return true;
                                else
                                    grid[i, j] ='.';
                            }
                        }
                        return false;
                    }
                }
            }
            return true;
        }

        static void FillSure(char[,] grid)
        {
            int new_cords = 0;
            int a = 0, b = 0;
            char zero = '0';

            while (a < grid.GetLength(0))
            {
                while (b < grid.GetLength(1))
                {
                    int amount_of_elements = 45;

                    int temp_b = 0, temp_a = 0;
                    
                    bool zero_and_odd_col = b == 0 && b % 3 == 0;
                    bool zero_and_odd_row = a == 0 && a % 3 == 0;

                    if (zero_and_odd_col && zero_and_odd_row)
                    {
                        for (int i = a; i < a + 3; i++)
                        {
                            for (int j = b; j < b + 3; j++)
                            {
                                int found_number = (int)char.GetNumericValue(grid[i, j]);

                                if (found_number != -1) 
                                {
                                    amount_of_elements -= found_number;
                                }
                                else
                                {
                                    temp_b = i;
                                    temp_a = j;
                                    new_cords++;
                                }
                            }
                        }

                        if (new_cords == 1) 
                        {
                            grid[temp_a, temp_b] = (char)(amount_of_elements + zero);
                        }
                    }

                    new_cords = 0;
                    amount_of_elements = 45;

                    int nRow = 0;
                    while (nRow < grid.GetLength(0))
                    {
                        int found_number = (int)char.GetNumericValue(grid[nRow, b]);

                        if (found_number != -1)
                        {
                            amount_of_elements -= found_number;
                        }
                        else
                        {
                            temp_a = nRow;
                            temp_b = b;
                            new_cords++;
                        }
                        nRow++;
                    }

                    if (new_cords == 1) 
                    {
                        grid[temp_a, temp_b] = (char)(amount_of_elements + zero);
                    }

                    new_cords = 0;
                    amount_of_elements = 45;

                    int nCol = 0;
                    while (nCol < grid.GetLength(1))
                    {
                        int found_number = (int)char.GetNumericValue(grid[temp_a, nCol]);

                        if (found_number != -1)
                        {
                            amount_of_elements -= found_number;
                        }
                        else
                        {
                            temp_a = a;
                            temp_b = nCol;
                            new_cords++;
                        }
                        nCol++;
                    }

                    if (new_cords == 1) 
                    {
                        grid[temp_a, temp_b] = (char)(amount_of_elements + zero);                
                    }
                    b++;
                }
                a++;
            }
        }

        static void Main(string[] args)
        {
            string example1 = "632085400754061300819340567000273005021406080000510000060030900048050002173629854";

            char[,] grid = LoadSudoku(example1);
            PrintSudoku(grid);

            Console.WriteLine("\n\n\n");

            FillSure(grid);
            PrintSudoku(grid);

            Console.WriteLine("\n\n\n");

            SolveSudoku(grid);
            PrintSudoku(grid);
            Console.ReadLine();
        }
    }
}
