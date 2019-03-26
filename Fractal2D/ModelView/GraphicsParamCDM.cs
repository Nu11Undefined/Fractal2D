using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;
using System.Drawing.Imaging;

namespace Fractal2D
{
    /// <summary>
    /// Параметры отображения систем функций комплексного переменного
    /// </summary>
    public class GraphicsParamCDM: GraphicsParam
    {
        /// <summary>
        /// Отображение рисунка
        /// </summary>
        public Bitmap Bitmap { get; set; }
        Color [] Colors { get; set; }
        public GraphicsParamCDM(RectangleF rect, Color[] colors):base(rect)
        {
            Colors = colors.Copy();
        }
        /// <summary>
        /// Сформировать графику
        /// </summary>
        /// <param name="model">Математическая модель</param>
        public void Validate(ComplexDynamicMap model)
        {
            if (Bitmap != null) Bitmap.Dispose();
            Bitmap = new Bitmap(model.RCount, model.ICount, PixelFormat.Format24bppRgb);
            var bd = Bitmap.LockBits(Bitmap.GetRect(), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            unsafe
            {
                double c;
                Color col;
                int c1;
                byte* p = (byte*)bd.Scan0;
                int index=0;
                int delta = bd.Stride - bd.Width * 3;
                for (int i = 0; i < bd.Height; i++, index+=delta)
                {
                    for (int r = 0; r < bd.Width; r++)
                    {
                        c = (Colors.Length - 1) * model[i, r];
                        if (c <0) c = 0;
                        c1 = (int)Math.Floor(c);
                        col = Extension.Interpolate(Colors[c1], Colors[c1 + 1], c % 1.0);
                        p[index++] = col.B;
                        p[index++] = col.G;
                        p[index++] = col.R;
                    }
                }
            }
            Bitmap.UnlockBits(bd);
            //Bitmap.Save("2.png");
        }
        public Complex ScreenToModel(int x, int y, ComplexDynamicMap model)
        {
            return new Complex(
                model.Minimum.Real + (model.Maximum.Real - model.Minimum.Real) * (x - DrawRect.Left) / DrawRect.Width,
               model.Maximum.Imaginary - (model.Maximum.Imaginary - model.Minimum.Imaginary) * (y - DrawRect.Top) / DrawRect.Height);
        }
    }
}
