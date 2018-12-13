using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        class Matrix
        {
            public int rows, columns;
            public int[,] numbers;
            public int[,] distance;
            public int[,] graph;
            public int[] way;

            public Matrix(int[,] nums, int rows, int columns)
            {
                this.rows = rows;
                this.columns = columns;
                this.way = new int[rows];
                numbers = new int[rows, columns];
                distance = new int[rows, rows];
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < columns; j++)
                        numbers[i, j] = nums[i, j];
                FindDistance();
                CreateGraph();
            }

            private void FindDistance()
            {
                //подсчет Хэммингова расстояния для строк матрицы
                //заполнение матрицы расстояний симметричной
                for (int i = 0; i < rows; i++)
                    for (int j = i; j < rows; j++)
                    {
                        if (i == j)
                        {
                            //какой смысл сравнивать строку с собой же
                            distance[i, j] = 0;
                        }
                        else
                        {
                            //сравнение каждого элемента строки и подсчет расстояния
                            //Хэммингово расстояние равно количеству неодинаковых символов двух последовательностей на соответствующих позициях
                            int count = 0;
                            for (int k = 0; k < columns; k++)
                            {
                                if (numbers[i, k] != numbers[j, k])
                                    count++;
                            }
                            distance[i, j] = distance[j,i] = count;
                        }
                    }

                Console.WriteLine("Матрица расстояний Хэмминга");
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        Console.Write(distance[i, j] + " ");
                    }
                    Console.Write("\n");
                }
            }

            private void CreateGraph()
            {
                graph = new int[rows, rows];
                //первый проход - поиск минимального значения во всей матрице
                int minVal = columns + 1;
                int imin = 0, jmin = 0;
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < rows; j++)
                        if (i != j && distance[i, j] < minVal)
                        {
                            //фиксация минимального значения
                            minVal = distance[i, j];
                            imin = i;
                            jmin = j;
                        }
                way[0] = imin;
                for (int step = 0; step < rows - 1; step++)
                {
                    //записываем минимальное значение
                    graph[imin, jmin] = minVal;
                    graph[jmin, imin] = minVal;
                    //удаляем строку и столбец i
                    for (int i = 0; i < rows; i++)
                    {
                        distance[imin, i] = -1;
                        distance[i, imin] = -1;
                    }
                    //ищем минимальный столбец в строке j
                    minVal = columns + 1;
                    imin = jmin;
                    for (int j = 0; j < rows; j++)
                    {
                        if (imin != j && distance[imin, j] < minVal && distance[imin, j] != -1)
                        {
                            minVal = distance[imin, j];
                            jmin = j;
                        }
                    }
                    //если осталась последняя вершина
                    if (minVal == columns + 1)
                    {
                        int i = 0;
                        while (minVal == columns + 1 && i < rows)
                        {
                            for (int j = 0; j < rows; j++)
                                if (distance[i, j] != -1)
                                {
                                    minVal = distance[i, j];
                                    imin = i;
                                    jmin = j;
                                }
                            i++;
                        }
                    }
                    way[step + 1] = imin;
                }
                graph[imin, jmin] = minVal;
                graph[jmin, imin] = minVal;

                //print
                string str1 = "" + way[0].ToString();
                string str2 = "";
                for (int i = 1; i < way.Length; i++)
                {
                    str1 += " - " + way[i];
                    str2 += graph[way[i], way[i-1]] + "   ";
                }
                Console.WriteLine($"Граф {str1}");
                Console.WriteLine($"Вес   {str2}");
            }

            public void CreateGroups(int L)
            {
                if (L > rows || L == 0 || L == 1) return;
                int[] groups = new int[L-1];
                for (int i = 0; i < L - 1; i++)
                {
                    int maxVal = -1;
                    int v = 0;
                    for (int j = 1; j < rows; j++)
                    {
                        if (maxVal < graph[way[j], way[j - 1]])
                        {
                            maxVal = graph[way[j], way[j - 1]];
                            groups[i] = j;
                        }
                    }
                    graph[way[groups[i]], way[groups[i] - 1]] = -1;
                    graph[way[groups[i] -1], way[groups[i]]] = -1;
                }

                Array.Sort(groups);
                string str1 = "Кластеры ";
                int h = 0;
                for (int i = 0; i < way.Length; i++)
                {
                    
                    if (groups[h] == i)
                    {
                        if (h < groups.Length - 1) h++;
                        str1 += "| ";
                    }
                    str1 += way[i] + " ";

                }
                Console.WriteLine(str1);
            }
        }

        public static void Main()
        {
            //initial data
            int rows, columns, L;
            //int rows = 6, columns = 8, L = 3;
            Console.Write("Введите количество строк матрицы: ");
            rows = int.Parse(Console.ReadLine());
            Console.Write("Введите количество столбцов матрицы: ");
            columns = int.Parse(Console.ReadLine());

            int[,] numbers = new int[rows, columns];
            //int[,] numbers = new int[,] {
            //    {1,1,0,1,1,0,0,1},
            //    {1,0,1,1,0,1,1,0 },
            //    {0,1,0,0,0,0,0,1 },
            //    {1,1,0,0,1,0,0,1 },
            //    {0,0,1,1,0,1,1,0 },
            //    {0,1,0,0,0,0,0,1 }
            //    };
            Console.WriteLine($"Введите значения исходной матрицы ({rows}x{columns}):");
            for (int i = 0; i < rows; i++)
            {
                string[] str = Console.ReadLine().Split(' ');
                for (int j = 0; j < columns; j++)
                    numbers[i, j] = int.Parse(str[j]);
            }
            Console.Write("Введите параметр L : ");
            L = int.Parse(Console.ReadLine());

            Matrix matrix = new Matrix(numbers, rows, columns);
            matrix.CreateGroups(L);
            Console.ReadKey();
        }

    }
}
