using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class ShapeRectangle: ShapePolygonBase
    {
        public ShapeRectangle(int pX, int pY): base()
        {
            IsPolygon = false;

            //specify TL, TR, BR, TL
            Points = new List<PointF> { new Point(pX, pY), new Point(pX + 32, pY), new Point(pX + 32, pY + 32), new Point(pX, pY + 32) };
        }
    }
}
