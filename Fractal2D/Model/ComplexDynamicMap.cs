using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Numerics;
using SharpGL;
using Alea;
using Alea.Parallel;

namespace Fractal2D
{
    /// <summary>
    /// Набор рекуррентных функций комплексного переменного на комплексной плоскости, 
    /// точки которой определяют начальные параметры функции
    /// </summary>
    public class ComplexDynamicMap: IDrawn
    {
        /// <summary>
        /// Минимальное значение - левый нижний угол комплексной плоскости
        /// </summary>
        public Complex Minimum { get; set; }
        /// <summary>
        /// Максимальное значение - правый верхний угол комплексной плоскости
        /// </summary>
        public Complex Maximum { get; set; }
        /// <summary>
        /// Охват комплексной плоскости
        /// </summary>
        Complex Range => Maximum - Minimum;
        /// <summary>
        /// Коэффициент зума
        /// </summary>
        public double ZoomK { get; set; } = 3;
        public double Zoom { get; set; } = 1;
        double[,] _values; // ключевой параметр функции, на основе которого будет формироваться отображение
        public double this[int i, int r] { get => _values[i, r]; }
        
        /// <summary>
        /// Количество опорных точек по вещественной оси
        /// </summary>
        public int RCount { get; set; }
        /// <summary>
        /// Количество опорных точек по комплексной оси
        /// </summary>
        public int ICount { get; set; }

        const int ItCount0 = 500; // максимальное количество итераций

        Complex cParam1 = new Complex(-0.4, 0.6);
        Complex paramDelta;
        /// <summary>
        /// Радиус допустимой области
        /// </summary>
        static double GetRadius(Complex cParam) => (1 + Math.Sqrt(1 + 4 * cParam.Module())) / 2;
        public ComplexDynamicMap(Complex min, Complex max, Complex delta, int rCount, int iCount)
        {
            Minimum = min;
            Maximum = max;
            paramDelta = delta;
            RCount = rCount;
            ICount = iCount;
            CalcStage();
        }
        const double EPS = 0.000001; // для коррекции значения в данной точке
        /// <summary>
        /// Вычислить текущее состояние системы
        /// </summary>
        public void CalcStage()
        {
            Complex cRange = Range;
            double dR = cRange.Real / RCount, dI = cRange.Imaginary / ICount, rad = GetRadius(cParam1);
            int countMax = (int)(ItCount0);
            _values = new double[ICount, RCount];

            Parallel
            //Gpu.Default
                .For(0, ICount, (im) =>
            //for(int im = 0;im<ICount;im++)
            {
                int i = im;
                //Console.WriteLine($"iCount = {i}");
                for (int r = 0; r < RCount; r++)
                {
                    /*
                    _values[i, r] = (double)ComplexDynamic.
                    //Julia
                    Mandelbrot
                    (Minimum.Real + dR * r, Maximum.Imaginary - dI * i
                    , cParam1
                    ).IterationCountInsideArea(rad, countMax)/countMax - EPS;
                    */

                    _values[i, r] = (double)ComplexDynamic.
                    IterationCountInsideArea
                    (
                        (z, args) => z * z + args[0],
                        //new Complex(Minimum.Real + dR * r, Maximum.Imaginary - dI * i), cParam1, // Julia
                        Complex.Zero, new Complex(Minimum.Real + dR * r, Maximum.Imaginary - dI * i), // Mandelbrot
                        rad, countMax)/
                        countMax - EPS
                        ;
                }
            }
            );

        }
        static Font font = new Font(Extension.Font, 15F, GraphicsUnit.Pixel);
        public void Draw(Graphics g, GraphicsParam p0)
        {
            var p = p0 as GraphicsParamCDM;
            p.Validate(this);
            if (p.Bitmap == null) return;
            g.DrawImage(p.Bitmap, p.DrawRect);
            g.DrawString($"Zoom = {Zoom:f2}\nParam = {cParam1}", font, Brushes.White, 10, 10);

        }
        public void Draw(OpenGL g, GraphicsParam p0)
        {
            var p = p0 as GraphicsParamCDM;

        }
        public void ZoomIn(Complex point)
        {
            ZoomWorker(point, ZoomK);
        }
        public void ZoomOut(Complex point)
        {
            ZoomWorker(point, 1/ZoomK);
        }
        private void ZoomWorker(Complex p, double k)
        {
            Zoom *= k;
            Complex delta = p - Minimum, newDelta = delta/k, range = Range;
            Minimum = p - newDelta;
            Maximum = Minimum + range / k;
        }
        /// <summary>
        /// Увеличить параметр
        /// </summary>
        public void ParamUp()
        {
            ParamWorker();
        }
        /// <summary>
        /// Уменьшить параметр
        /// </summary>
        public void ParamDown()
        {
            ParamWorker(-1);
        }
        private void ParamWorker(double k = 1)
        {
            cParam1 += k * paramDelta;
            CalcStage();
        }
    }
}
