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


        List<Poly> Polys = new List<Poly>();

        Poly SelectedPoly;
        Point MouseDownLocation;
        bool IsMouseDown;


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            foreach (var poly in Polys)
            {

                poly.Draw(e.Graphics);
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsMouseDown)
            {
                foreach (var poly in Polys)
                {
                    if (poly.HitTest(e.X, e.Y))
                    {
                        switch (poly.ItemHit)
                        {
                            case ShapeItemHit.Body:
                                Cursor = Cursors.SizeAll;
                                break;

                            case ShapeItemHit.Line:
                                Cursor = Cursors.Hand;
                                break;


                            case ShapeItemHit.Handle:
                                Cursor = Cursors.Cross;
                                break;
                        }

                        return;
                    }
                    else
                    {
                        Cursor = Cursors.Default;
                    }
                }
            }
            else if (SelectedPoly != null)
            {
                var bounds = SelectedPoly.BoundingRectangle;
                Point delta = new Point(e.X - MouseDownLocation.X, e.Y - MouseDownLocation.Y);

                if (SelectedPoly.ItemHit == ShapeItemHit.Body)
                {
                    SelectedPoly.Drag(delta.X, delta.Y);                    
                }
                else if (SelectedPoly.ItemHit == ShapeItemHit.Handle)
                {       
                     SelectedPoly.MoveHandle(delta.X, delta.Y);                    
                }

                MouseDownLocation = new Point(e.X, e.Y);

                this.Invalidate();
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            IsMouseDown = false;
            if (SelectedPoly != null)
            {
                propertyGrid1.Refresh();
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            IsMouseDown = true;
            MouseDownLocation = new Point(e.X, e.Y);

            SelectedPoly = null;
            propertyGrid1.SelectedObject = null;
            foreach (var poly in Polys)
            {

                if (poly.HitTest(e.X, e.Y))
                {
                    propertyGrid1.SelectedObject = SelectedPoly = poly;
                    return;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (SelectedPoly != null && SelectedPoly.ItemHit == ShapeItemHit.Line)
            {

                SelectedPoly.AddHandle(e.X, e.Y);
                this.Invalidate();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Polys.Add(new Poly(false, 300, 300));
            this.Invalidate();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Polys.Add(new Poly(true, 300, 300));
            this.Invalidate();
        }
    }
}
