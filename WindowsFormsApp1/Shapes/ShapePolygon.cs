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
        public Color _BoundaryColour { get; set; } = Color.LightBlue;
        private Pen _BoundaryPen => new Pen(LineColour, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash } ;

        public ShapePolygon(int pX, int pY) : base()
        {
            IsPolygon = true;

            float width = 50;
            double angle = 2 * Math.PI / 5; // Angle between each point

            // Adjust starting angle to create a flat bottom edge
            double startAngle = -Math.PI / 2;

            Points = new List<PointF>();

            for (int i = 0; i < 5; i++)
            {
                float x = (float)(pX + width * Math.Cos(startAngle + i * angle));
                float y = (float)(pY + width * Math.Sin(startAngle + i * angle));
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

        /// <summary>
        /// A handle has been added on the line who's index is specified by LineHit
        /// </summary>
        /// <param name="pX"></param>
        /// <param name=""></param>
        public void AddHandle(int pX, int pY)
        {
            var lines = CalculateLines();

            // the index of LineHit corresponds the Points index of the point which begins it
            // and we
            PointF newPoint = new PointF(pX, pY);

            Points.Insert(LineHit + 1, newPoint);

        }

        public override void Draw(Graphics g)
        {
            base.Draw(g);

            if (Selected)
            {
                g.DrawRectangle(_BoundaryPen, GetBoundingRectangle());
            }

        }
    }
}
