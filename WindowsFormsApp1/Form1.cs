using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.DoubleBuffered = true;


            linkLabel1_LinkClicked(null,null);
        }


        List<ShapePolygonBase> _Shapes = new List<ShapePolygonBase>();

        
        Point MouseDownLocation;
        bool IsMouseDown;

        ShapePolygonBase _SelectedShape;
        public ShapePolygonBase SelectedShape { get { return _SelectedShape; } set { 
                
                foreach(var p in _Shapes)
                {
                    p.Selected = p == value;
                }

                _SelectedShape = value; 
            } }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            foreach (var shape in _Shapes)
            {

                shape.Draw(e.Graphics);
            }

        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsMouseDown)
            {
                foreach (var shape in _Shapes)
                {
                    if (shape.HitTest(e.X, e.Y))
                    {
                        if (shape == SelectedShape)
                        {
                            switch (shape.ItemHit)
                            {
                                case SelectionPartHit.body:
                                    Cursor = Cursors.SizeAll;
                                    break;

                                case SelectionPartHit.line:
                                    Cursor = Cursors.Hand;
                                    break;


                                case SelectionPartHit.dragHandle:
                                    Cursor = Cursors.Cross;
                                    break;

                                case SelectionPartHit.deleteButton:
                                    Cursor = Cursors.Hand;
                                    break;
                            }                           
                        }

                         return;
                    }
                    else
                    {
                        Cursor = Cursors.Default;
                    }
                }
            }
            else if (SelectedShape != null)
            {
                var bounds = SelectedShape.GetBoundingRectangle();
                Point delta = new Point(e.X - MouseDownLocation.X, e.Y - MouseDownLocation.Y);

                if (SelectedShape.ItemHit == SelectionPartHit.body)
                {
                    SelectedShape.Drag(delta.X, delta.Y);                    
                }
                else if (SelectedShape.ItemHit == SelectionPartHit.dragHandle)
                {       
                     SelectedShape.MoveHandle(delta.X, delta.Y);                    
                }

                MouseDownLocation = new Point(e.X, e.Y);

                this.Invalidate();
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            IsMouseDown = false;
            if (SelectedShape != null)
            {
                propertyGrid1.Refresh();
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            IsMouseDown = true;
            MouseDownLocation = new Point(e.X, e.Y);

            SelectedShape = null;
            propertyGrid1.SelectedObject = null;
            foreach (var shape in _Shapes)
            {

                if (shape.HitTest(e.X, e.Y))
                {
                    propertyGrid1.SelectedObject = SelectedShape = shape;
                    return;
                }
            }

            SelectedShape = null;
            this.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// Doubleclick event, do things to the selected poly lines
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (SelectedShape != null && SelectedShape is ShapePolygon)                  
            {
                if (SelectedShape.ItemHit == SelectionPartHit.line)
                {
                    (SelectedShape as ShapePolygon).AddHandle(e.X, e.Y);
                    this.Invalidate();
                }else if (SelectedShape.ItemHit == SelectionPartHit.dragHandle)
                {
                    (SelectedShape as ShapePolygon).DeleteHandle();
                }
                this.Invalidate();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _Shapes.Add(new ShapeRectangle( 300, 300));
            this.Invalidate();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _Shapes.Add(new ShapePolygon(300, 300));
            this.Invalidate();
        }
    }
}
