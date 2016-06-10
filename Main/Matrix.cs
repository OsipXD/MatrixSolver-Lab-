using System;

namespace Main
{
    class Matrix
    {
        private readonly int _size;
        private readonly double[] _a, _b, _c;

        public Random Random { get; } = new Random();

        public Matrix(int size)
        {
            _size = size;
            _a = new double[size - 1];
            _b = new double[size - 1];
            _c = new double[size];

            for (int i = 0; i < size; i++)
            {
                if (i < size - 1)
                {
                    _a[i] = Random.Next(0, 9);
                    _b[i] = Random.Next(0, 9);
                }

                _c[i] = Random.Next(10, 20);
            }
        }

        public Matrix(int size, double[] a, double[] b, double[] c)
        {
            _size = size;
            _a = (double[]) a.Clone();
            _b = (double[]) b.Clone();
            _c = (double[]) c.Clone();
        }

        public double[] SolveSystem(double[] f)
        {
            var beta = new double[_size - 1];
            var gamma = new double[_size];
            var x = new double[_size];

            beta[0] = _b[0] / _c[0];
            gamma[0] = f[0] / _c[0];

            // Вычисление прогоночных коэффициентов
            for (var i = 1; i < _size; i++)
            {
                var denominator = (_c[i] - beta[i - 1] * _a[i - 1]);

                if (i < _size - 1)
                {
                    beta[i] = _b[i] / denominator;
                }
                gamma[i] = (f[i] - _a[i - 1] * gamma[i - 1]) / denominator;
            }

            // Нахождение решения
            x[_size - 1] = gamma[_size - 1];
            for (var i = _size - 2; i >= 0; i--)
            {
                x[i] = gamma[i] - beta[i] * x[i + 1];
            }

            return x;
        }

        public void Show(double[] f, double[] x)
        {
            for (var i = 0; i < _size; i++)
            {
                for (var j = 0; j < _size; j++)
                {
                    if (i == j)
                    {
                        Console.Write("{0, 4}", _c[i]);
                    }
                    else if (i + 1 == j)
                    {
                        Console.Write("{0, 4}", _b[i]);
                    }
                    else if (i == j + 1)
                    {
                        Console.Write("{0, 4}", _a[j]);
                    }
                    else
                    {
                        Console.Write("{0, 4}", 0);
                    }
                }

                Console.WriteLine(" | {0, 4} || {1}", f[i], x[i]);
            }
        }

        public override string ToString()
        {
            return base.ToString() + "[size = " + _size + "]";
        }
    }
}