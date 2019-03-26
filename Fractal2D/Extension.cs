using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SharpGL;
using SharpGL.Enumerations;
using System.Numerics;

namespace Fractal2D
{
    /// <summary>
    /// Дополнительные функции
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// Простая функция получения модуля комплексного числа
        /// </summary>
        /// <param name="c">Комплексное число</param>
        public static double Module(this Complex c)
        {
            return Math.Sqrt(c.Real * c.Real + c.Imaginary * c.Imaginary);
        }
        /// <summary>
        /// Копирует массив структур
        /// </summary>
        public static T[] Copy<T>(this T[] array) where T:struct
        {
            T[] b = new T[array.Length];
            for (int i = 0; i < array.Length; i++) b[i] = array[i];
            return b;
        }
        private static byte Interpolate(byte v1, byte v2, double k)
        {
            return (byte)(v1 + (v2 - v1) * k);
        }

        /// <summary>
        /// Интерполяция цвета
        /// </summary>
        public static Color Interpolate(Color c1, Color c2, double k)
        {
            return
                Color.FromArgb(
                    Interpolate(c1.A, c2.A, k),
                    Interpolate(c1.R, c2.R, k),
                    Interpolate(c1.G, c2.G, k),
                    Interpolate(c1.B, c2.B, k)
                    );
        }
        public static string Font = "Consolas";
        /// <summary>
        /// Получить прямоугольник относительно размеров
        /// </summary>
        /// <param name="r">Исходный прямоугольник</param>
        /// <param name="k">Прямоугольник коэффициентов</param>
        /// <returns></returns>
        public static RectangleF Mult(RectangleF r, RectangleF k)
        {
            return new RectangleF(r.X + r.Width * k.X, r.Y + r.Height * k.Y, r.Width * k.Width, r.Height * k.Height);
        }
        public static Rectangle GetRect(this Bitmap bmp)
        {
            return new Rectangle(0, 0, bmp.Width, bmp.Height);
        }

        public static void FillRectangle(this OpenGL gl, Color c, float x, float y, float w, float h)
        {
            gl.SetColor(c);
            gl.Begin(BeginMode.Polygon);
            gl.Vertex(x, y);
            gl.Vertex(x+w, y);
            gl.Vertex(x+w, y+h);
            gl.Vertex(x, y+h);
            gl.End();
        }
        public static void DrawLine(this OpenGL gl, Color c, float lineW, float x1, float y1, float x2, float y2)
        {
            gl.SetColor(c);
            gl.LineWidth(lineW);
            gl.Begin(BeginMode.LineLoop);
            gl.Vertex(x1, y1);
            gl.Vertex(x2, y2);
            gl.End();
        }
        public static void DrawRectangle(this OpenGL gl, Color c, float lineW, float x, float y, float w, float h)
        {
            gl.SetColor(c);
            gl.LineWidth(lineW);
            gl.Vertex(x, y);
            gl.Vertex(x + w, y);
            gl.Vertex(x + w, y + h);
            gl.Vertex(x, y + h);
            gl.End();
        }
        public static void SetColor(this OpenGL gl, Color c)
        {
            gl.Color(c.R/255F, c.G/255F, c.B/255F, c.A/255F);
        }
        /// <summary>
        /// Проецирует отображение OpenGL на экран. 
        /// Позволит использовать экранные координаты
        /// </summary>
        public static void ProjectToScreen(this OpenGL gl, float w, float h)
        {
            gl.Viewport(0, 0, (int)w, (int)h);
            gl.MatrixMode(MatrixMode.Projection);
            gl.LoadIdentity();
            gl.Ortho(0, w, h, 0, -1, 1);
            gl.MatrixMode(MatrixMode.Modelview);
        }

    }
}
