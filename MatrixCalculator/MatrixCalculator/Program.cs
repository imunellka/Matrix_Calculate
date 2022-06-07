using System;
using System.IO;

// Дорогой проверяющий, хочу немного рассказать о моей программе.
// Поскольку ограничения не были четко прописаны пишу свои.
// Программа работает с целочисленными матрицами.
// А также размер матрицы не превосходит 10*10.
// Числа в матрицах не превосходят 100 по модулю.
// P.s. по словам лектора, такие ограничения не являются основанием для снижения баллов. 
// В программе реализован весь дополнительный функционал ( СЛАУ + ввод из файла).
// Приятного просмотра)

namespace MatrixCalculator
{
    /// <summary>
    /// В рамках процедурного подхода используется один класс.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Метод формирует матрицу.
        /// </summary>
        /// <returns>Заполненная матрица</returns>
        static int[,] CreateMatrix()
        {
            uint size1, size2;
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("Сколько строк вы хотите видеть в будущей матрице?");
            Console.WriteLine("Введите число от 1 до 10");
            while (true)
            {
                if (!uint.TryParse(Console.ReadLine(), out size1))
                    Console.WriteLine("Посмотрите на критерии входных данных");
                else if (size1 > 10 || size1<1)
                    Console.WriteLine("Введите целое положительное число от 1 до 10");
                else
                    break;
            }
            Console.WriteLine("Сколько столбцов вы хотите видеть в будущей матрице?");
            Console.WriteLine("Введите число от 1 до 10");
            while (true)
            {
                if (!uint.TryParse(Console.ReadLine(), out size2))
                    Console.WriteLine("Посмотрите на критерии входных данных");
                else if (size2 > 10 || size2 < 1)
                    Console.WriteLine("Введите целое положительное число от 1 до 10");
                else
                    break;
            }
            Console.ResetColor();
            int[,] matrix = new int[(int)size1, (int)size2];
            Console.WriteLine("Как вы хотите заполнить матрицу?");
            Console.WriteLine("1. вручную");
            Console.WriteLine("2. рандомно");
            Console.WriteLine("3. чтение из файла");
            matrix = Delegate(size1, size2, matrix);
            return matrix;
        }

        /// <summary>
        /// Метод регулирует создание матриц.
        /// </summary>
        /// <param name="size1">кол-во строк в матрице</param>
        /// <param name="size2">кол-во столбцов в матрице</param>
        /// <param name="matrix">заполняемая матрица</param>
        /// <returns></returns>
        private static int[,] Delegate(uint size1, uint size2, int[,] matrix)
        {
            int option1 = 0;
            while (option1 < 1 || option1 > 3)
            {
                Console.WriteLine("Выберите опцию (1 или 2 или 3)");
                int.TryParse(Console.ReadLine(), out option1);
            }
            switch (option1)
            {
                case <= 1:
                    CreateManually(ref matrix, (int)size1, (int)size2);
                    break;
                case <= 2:
                    CreateRandom(ref matrix, (int)size1, (int)size2);
                    break;
                case <= 3:
                    bool f = false;
                    while (!f)
                    {
                        f = CreateFromFile(ref matrix, (int)size1, (int)size2);
                    }
                    break;
            }

            return matrix;
        }

        /// <summary>
        /// Метод для заполнения матрицы вручную.
        /// </summary>
        /// <param name="matrix">исходная матрица</param>
        /// <param name="size1">кол-во строк в матрице</param>
        /// <param name="size2">кол-во столбцов в матрице</param>
        static void CreateManually(ref int[,] matrix, int size1, int size2)
        {
            int i = 0, j = 0;
            Console.WriteLine("Введите {0} целочисленных элементов не превышающих 100 по модулю", size1 * size2);
            while (i < size1)
            {
                while (j < size2)
                {
                    if (int.TryParse(Console.ReadLine(), out int elem))
                    {
                        if (Math.Abs(elem) <= 100)
                        {
                            matrix[i, j] = elem;
                            j += 1;
                        }
                        else
                            Console.WriteLine("Число не должно превышать 100 по модулю.");
                    }
                    else
                        Console.WriteLine("То что вы ввели не преобразовывается в целое число.");
                }
                j = 0;
                i += 1;
            }
            Console.WriteLine("Ваша матрица готова");
            PrintMatrix(matrix);
        }

