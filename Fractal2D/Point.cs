using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fractal2D
{
    public class Point<T> where T : struct, IComparable, IFormattable
    {
        public T X { get; set; }
        public T Y { get; set; }
        public Point(T x, T y)
        {
            X = x;
            Y = y;
        }
        public override string ToString()
        {
            return $"Point<{X.GetType()}>(X = {X}, Y = {Y}";
        }
    }
}
