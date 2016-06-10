using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Main
{
    class CppFuncs
    {
        [DllImport("MatrixLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double CountTime(int size, int count);
        [DllImport("MatrixLib.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SolveSystem(int size, double[] a, double[] b, double[] c, double[] f, double[] x);
    }

    class Program
    {
        static void Main(string[] args)
        {
            ValidateAnswer();
            WorkWithFile();
        }

        private static void ValidateAnswer()
        {
            Console.WriteLine("=======================================\n" +
                              "Шаг 1: Проверка правильности вычислений\n" +
                              "=======================================\n\n" +
                              "Ожидаемый результат: \n" +
                              "x = {-10, 5, -2, -10, -3}\n");

            // Тестовая матрица, где x = {-10, 5, -2, -10, -3}
            double[] a = { -3, -5, -6, -5 };
            double[] b = { -1, -1, 2, -4 };
            double[] c = { 2, 8, 12, 18, 10 };
            double[] f = { -25, 72, -69, -154, 20 };

            // Решение системы уравнений (C#)
            Console.WriteLine("С#:");
            Matrix matrix = new Matrix(5, a, b, c);
            double[] x = matrix.SolveSystem(f);
            matrix.Show(f, x);

            // Решение системы уравнений (C++)
            Console.WriteLine("\nС++:");

            try
            {
                CppFuncs.SolveSystem(5, a, b, c, f, x);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Что-то пошло не так при работе с библиотекой :(\n" +
                                  e.Message);
                Console.ReadKey();
                Environment.Exit(0);
            }


            // Ждем нажатия клавиши для завершения
            Console.Write("\nНажмите Enter для перехода к следующему шагу...");
            Console.ReadKey();
            Console.Clear();
        }

        private static void WorkWithFile()
        {
            string filename;
            while (true)
            {
                Console.Write("==============================\n" +
                              "Шаг 2: Загрузка/создание файла\n" +
                              "==============================\n\n" +
                              "Введите имя файла: ");
                filename = Console.ReadLine();
                if (filename != null)
                {
                    filename = filename.Trim();
                    if (!filename.Equals(".dat") && !filename.Equals(""))
                    {
                        if (!filename.EndsWith(".dat"))
                        {
                            filename += ".dat";
                        }

                        break;
                    }
                }

                Console.Clear();
            }

            TimeList timeList = new TimeList(filename);
            Console.Write("\nНажмите Enter для перехода к следующему шагу...");
            Console.ReadKey();
            Console.Clear();

            while (true)
            {
                Console.Write("================================\n" +
                              "Шаг 3: Сравнение скорости работы\n" +
                              "================================\n\n" +
                              "Введите порядок матрицы (0 для выхода): ");

                var size = ReadInt();

                if (size == 0)
                {
                    Console.Clear();
                    End(timeList);
                    break;
                }
                
                Console.Write("Введите число повторов: ");
                var count = ReadInt();

                try
                {
                    Console.Write("Расчёты на С++...");
                    var cppTime = CppFuncs.CountTime(size, count);

                    Console.Write("Расчёты на С#...");
                    var charpTime = CountTime(size, count);

                    timeList.Add(size, count, cppTime, charpTime);
                    Console.Clear();
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Что-то пошло не так при работе с библиотекой :(\n" +
                                      e.Message);
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
        }

        private static double CountTime(int size, int count)
        {
            Matrix matrix = new Matrix(size);
            var f = new double[size];

            for (var i = 0; i < size; i++)
            {
                f[i] = matrix.Random.Next(0, 100);
            }

            Stopwatch sw = Stopwatch.StartNew();
            for (var i = 0; i < count; i++)
            {
                matrix.SolveSystem(f);
            }
            sw.Stop();

            return sw.ElapsedMilliseconds / 1000.0;
        }

        private static void End(TimeList timeList)
        {
            Console.Write("========================\n" +
                          "Шаг 4: Сохранение списка\n" +
                          "========================\n\n");
            
            timeList.Save();

            Console.Write("\nНажмите Enter для завершения работы...");
            Console.ReadKey();
        }

        private static int ReadInt()
        {
            int a;
            var cursorTop = Console.CursorTop;
            var cursorLeft = Console.CursorLeft;
            while (!int.TryParse(Console.ReadLine(), out a))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Вы должны ввести целое число!");
                Console.ResetColor();
                Console.SetCursorPosition(cursorLeft, cursorTop);
                ClearLine();
            }

            ClearLine();
            return a;
        }

        private static void ClearLine()
        {
            var cursorLeft = Console.CursorLeft;
            var cursorTop = Console.CursorTop;
            Console.Write(new string(' ', Console.BufferWidth - cursorLeft));
            Console.SetCursorPosition(cursorLeft, cursorTop);
        }
    }
}