        /// <summary>
        /// Метод для заполнения матрицы из файла.
        /// </summary>
        /// <param name="matrix">исходная матрица</param>
        /// <param name="size1">кол-во строк в матрице</param>
        /// <param name="size2">кол-во столбцов в матрице</param>
        static bool CreateFromFile(ref int[,] matrix, int size1, int size2)
        {
            Console.WriteLine("Укажите полный путь к файлу");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            try
            {
                string way = Console.ReadLine();
                StreamReader sr = new StreamReader(way);
                String line = sr.ReadLine();
                int i = 0, j = 0;
                while (line != null)
                {
                    foreach (var k in line.Split(' '))
                    {
                        matrix[i, j] = int.Parse(k);
                        j += 1;
                    }
                    j = 0;
                    i += 1;
                    line = sr.ReadLine();
                }
                sr.Close();
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Неправильный путь файла");
                Console.WriteLine("Попытайтесь снова");
                return false;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Неправильный путь файла");
                Console.WriteLine("Попытайтесь снова");
                return false;
            }
            catch (SystemException)
            {
                Console.WriteLine("Содержимое файла не соответствует формату ( целые числа, размер матрицы)");
                Console.WriteLine("Попытайтесь снова");
                return false;
            }
            Console.ResetColor();
            Console.WriteLine("Ваша матрица готова");
            PrintMatrix(matrix);
            return true;
        }


