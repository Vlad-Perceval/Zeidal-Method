using System;
using System.IO;

namespace ZeidelProject
{
    class Program
    {
        static int n = 3;
        static double[,] a = new double[n, n];//матрица коэф.
        static double[,] copya = new double[n, n];
        static double[] copyb = new double[n];
        static double[] b = new double[n];//столбец свободных членов
        static double[] Ans = new double[n];//результирующий вектор
        static double eps = 0.00001;//точность
        static int value = 0;//счётчик итераций
        //Чтение системы
        static void MatrixRead(string FileName)
        {
            StreamReader f = new StreamReader(FileName);
            for (int i = 0; i < n; i++)
            {
                string s = f.ReadLine();
                string[] split = s.Split(new Char[] { ' ' });
                for (int j = 0; j < n; j++)
                    copya[i, j] = a[i, j] = Convert.ToDouble(split[j]);
                copyb[i] = b[i] = Convert.ToDouble(split[n]);
            }
        }
        //Печать системы
        static void MatrixPrint()
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    Console.Write("{0, 7}", Math.Round(a[i, j], 3));
                Console.WriteLine("|" + "{0, 7}", Math.Round(b[i], 3));
            }
            Console.WriteLine();
        }
        //первая норма
        static double FirstNorm()
        {
            int i, j;
            double sum = 0, subSum;
            for (i = 0; i < n; i++)
            {
                subSum = 0;
                for (j = 0; j < n; j++)
                    subSum += Math.Abs(a[i, j]);
                if (subSum > sum)
                    sum = subSum;
            }
            return sum;
        }
        //вторая норма
        static double SecondNorm()
        {
            int i, j;
            double sum = 0, subSum;
            for (j = 0; j < n; j++)
            {
                subSum = 0;
                for (i = 0; i < n; i++)
                    subSum += Math.Abs(a[i, j]);
                if (subSum > sum)
                    sum = subSum;
            }
            return sum;
        }
        //треться норма
        static double ThirdNorm()
        {
            int i, j;
            double sum = 0;
            for (i = 0; i < n; i++)
            {
                for (j = 0; j < n; j++)
                    sum += (a[i, j] * a[i, j]);
            }
            sum = Math.Sqrt(sum);
            return sum;
        }

        static void Zeidel()
        {
            double g = 0, max = 0, buf = 0;
            for (int i = 0; i < n; i++)
                Ans[i] = b[i];//столбец свободных становится первым приближением
            Console.WriteLine("№\tx1\t\tx2\t\tx3\t\td");
            do
            {

                Console.Write("{0}\t", value);
                for (int i = 0; i < n; i++)
                    Console.Write("{0:0.000000}\t", Math.Round(Ans[i], 6));
                Console.WriteLine("{0:0.000000}\t", Math.Round(max, 6));
                for (int i = 0; i < n; i++)
                {
                    max = 0; buf = 0;
                    g = b[i];
                    for (int j = 0; j < n; j++)
                    {
                        double c = a[i, j] * Ans[j];
                        g = g + c;
                    }
                    buf = Math.Abs(g - Ans[i]);
                    if (max < buf) max = buf;
                    Ans[i] = g;
                }
                value++;

            } while (max > eps);
            Console.WriteLine("Всего итераций - {0} шт.", value);
            for (int i = 0; i < n; i++)
            {
                Console.Write("X" + (i + 1) + " = ");
                Console.WriteLine("{0, 7}", Ans[i]);
            }
        }

        static void CheckAnsver()
        {
            Console.WriteLine("Проверка:");
            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                for (int j = 0; j < n; j++)
                {
                    sum += copya[i, j] * Ans[j];
                }
                Console.WriteLine("{0} = {1}", sum, copyb[i]);
            }
            Console.WriteLine();
        }

        static void DiagonalDominating()
        {
            double[] str1 = new double[n];
            double[] str2 = new double[n];
            double[] str3 = new double[n];
            double[] freeB = (double[])b.Clone();
            for (int i = 0; i < n; i++)
            {
                str1[i] = a[0, i];
                str2[i] = a[1, i];
                str3[i] = a[2, i];
            }
            for (int i = 0; i < n; i++)
            {
                a[0, i] = (str1[i] + str3[i]) / 10;
                a[1, i] = (str2[i] - str3[i]) / 10;
                a[2, i] = (4 * str1[i] - 3 * str2[i]) / 10;
            }
            freeB[0] = (b[0] + b[2]) / 10;
            freeB[1] = (b[1] - b[2]) / 10;
            freeB[2] = (4 * b[0] - 3 * b[1]) / 10;
            b = freeB;
        }

        static void SystemTransformation()
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                    {
                        if (a[i, j] > 0)
                            a[i, j] = (1 - a[i, j]);
                        else
                        {
                            a[i, j] = (1 + a[i, j]);
                            b[i] = -b[i];
                        }
                    }
                    else a[i, j] = -a[i, j];
                }
            }
        }

        static void SystemPrint()
        {
            for (int i = 0; i < n; i++)
                Console.WriteLine("x{0} = {1}x1+({2})x2+({3})x3+({4})", i + 1, a[i, 0], a[i, 1], a[i, 2], b[i]);
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            MatrixRead("C:\\Users\\yfvfy\\source\\repos\\Zeidal\\Zeidal\\input.txt");
            Console.WriteLine("Исходная система:");
            MatrixPrint();
            Console.WriteLine("Приведем систему к такому виду, чтобы преобладали диагональне элементы:\n");
            DiagonalDominating();
            MatrixPrint();
            Console.WriteLine("Приведем систему к  виду x=Cx+d:\n");
            SystemTransformation();
            SystemPrint();
            if (FirstNorm() < 1 || SecondNorm() < 1 || ThirdNorm() < 1)
            {
                Zeidel();
                CheckAnsver();
            }
            else Console.WriteLine("Условие сходимости по евклидовой метрике не выполняется!");
        }
    }
}
