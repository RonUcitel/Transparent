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
        public Form1()
        {
            InitializeComponent();
            Focus();
            if (BackgroundImage == null)
            {
                BackColor = Color.Red;
            }
            //MakeTransparent2(pictureBox1, this);
            MakeTransparent2(button1, this);
        }

        public void MakeTransparent2(Control c, Form f)
        {
            Bitmap output = new Bitmap(f.ClientSize.Width, f.ClientSize.Height);
            using (Graphics g = Graphics.FromImage(output))
            {
                g.Clear(f.BackColor);
                using (Bitmap full_back = new Bitmap(f.Width, f.Height))
                {
                    f.DrawToBitmap(full_back, new Rectangle(0, 0, f.Width, f.Height));
                    Point toScreen = f.PointToScreen(new Point(0, 0));
                    Bitmap back = new Bitmap(f.ClientSize.Width, f.ClientSize.Height);
                    using (Graphics g_back = Graphics.FromImage(back))
                    {
                        g_back.DrawImage(full_back, 0, 0, new Rectangle(toScreen.X - f.Left, toScreen.Y - f.Top, f.ClientSize.Width, f.ClientSize.Height), GraphicsUnit.Pixel);
                    }
                    g.DrawImage(back, new Point(0, 0));
                }

            }

            SortedDictionary<int, Control> sd = new SortedDictionary<int, Control>();
            int min = f.Controls.GetChildIndex(c);
            foreach (Control item in f.Controls)
            {
                if (f.Controls.GetChildIndex(item) > min)
                {
                    sd.Add(f.Controls.GetChildIndex(item), item);
                }
            }
            for (int i = sd.Count; i > min; i--)
            {
                sd[i].DrawToBitmap(output, new Rectangle(sd[i].Left, sd[i].Top, sd[i].Width, sd[i].Height));
            }


            Rectangle r = c.ClientRectangle;
            r.Location = new Point(c.Left + 1, c.Top + 1);

            c.BackgroundImage = output.Clone(r, output.PixelFormat);
        }

        public Bitmap MakeTransparent(Control c)
        {

            //Make a new Bimap painted with the color of the form.
            Bitmap output = new Bitmap(Width, Height);
            Graphics g = Graphics.FromImage(output);
            g.Clear(BackColor);


            if (BackgroundImage != null)//If the form have a BackGoundImage:
            {
                AddImageAtPlace(output, this);//Add it to the output Bitmap.
            }

            //Make a list of the Controls on the form and order it by the "z" value of the control(what is on what at the desplay).
            //List<Control> l = Controls.OfType<Control>().ToList();
            //l.Sort((x, y) => Controls.GetChildIndex(x).CompareTo(y));//Use a Lambda Expression to make a Comparison method.

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
                r = new Rectangle(c.Left - 1, c.Top - 1, c.Width - 2, c.Height - 2);
                //r.Location = /*c.Location*/ new Point();
            }

            return output.Clone(r, output.PixelFormat);
        }

        public void AddImageAtPlace(Bitmap back, Control c)
        {
            using (Graphics g = Graphics.FromImage(back))
            {
                Bitmap p = GetResizedImage(c);
                if (c == this)
                {
                    g.DrawImage(p, new Point(0, (back.Height - p.Height) / 2));
                }
                else
                {
                    g.DrawImage(p, new Point(c.Left, (back.Height - p.Height) / 2));
                }
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