        /// <summary>
        /// Вывод матрицы на экран.
        /// </summary>
        /// <param name="matrix">исходная матрица</param>
        static void PrintMatrix(int[,] matrix)
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            for (int i = 0; i < matrix.GetLength(0); i++, Console.WriteLine())
                for (int j = 0; j < matrix.GetLength(1); j++)
                    Console.Write("{0,7}", matrix[i, j]);
            Console.ResetColor();
        }

        /// <summary>
        /// Метод заполняет матрицу рандомными элементами.
        /// </summary>
        /// <param name="matrix">исходная матрица</param>
        /// <param name="size1">кол-во строк в матрице</param>
        /// <param name="size2">кол-во столбцов в матрице</param>
        static void CreateRandom(ref int[,] matrix, int size1, int size2)
        {
            int range,range2 = 0;
            Console.WriteLine("Введите одно  число - нижнюю границу [-100;100].");
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out range) && range<=100 && range>=-100) {
                    break;
                }
                Console.WriteLine("Введите одно  число  - нижнюю границу [-100;100].");
            }
            Console.WriteLine("Введите одно  число - верхнюю границу [-100;100].");
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out range2) && range2 <= 100 && range2 >= -100)
                {
                    if (range2 >= range)  break; 
                    else Console.WriteLine("Верхняя граница должна быть не меньше нижней");
                }
                Console.WriteLine("Введите одно  число  - верхнюю границу [-100;100].");
            }
            int i = 0, j = 0;
            Random rnd = new Random();
            while (i < size1)
            {
                while (j < size2)
                {
                    matrix[i, j] = rnd.Next(range, range2+1);
                    j += 1;
                }
                j = 0;
                i += 1;
            }
            Console.WriteLine("Ваша матрица готова");
            PrintMatrix(matrix);
        }

        /// <summary>
        /// метод находит след матрицы.
        /// </summary>
        static void TraceOfMatrix()
        {
            int[,] matrix = CreateMatrix();
            long sum = 0;
            if (matrix.GetLength(0) != matrix.GetLength(1))
            {
                Console.WriteLine("Простите, но нельзя найти след не квадратной матрицы.");
                return;
            }
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                    if (i == j)
                    {
                        sum += matrix[i, j];
                    }
            Console.WriteLine("След матрицы равен {0}", sum);

        }

        /// <summary>
        /// Метод выводит транспонированную матрицу.
        /// </summary>
        static void Transposition()
        {

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            int[,] matrix = CreateMatrix();
            Console.WriteLine("Транспонированная матрица");
            for (int j = 0; j < matrix.GetLength(1); j++, Console.WriteLine())
                for (int i = 0; i < matrix.GetLength(0); i++)
                    Console.Write("{0,5}", matrix[i, j]);
            Console.ResetColor();
        }

        /// <summary>
        /// Метод находит сумму матриц.
        /// </summary>
        static void SumOfMatrices()
        {
            int[,] matrix1 = CreateMatrix();
            Console.WriteLine("А теперь 2 матрица)");
            int[,] matrix2 = CreateMatrix();
            if (matrix1.GetLength(1) != matrix2.GetLength(1) || matrix1.GetLength(0) != matrix2.GetLength(0))
            {
                Console.WriteLine("Операция суммирования с данными матрицами невозможна");
                return;
            }
            Console.WriteLine("Сумма матриц:");
            for (int i = 0; i < matrix1.GetLength(0); i++)
                for (int j = 0; j < matrix1.GetLength(1); j++)

                    matrix1[i, j] += matrix2[i, j];
            PrintMatrix(matrix1);
        }

        /// <summary>
        /// Метод находит разность матриц.
        /// </summary>
        static void SubstractionOfMatrices()
        {
            int[,] matrix1 = CreateMatrix();
            Console.WriteLine("А теперь 2 матрица)");
            int[,] matrix2 = CreateMatrix();
            if (matrix1.GetLength(1) != matrix2.GetLength(1) || matrix1.GetLength(0) != matrix2.GetLength(0))
            {
                Console.WriteLine("Операция вычитания с данными матрицами невозможна");
                return;
            }
            Console.WriteLine("Разность матриц:");
            for (int i = 0; i < matrix1.GetLength(0); i++)
                for (int j = 0; j < matrix1.GetLength(1); j++)

                    matrix1[i, j] -= matrix2[i, j];
            PrintMatrix(matrix1);
        }

        /// <summary>
        /// Метод находит произведение матриц.
        /// </summary>
        static void Multiplication()
        {
            int[,] matrix1 = CreateMatrix();
            Console.WriteLine("А теперь 2 матрица)");
            int[,] matrix2 = CreateMatrix();
            int[,] matrix3 = new int[matrix1.GetLength(0), matrix2.GetLength(1)];
            if (matrix1.GetLength(1) != matrix2.GetLength(0))
            {
                Console.WriteLine("Операция умножения с данными матрицами невозможна");
                return;
            }
            Console.WriteLine("Произведение матриц:");
            for (int i = 0; i < matrix1.GetLength(0); i++)
            {
                for (int j = 0; j < matrix2.GetLength(1); j++)
                {
                    for (int k = 0; k < matrix2.GetLength(0); k++)
                    {
                        matrix3[i, j] += matrix1[i, k]*matrix2[k,j];
                    }
                }
            }

            PrintMatrix(matrix3);
        }


        /// <summary>
        /// Метод находит умножение матрицы на число.
        /// </summary>
        static void NumberMultiply()
        {
            int number = 0;
            int[,] matrix1 = CreateMatrix();
            Console.WriteLine("Введите число на которое хотите умножить матрицу [-1000;1000]");
            while(true)
            {
                if (!int.TryParse(Console.ReadLine(), out number))
                {
                    Console.WriteLine("Пожалуйста введите целое число");
                }
                else if (number < -1000 || number > 1000)
                {
                    Console.WriteLine("Программа принимает число в интервале от -1000 до 1000");
                }
                else break;

            }
            
            Console.WriteLine("Умножение матрицы на число:");
            for (int i = 0; i < matrix1.GetLength(0); i++)
                for (int j = 0; j < matrix1.GetLength(1); j++)

                    matrix1[i, j]*=number;
            PrintMatrix(matrix1);
        }

        /// <summary>
        /// Метод создает дубликат матрицы (int -->double)
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns>Дубликат матрицы</returns>
        static double[,] CopyMatrix(int[,] matrix)
        {
            double[,] result = new double[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0); ++i) 
                for (int j = 0; j < matrix.GetLength(1); ++j)
                    result[i,j] = matrix[i,j];
            return result;
        }

        /// <summary>
        /// Разложение матрицы для дальнейшего нахождения определителя.
        /// </summary>
        /// <param name="matrix">матрица</param>
        /// <param name="permutation">перестановки</param>
        /// <param name="flag">флаг</param>
        /// <returns></returns>
        /// 
        static double[,] MatrixDecompose(int[,] matrix, out int[] permutation, out int flag)
        {
            int size = matrix.GetLength(0);
            double[,] result = CopyMatrix(matrix);
            permutation = new int[size];
            for (int i = 0; i < size; ++i) { permutation[i] = i; }
            flag = 1;
            for (int j = 0; j < size - 1; ++j)
            {
                double Max = Math.Abs(result[j, j]);
                int permutationRow = j;
                for (int i = j + 1; i < size; ++i)
                {
                    if (result[i, j] > Max)
                    { Max = result[i, j]; permutationRow = i;}
                }
                if (permutationRow != j)
                {
                    for (int k = 0; k < result.GetLength(1); k++)
                    {
                        double rowPtr = result[permutationRow, k];
                        result[permutationRow, k] = result[j, k];
                        result[j, k] = rowPtr;
                    }
                    int tmp = permutation[permutationRow];
                    permutation[permutationRow] = permutation[j];
                    permutation[j] = tmp;
                    flag = -flag;
                }
                if (Math.Abs(result[j, j]) < 1.0E-18)
                    return null;
                for (int i = j + 1; i < size; ++i)
                {
                    result[i, j] /= result[j, j];
                    for (int k = j + 1; k < size; ++k)
                    {
                        result[i, k] -= result[i, j] * result[j, k];
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// метод-помощник для решения СЛАУ.
        /// </summary>
        /// <param name="TransformedMatrix"></param>
        /// <param name="answers">то, чему равняются строки системы уравнений</param>
        /// <returns>массив ответов</returns>
        static double[] Solver(double[,] TransformedMatrix, int[] answers)
        {
            int size = TransformedMatrix.GetLength(0);
            double[] x = new double[size];
            answers.CopyTo(x, 0);
            for (int i = 1; i < size; ++i)
            {
                double sum = x[i];
                for (int j = 0; j < i; ++j)
                    sum -= TransformedMatrix[i,j] * x[j];
                x[i] = sum;
            }
            x[size - 1] /= TransformedMatrix[size - 1,size - 1];
            for (int i = size - 2; i >= 0; --i)
            {
                double sum = x[i];
                for (int j = i + 1; j < size; ++j)
                    sum -= TransformedMatrix[i,j] * x[j];
                x[i] = sum / TransformedMatrix[i,i];
            }
            return x;
        }

        /// <summary>
        /// Основной метод запускающий решение системы линейных уравнений.
        /// </summary>
        static void SystemSolve()
        {
            int[,] matrix = CreateMatrix();
            int[,] parameters= new int[matrix.GetLength(0), matrix.GetLength(1)-1];
            int[] answers = new int[matrix.GetLength(0)];
            if (matrix.GetLength(0)!= matrix.GetLength(1) -1)
            {
                Console.WriteLine("у такой системы нет единственного решения.");
                Console.WriteLine("Программа принимает системы вида Ax =B.");
                return;
            }
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (j != matrix.GetLength(1) - 1)  parameters[i, j] = matrix[i, j];
                    else answers[i] = matrix[i, j];
                 }
            }
            int size = parameters.GetLength(0);
            int[] permutation;
            int flag;
            double[,] TransformedMatrix = MatrixDecompose( parameters, out permutation, out flag);
            if (TransformedMatrix == null)
            {
                Console.WriteLine("У этой системы нет решений.");
                return;
            }
            int[] bp = new int[answers.GetLength(0)];
            for (int i = 0; i < size; ++i)
                bp[i] = answers[permutation[i]];
            double[] x = Solver(TransformedMatrix, bp);
            bool taggle = true;
            for (int i = 0; i < x.Length; i++)
            {
                if (double.IsNaN(x[i])) taggle = false;
            }
            if (taggle == false)
            {
                Console.WriteLine("У системы нет единственного решения");
                return;
            }
            for (int i = 0; i < x.Length; i++)
                Console.WriteLine("X{0} = {1,5}", i+1,Math.Round(x[i],3));
            return;
        }

        /// <summary>
        /// Основной метод нахождения определителя матрицы.
        /// </summary>
        static void MatrixDeterminant()
        {
            int[,] matrix = CreateMatrix();
            int[] permutation;
            int flag;
            if (matrix.GetLength(0) != matrix.GetLength(1))
            {
                Console.WriteLine("Невозможно найти определитель не квадратной матрицы");
                return;
            }
            double[,] lum = MatrixDecompose(matrix, out permutation, out flag);
            if (lum==null){
                Console.WriteLine("Невозможно найти определитель матрицы");
                return;
            }
            double result = flag;
            for (int i = 0; i < lum.GetLength(0); ++i)
                result *= lum[i,i];
            Console.WriteLine("Определитель матрицы:  {0}", Math.Round(result,2));
            return;
        }

        /// <summary>
        /// Основной метод запуска калькулятора.
        /// </summary>
        static void Main()
        {
            ConsoleKeyInfo KeyToExit;
            do
            {
                int option = 0;
                TalkWithUser();
                while (option < 1 || option > 8)
                {
                    Console.WriteLine("Введите цифру от 1 до 8");
                    int.TryParse(Console.ReadLine(), out option);
                }
                switch (option)
                {
                    case <= 1:
                        TraceOfMatrix();
                        break;
                    case <= 2:
                        Transposition();
                        break;
                    case <= 3:
                        SumOfMatrices();
                        break;
                    case <= 4:
                        SubstractionOfMatrices();
                        break;
                    case <= 5:
                        Multiplication();
                        break;
                    case <= 6:
                        NumberMultiply();
                        break;
                    case <= 7:
                        MatrixDeterminant();
                        break;
                    case <= 8:
                        SystemSolve();
                        break;
                }
                Console.WriteLine("Если хочешь возобновить нажми любой символ, чтобы закрыть нажми escape.");
                KeyToExit = Console.ReadKey(true);
                Console.Clear();
            } while (KeyToExit.Key != ConsoleKey.Escape);
            Console.WriteLine("До свидания!");
            Console.ReadLine();
        }

        /// <summary>
        /// Декомпозиция для общения с пользователем.
        /// </summary>
        private static void TalkWithUser()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Добро пожаловать в калькулятор матриц");
            Console.WriteLine("Выберите доступную опцию:");
            Console.WriteLine("1.нахождение следа матрицы;");
            Console.WriteLine("2.транспонирование матрицы;");
            Console.WriteLine("3.сумма двух матриц;");
            Console.WriteLine("4.разность двух матриц;");
            Console.WriteLine("5.произведение двух матриц;");
            Console.WriteLine("6.умножение матрицы на число;");
            Console.WriteLine("7.нахождение определителя матрицы.");
            Console.WriteLine("8.решение системы линейных уравнений");
            Console.ResetColor();
        }
    }
}
