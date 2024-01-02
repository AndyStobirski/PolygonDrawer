using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WindowsFormsApp1
{

    public enum ShapeItemHit  {Body, Handle, Line, None };

    public class Poly : BaseShape
    {
        public Poly (bool pIsRectangle, int pX, int pY    )
        {
            IsRectangle = pIsRectangle;

            //specify TL, TR, BR, TL
            Points = new List<PointF> { new Point(pX, pY), new Point(pX + 32, pY), new Point(pX + 32, pY+32), new Point(pX, pY + 32) };

        }

        public Color LineColour { get; set; } = Color.Black;
        public Color HandleColour { get; set; } = Color.Black;
        public Color HandleFill { get; set; } = Color.White;
        public Color BodyFill { get; set; } = Color.Blue;
        private Pen _Handlepen => new Pen(HandleColour, 2);
        private Pen _LinePen => new Pen(LineColour, 2);
        private SolidBrush _HandleBrush => new SolidBrush(HandleFill);



        private SolidBrush _BodyBrush => new SolidBrush(BodyFill);

        private int _handleSize = 8;

        /// <summary>
        /// Define handles, dependant on IsRectangle
        /// </summary>
        /// <returns></returns>
        private Rectangle[] Handles()
        {
            if (IsRectangle)  // poly draws handles on points
            {
                return Points.Select(p => new Rectangle((int)p.X - _handleSize / 2, (int)p.Y - _handleSize / 2, _handleSize, _handleSize)).ToArray();
            }
            else
            {
                //rectangle draws handles in middle of side
                return new Rectangle[]
                {
                    new Rectangle((int)(Points[0].X + (Points[1].X - Points[0].X)/2) - _handleSize / 2 ,(int)Points[0].Y - _handleSize / 2 ,_handleSize,_handleSize   ) //Top middle
                    , new Rectangle((int)(Points[0].X + (Points[1].X - Points[0].X)) - _handleSize / 2 ,(int)(Points[0].Y + (Points[2].Y - Points[0].Y)/2) - _handleSize / 2 ,_handleSize,_handleSize   ) //Right middle
                    , new Rectangle((int)(Points[0].X + (Points[1].X - Points[0].X)/2) - _handleSize / 2 ,(int)(Points[0].Y + (Points[2].Y - Points[0].Y)) - _handleSize / 2 ,_handleSize,_handleSize   ) //bottom middle
                    , new Rectangle((int)(Points[0].X ) - _handleSize / 2 ,(int)(Points[0].Y + (Points[2].Y - Points[0].Y)/2) - _handleSize / 2 ,_handleSize,_handleSize   ) //Left middle
                };
            }    
        }

        


        /// <summary>
        /// Portion of the poly hit
        /// </summary>
        public ShapeItemHit ItemHit { get; private set; }

        /// <summary>
        /// Index of the handle hit
        /// </summary>
        public int HandleHit { get; private set; }

        /// <summary>
        /// The index of the bounding line hit
        /// </summary>
        public int LineHit { get; private set; }



        /// <summary>
        /// Points that comprise the poly
        /// </summary>
        public List<PointF> Points;

        private List<Line> CalculateLines()
        {
            List<Line>  Lines = new List<Line>();

            for (int ctr = 0; ctr < Points.Count - 1; ctr++)
            {
                Lines.Add(new Line() { X1 = Points[ctr].X, Y1 = Points[ctr].Y, X2 = Points[ctr + 1].X, Y2 = Points[ctr + 1].Y });
            }
            Lines.Add(new Line() { X1 = Points.Last().X, Y1 = Points.Last().Y, X2 = Points.First().X, Y2 = Points.First().Y });

            return Lines;
        }


        /// <summary>
        /// Rectangle which bounds the polygon
        /// </summary>
        /// <returns></returns>
        public Rectangle BoundingRectangle
        {
            get
            {
                Rectangle r = new Rectangle();

                r.X = (int)Points.Select(p => p.X).Min();
                r.Y = (int)Points.Select(p => p.Y).Min();

                int x1 = (int)Points.Select(p => p.X).Max();
                int y1 = (int)Points.Select(p => p.Y).Max();

                r.Size = new Size(x1 - r.X, y1 - r.Y);

                return r;
            }
        }

        /// <summary>
        /// Build a shape path which represents poly for hit testing
        /// </summary>
        /// <returns></returns>
        private GraphicsPath ShapePath()
        {
            GraphicsPath path = new GraphicsPath();

            foreach (var line in CalculateLines())
            {
                path.AddLine(line.X1, line.Y1, line.X2, line.Y2);
            }

            return path;
        }


        /// <summary>
        /// If false, it's a rectangle, else it's a polygo
        /// </summary>
        public bool IsRectangle { get; private set; }

        public override void Draw(Graphics g)
        {
            g.FillPolygon(_BodyBrush, Points.ToArray());
            g.DrawPolygon(_LinePen, Points.ToArray());

            foreach (Rectangle r in Handles())
            {
                g.FillRectangle(_HandleBrush, r);
                g.DrawRectangle(_Handlepen, r);
            }
        }

        /// <summary>
        /// Test which handle has been hit
        /// </summary>
        /// <param name="pX"></param>
        /// <param name="pY"></param>
        /// <returns></returns>
        private bool TestHandleHit(int pX, int pY)
        {
            for(int ctr =0; ctr < Handles().Count(); ctr++)
            {
                if (Handles()[ctr].Contains(pX,pY))
                {
                    HandleHit = ctr;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Test which line has been hit
        /// </summary>
        /// <param name="pX"></param>
        /// <param name="pY"></param>
        /// <returns></returns>
        private bool TestLineHit(int pX, int pY)
        {
            var lines = CalculateLines();
            GraphicsPath path;

            for (int ctr = 0; ctr < lines.Count; ctr++)
            {
                var line = lines[ctr];
                path = new GraphicsPath();
                path.AddLine(line.X1, line.Y1, line.X2, line.Y2); 

                if(path.IsOutlineVisible(new Point(pX,pY), _LinePen))
                {
                    LineHit = ctr;
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// See what the coordinates hit, if anything
        /// </summary>
        /// <param name="pX"></param>
        /// <param name="pY"></param>
        /// <returns></returns>
        public override bool HitTest(int pX, int pY)
        {
            bool hit = true;
            var path = ShapePath();
            if (TestHandleHit(pX,pY))
            {
                ItemHit = ShapeItemHit.Handle;
            }
            else if (path.IsVisible(pX,pY))
            {
                ItemHit = ShapeItemHit.Body;
            }
            else if (TestLineHit(pX, pY))
            {
                ItemHit = ShapeItemHit.Line;
            }
            else
            {
                ItemHit = ShapeItemHit.None;
                hit = false;
            }

            return hit;
        }

        /// <summary>
        /// Move the selected handle, the behaviour depends on whether IsComplex is truee
        /// </summary>
        /// <param name="pX"></param>
        /// <param name="pY"></param>
        public void MoveHandle(int pX, int pY)
        {
            if (IsRectangle)  //  it's a polygon, so move the individual point
            {
                var p = Points[HandleHit];
                Points[HandleHit] = new PointF(p.X + pX, p.Y + pY);
            }
            else
            {
                PointF p1, p2;
                switch(HandleHit)
                {
                    case 0:     //top handle,  Points 0 and 1
                        p1 = Points[0];
                        Points[0] = new PointF(p1.X, p1.Y + pY);

                        p2 = Points[1];
                        Points[1] = new PointF(p2.X, p2.Y +pY);
                        break;

                    case 1:     //right handle, Points 1 and 2
                        p1 = Points[1];
                        Points[1] = new PointF(p1.X + pX, p1.Y);

                        p2 = Points[2];
                        Points[2] = new PointF(p2.X + pX, p2.Y);
                        break;

                    case 2:     //bottom handle, Points 2 and 3
                        p1 = Points[2];
                        Points[2] = new PointF(p1.X, p1.Y + pY);

                        p2 = Points[3];
                        Points[3] = new PointF(p2.X, p2.Y + pY);
                        break;

                    case 3:     //left handle, Points 3 and 0
                        p1 = Points[3];
                        Points[3] = new PointF(p1.X + pX, p1.Y);

                        p2 = Points[0];
                        Points[0] = new PointF(p2.X + pX, p2.Y);
                        break;
                }
            }
        }

        /// <summary>
        /// Draw the whole shape
        /// </summary>
        /// <param name="pX"></param>
        /// <param name="pY"></param>
        /// <returns></returns>
        public override bool Drag(int pX, int pY)
        {
            for (int ctr = 0; ctr < Points.Count; ctr++)
            {
                var p = Points[ctr];

                Points[ctr] = new PointF(p.X + pX, p.Y + pY);
            }

            return true;
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
    }
}
