using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Fractal2D
{
    /// <summary>
    /// Универсальные параметры отображения
    /// </summary>
    public class GraphicsParam
    {
        /// <summary>
        /// Координата X начальной точки
        /// </summary>
        float X { get; set; }
        /// <summary>
        /// Координата Y начальной точки
        /// </summary>
        float Y { get; set; }
        /// <summary>
        /// Зум
        /// </summary>
        public float Zoom { get; set; } = 1;
        float _zoomK { get; set; } = 1.1F; // коэффициент зумирования
        /// <summary>
        /// Текущее количество кадров в секунду
        /// </summary>
        public int FPS { get; set; }
        /// <summary>
        /// Текущее количество этапов на фрейм
        /// </summary>
        public int SPF { get; set; }
        /// <summary>
        /// Прямоугольник окна
        /// </summary>
        public SizeF ClientSize { get; set; }
        /// <summary>
        /// Прямоугольник отображения
        /// </summary>
        public RectangleF DrawRect { get; set; }
        public GraphicsParam(RectangleF rect)
        {
            DrawRect = rect;
        }
        /// <summary>
        /// Обновить линейные размеры
        /// </summary>
        public void SetRect(float w, float h, float margin)
        {
            DrawRect = new RectangleF(margin, margin, w - 2 * margin, h - 2 * margin);
            ClientSize = new SizeF(w, h);
        }
        /// <summary>
        /// Приблизить отображение в данной точке
        /// </summary>
        public void ZoomIn(float x, float y)
        {
            ZoomWorker(x, y, _zoomK);
        }
        /// <summary>
        /// Отдалить отображение в данной точке
        /// </summary>
        public void ZoomOut(float x, float y)
        {
            ZoomWorker(x, y, 1 / _zoomK);
        }
        private void ZoomWorker(float x, float y, float k)
        {
            // текущие расстояния до краев отображения
            float distToLeft = x - X, distToTop = y - Y;
            // обновить крайние точки
            X = x - k * distToLeft;
            Y = y - k * distToTop;
        }
        /// <summary>
        /// Сместить обзор попиксельно
        /// </summary>
        public void ShiftByPixel(int dx, int dy)
        {
            ShiftWorker(dx, dy);
        }
        /// <summary>
        /// Сместить начальную точку
        /// </summary>
        /// <param name="x">Смещение по X</param>
        /// <param name="y">Смещенеи по Y</param>
        private void ShiftWorker(float x, float y)
        {
            X += x;
            Y += y;
        }
        /// <summary>
        /// Малая величина для нивелировки погрешности вычислений в float
        /// </summary>
        const float EPS = 0.00001F;

    }
}
