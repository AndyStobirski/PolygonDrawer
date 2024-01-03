using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public abstract class ShapeBase
    {
        public string ID { get; private set; }

        public ShapeBase()
        {
            ID = Guid.NewGuid().ToString();
        }

        public bool Selected { get; set; }

        public Point Origin { get; set; }

        public abstract void Draw(Graphics g);

        public abstract bool HitTest(int pX, int pY);

        public abstract bool Drag(int pX, int pY);

        protected abstract Rectangle[] Handles();

        public abstract Rectangle GetBoundingRectangle();

    }
}
