// MatrixLib.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "MatrixLib.h"

#include <ctime>
#include <iostream>
#include <iomanip>

namespace MatrixLib
{
	int getInt(int start, int end) {
		return (start + rand() % (end - start + 1)) * (rand() % 2 == 0 ? 1 : -1);
	}

	Matrix::Matrix(int size) {
		this->size = size;

		a = new double[size - 1];
		b = new double[size - 1];
		c = new double[size];

		srand(time(nullptr));

		for (auto i = 0; i < size; i++) {
			if (i < size - 1) {
				a[i] = getInt(0, 9);
				b[i] = getInt(0, 9);
			}

			c[i] = getInt(10, 20);
		}
	}

	Matrix::Matrix(int size, double* a, double* b, double* c) {
		this->size = size;
		this->a = new double[size - 1];
		this->b = new double[size - 1];
		this->c = new double[size];

		for (auto i = 0; i < size; i++) {
			if (i < size - 1) {
				this->a[i] = a[i];
				this->b[i] = b[i];
			}

			this->c[i] = c[i];
		}
	}

	Matrix::Matrix(const Matrix& other) {
		size = other.size;
		a = new double[size - 1];
		b = new double[size - 1];
		c = new double[size];

		for (auto i = 0; i < size; i++) {
			if (i < size - 1) {
				a[i] = other.a[i];
				b[i] = other.b[i];
			}

			c[i] = other.c[i];
		}
	}

	Matrix::~Matrix() {
		delete[] a;
		delete[] b;
		delete[] c;
	}

	Matrix& Matrix::operator=(const Matrix& right) {
		delete[] a;
		delete[] b;
		delete[] c;

		size = right.size;
		a = new double[size - 1];
		b = new double[size - 1];
		c = new double[size];
		for (auto i = 0; i < size; i++) {
			if (i < size - 1) {
				a[i] = right.a[i];
				b[i] = right.b[i];
			}

			c[i] = right.c[i];
		}

		return *this;
	}

	void Matrix::solveSystem(double* f, double* x) {
		auto beta = new double[size - 1];
		auto gamma = new double[size];

		beta[0] = b[0] / c[0];
		gamma[0] = f[0] / c[0];

		// Вычисление прогоночных коэффициентов
		for (auto i = 1; i < size; i++) {
			auto denominator = (c[i] - beta[i - 1] * a[i - 1]);
			if (i < size - 1) {
				beta[i] = b[i] / denominator;
			}
			gamma[i] = (f[i] - a[i - 1] * gamma[i - 1]) / denominator;
		}

		// Нахождение решения
		x[size - 1] = gamma[size - 1];
		for (auto i = size - 2; i >= 0; i--) {
			x[i] = gamma[i] - beta[i] * x[i + 1];
		}

		delete[] beta;
		delete[] gamma;
	}

	void Matrix::show(double* f, double* x) {
		for (auto i = 0; i < size; i++) {
			for (auto j = 0; j < size; j++) {
				std::cout << std::setw(4);
				if (i == j) {
					std::cout << this->c[i];
				}
				else if (i + 1 == j) {
					std::cout << this->b[i];
				}
				else if (i == j + 1) {
					std::cout << this->a[j];
				}
				else {
					std::cout << 0;
				}
			}
			std::cout << " | " << std::setw(4) << f[i] << " || " << x[i] << "\n";
		}
	}

    double CountTime(int size, int count) {
	    auto matrix = new Matrix(size);
	    auto f = new double[size];
	    auto x = new double[size];
		for (auto i = 0; i < size; i++) {
			f[i] = getInt(0, 100);
		}

		time_t start = clock();
		for (auto i = 0; i < count; i++) {
			matrix->solveSystem(f, x);
		}

		auto a = static_cast<double>(clock() - start) / CLOCKS_PER_SEC;

		delete matrix;
		delete[] f;
		delete[] x;
	
		return a;
	}

    void SolveSystem(int size, double* a, double* b, double* c, double* f, double* x) {
	    auto matrix = new Matrix(size, a, b, c);
		matrix->solveSystem(f, x);
		matrix->show(f, x);

		delete matrix;
	}
}