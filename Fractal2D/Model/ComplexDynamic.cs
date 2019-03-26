using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;
using SharpGL;

namespace Fractal2D
{
    /// <summary>
    /// Динамика рекурсивной функции комплексного переменного
    /// </summary>
    public class ComplexDynamic: IAnimated
    {
        /// <summary>
        /// Универсальная функция комплексного переменного
        /// </summary>
        /// <param name="z">Аргумент</param>
        /// <param name="args">Коэффициенты</param>
        /// <returns></returns>
        public delegate Complex ComplexFunction(Complex z, params Complex[] args);
        /// <summary>
        /// Полином 2-ой степени без линейной компоненты
        /// </summary>
        public static ComplexFunction SimpleQuadratic = (z, args) => z * z + args[0];
        public int Count => _values.Count;
        public Complex this[int index] { get => _values[index]; }
        List<Complex> _values = new List<Complex>();
        public static int IterationCountInsideArea(ComplexFunction f, Complex z0, Complex param, double area, int countMax)
        {
            int c = 0;
            while (z0.Module() < area && ++c < countMax) z0 = f(z0, param);
            return c;
        }
        /// <summary>
        /// Нормализованный параметр удаленности
        /// </summary>
        public static double DistsanceOutArea(ComplexFunction f, Complex z0, Complex param, double area, int countMax)
        {
            int c = 0;
            while (z0.Module() < area && ++c < countMax) z0 = f(z0, param);
            return 0.5 + Math.Atan(-z0.Module()) / Math.PI;
        }
        /// <summary>
        /// Текущее значение
        /// </summary>
        public Complex Value { get => _values.Last(); }
        public Complex[] Params { get; set; }
        ComplexFunction _func;
        /// <summary>
        /// Ряд рекурсивной функции комплексного переменного
        /// </summary>
        /// <param name="function">Функция комплексного переменного</param>
        /// <param name="z0">Начальное значение</param>
        /// <param name="param">Константы</param>
        public ComplexDynamic(ComplexFunction function, Complex z0, params Complex[] param)
        {
            _values.Add(z0);
            _func = function;
            Params = new Complex[param.Length];
            for (int i = 0; i < param.Length; i++) Params[i] = param[i];
        }
        public void NextStage()
        {
            _values.Add(_func(_values.Last(), Params));
        }
        /// <summary>
        /// Динамика Мандельброта
        /// </summary>
        public static ComplexDynamic Mandelbrot(double r, double im, Complex c)
        {
            return new ComplexDynamic(SimpleQuadratic, Complex.Zero, new Complex(r, im));
        }
        /// <summary>
        /// Динамика Жюлиа
        /// </summary>
        /// <param name="c">Коэффициент</param>
        /// <returns></returns>
        public static ComplexDynamic Julia(double r, double im, Complex c)
        {
            return new ComplexDynamic(SimpleQuadratic, new Complex(r, im), c);
        }
        /// <summary>
        /// Возвращает количество итераций внутри радиуса
        /// </summary>
        /// <param name="areaRadius"></param>
        /// <returns></returns>
        public int IterationCountInsideArea(double areaRadius, int countMax)
        {
            // пока итеративный путь внутри зоны
            while (_values.Last().Module() <= areaRadius && _values.Count<countMax) NextStage();
            return _values.Count;
        }
        /// <summary>
        /// Обратное соотношение приращения координат после выхода из области
        /// </summary>
        /// <param name="areaRadius"></param>
        /// <returns></returns>
        public double IncreasingAfterLeavingArea(double areaRadius, int countMax)
        {
            // пока итеративный путь внутри зоны
            while (_values.Last().Module() <= areaRadius && _values.Count<countMax) NextStage();
            return (this[Count - 3] - this[Count - 2]).Module() / (this[Count - 2] - this[Count - 1]).Module();
        }
        public void Draw(Graphics g, GraphicsParam p)
        {

        }
        public void Draw(OpenGL g, GraphicsParam p)
        {

        }
    }
}
