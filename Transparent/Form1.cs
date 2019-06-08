using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Transparent
{
    public partial class Form1 : Form
    {
        Timer t = new Timer();
        public Form1()
        {
            InitializeComponent();
            if (BackgroundImage == null)
            {
                BackColor = Color.Red;
            }
            t.Interval = 1000;
            t.Tick += T_Tick;
            t.Start();
            
            //pictureBox1.Image = MakeTransparent(pictureBox1);
        }

        private void T_Tick(object sender, EventArgs e)
        {
            this.BackgroundImage = MakeTransparent(this);
            //pictureBox1.BackgroundImage = MakeTransparent(pictureBox1);
        }

        public Bitmap MakeTransparent(Control c)
        {

            //Make a new Bimap painted with the color of the form.
            Bitmap output = new Bitmap(Width, Height);
            Graphics g = Graphics.FromImage(output);
            g.Clear(BackColor);


            if (BackgroundImage != null)//If the form have a BackGoundImage:
            {
                AddImageAtPlace(output,g, this);//Add it to the output Bitmap.
            }

            //Make a list of the Controls on the form and order it by the "z" value of the control(what is on what at the desplay).
            List<Control> l = Controls.OfType<Control>().ToList();
            l.Sort((x, y) => Controls.GetChildIndex(x).CompareTo(y));//Use a Lambda Expression to make a Comparison method.

            //for (int i = 0; i < l.Count; i++)
            //{
            //    if (l[i].BackgroundImage != null && l[i].Name != c.Name)
            //    {
            //        AddImageAtPlace(output, l[i]);
            //    }

            //}
            Rectangle r = c.ClientRectangle;
            if (c != this)
            {
                r.Location = c.Location;
            }
            
            return output.Clone(r, output.PixelFormat);
        }

        public void AddImageAtPlace(Bitmap back, Graphics g, Control c)
        {
            if (c == this)
            {
                g.DrawImage(GetResizedImage(c), new Point(0, (back.Height - GetResizedImage(c).Height) / 2));
            }
            else
            {
                g.DrawImage(GetResizedImage(c), new Point(c.Left, (back.Height - GetResizedImage(c).Height) / 2));
            }
        }
        public Bitmap GetResizedImage(Control c)
        {
            Bitmap output = new Bitmap(c.BackgroundImage);
            double resizeFactor = Math.Max((double)output.Width / c.ClientSize.Width, (double)output.Height / c.ClientSize.Height);
            Size imageSize = new Size((int)(output.Width / resizeFactor), (int)(output.Height / resizeFactor));
            return new Bitmap(c.BackgroundImage, imageSize);
        }
    }
}
