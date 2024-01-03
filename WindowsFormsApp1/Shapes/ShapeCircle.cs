using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Shapes
{
    public class ShapeCircle : ShapeBase
    {
        public ShapeCircle(int pX, int pY)
        {

        }

        public override bool Drag(int pX, int pY)
        {
            throw new NotImplementedException();
        }

        public override void Draw(Graphics g)
        {
            throw new NotImplementedException();
        }

        public override Rectangle GetBoundingRectangle()
        {
            throw new NotImplementedException();
        }

        public override bool HitTest(int pX, int pY)
        {
            throw new NotImplementedException();
        }

        public override void MoveHandle(int pX, int pY)
        {
            throw new NotImplementedException();
        }

        protected override Rectangle[] Handles()
        {
            throw new NotImplementedException();
        }
    }
}
