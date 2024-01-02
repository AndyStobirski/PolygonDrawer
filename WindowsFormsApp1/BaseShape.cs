using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public abstract class BaseShape
    {
        public string ID { get; private set; }

        public BaseShape()
        {
            ID = Guid.NewGuid().ToString();
        }

        public Point Origin { get; set; }

        public abstract void Draw(Graphics g);

        public abstract bool HitTest(int pX, int pY);

        public abstract bool Drag(int pX, int pY);

    }
}
