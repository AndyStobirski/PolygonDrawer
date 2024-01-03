using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class ShapePolygon :ShapePolygonBase
    {
        public ShapePolygon(int pX, int pY) : base()
        {
            IsPolygon = true;

            float width = 50;
            double angle = 2 * Math.PI / 5; // Angle between each point

            Points = new List<PointF>();

            for (int i = 0; i < 5; i++)
            {
                float x = (float)(pX + width * Math.Cos(i * angle));
                float y = (float)(pY + width * Math.Sin(i * angle));
                Points.Add(new PointF(x, y));
            }
        }

        /// <summary>
        /// Delete the selected handle
        /// </summary>
        public void DeleteHandle()
        {
            if (Points.Count>3)
            {
                Points.RemoveAt(HandleHit);
                ItemHit = SelectionPartHit.none;
            }
        }
    }
}
