#ifdef MATRIXLIB_EXPORTS
#define MATRIXLIB_API extern "C" __declspec(dllexport) 
#else
#define MATRIXLIB_API __declspec(dllimport) 
#endif

namespace MatrixLib
{
	class Matrix {
		int size;
		double *a, *b, *c;
	public:
		Matrix(int size);
		Matrix(int size, double* a, double* b, double* c);
		Matrix(const Matrix& other);
		~Matrix();
		Matrix& operator=(const Matrix& other);
		void solveSystem(double* f, double* x);
		void show(double* f, double* x);
	};

	static int getInt(int start, int end);

	// Ёкспортируемые функции
	MATRIXLIB_API double CountTime(int size, int count);
	MATRIXLIB_API void SolveSystem(int size, double *a, double *b, double *c, double *f, double *x);
}
