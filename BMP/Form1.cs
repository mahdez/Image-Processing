using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BMP
{
    public partial class Form1 : Form
    {
        private Bitmap f_image = null;
        public Bitmap image;
        int m = 0;
        int sin90 = 1, sin270 = -1;
        int cos90 = 0, cos270 = 0;
        double gamma = 0;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnOpen_Click(object sender, EventArgs e)
        {

            openFileDialog1.Filter = "bitmap (*.bmp)|*.bmp";
            openFileDialog1.FilterIndex = 1;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (f_image != null) f_image.Dispose();
                    f_image = (Bitmap)Bitmap.FromFile(openFileDialog1.FileName, false);

                }
                catch (Exception)
                {
                    MessageBox.Show("Can not open file", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            image = new Bitmap(f_image.Width, f_image.Height);
            for (int i = 0; i < f_image.Height; i++)
            {
                for (int j = 0; j < f_image.Width; j++)
                {
                    Color PixelColor = f_image.GetPixel(j, i);
                    int C_gray = (int)(PixelColor.R + PixelColor.G + PixelColor.B) / 3;
                    //  num_pic.Add(C_gray);
                    m = m + C_gray;

                    image.SetPixel(j, i, Color.FromArgb(C_gray, C_gray, C_gray));

                }
            }
            m = m / (f_image.Width * f_image.Height);
            pictureBox1.Image = f_image;
            pictureBox2.Image = image;
        }

        private void btnThreshold_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < f_image.Height; i++)
            {
                for (int j = 0; j < f_image.Width; j++)
                {
                    Color PixelColor = f_image.GetPixel(j, i);
                    int C_gray = (int)(PixelColor.R + PixelColor.G + PixelColor.B) / 3;
                    
                    if (C_gray > m)
                    {
                        C_gray = 255;
                    }
                    else
                    {
                        C_gray = 0;
                    }
                    image.SetPixel(j, i, Color.FromArgb(C_gray, C_gray, C_gray));

                }
            }
            pictureBox2.Image = image;
        }

        private void button3_Click(object sender, EventArgs e)
        {
         
            for (int i = 0; i < f_image.Height; i++)
            {
                for (int j = 0; j < f_image.Width; j++)
                {
                    Color PixelColor = f_image.GetPixel(j, i);
                    int Cont = (int)(PixelColor.R + PixelColor.G + PixelColor.B) / 3;
                    
                    int s = 0;
                    if (Cont > 150)
                    {
                        s = (int)((0.523 * Cont) + 121.635);
                    }
                    else if (Cont >=100)
                    {
                        s = (int)((3 * Cont) - 250);
                    }
                    else
                    {
                        s = (int)(0.5 * Cont);
                    }
                    image.SetPixel(j, i, Color.FromArgb(s, s, s));
                }
            }
            
            pictureBox2.Image = image;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int c = 1;
            gamma = (double)trackBar1.Value / 10.0;
            label2.Text = gamma.ToString();
            

            for (int i = 0; i < f_image.Height; i++)
            {
                for (int j = 0; j < f_image.Width; j++)
                {
                    Color PixelColor = f_image.GetPixel(i, j);
                    int img = (int)(PixelColor.R + PixelColor.G + PixelColor.B) / 3;
                    // img = (img / 10) ;

                    // img.ToString()
                    int s = c * (int)Math.Pow(img,gamma);
                    s = 255 - s;
                    if (s > 255)
                    {
                        s = s / 10;
                        s = 255 - s;
                    }
                    if (s<0)
                    {
                        s = (s / 10) * -1;
                        
                    }
                    image.SetPixel(i, j, Color.FromArgb(s, s, s));
                    //textBox1.Text = s.ToString();
                }
            }
            pictureBox2.Image = image;
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            //array No. of pixel
            int[] Nop = new int[256];
            for (int i = 0; i < 256; i++)
            {
                Nop[i] = 0;
            }

            //array No. of frequency
            int[] Nof = new int[256];

            //array No. of frequency / Total number pixel
            float[] Nopf = new float[256];

            //array No.of possible
            int[] NoP = new int[256];

            //count No. of pixel
            for (int i = 0; i < f_image.Height; i++)
            {
                for (int j = 0; j < f_image.Width; j++)
                {
                    Color PixelColor = f_image.GetPixel(j, i);
                    int C_gray = (int)(PixelColor.R + PixelColor.G + PixelColor.B) / 3;
                    Nop[C_gray]++;

                }

            }

            //count No. of frequency
            Nop[0] = Nof[0];
            for (int i = 1; i < 256; i++)
            {
                Nof[i] = Nof[i - 1] + Nop[i];
            }

            //count No. of frequency / Total number pixel
            int NM = f_image.Height * f_image.Width;
            for (int i = 0; i < 256; i++)
            {
                Nopf[i] = (float)Nof[i] / NM;
            }

            //count No. of possible

            for (int i = 0; i < 256; i++)
            {
                NoP[i] = (int)(Nopf[i] * 255);
            }

            //output
            int ss;
            for (int i = 0; i < f_image.Height; i++)
            {
                for (int j = 0; j < f_image.Width; j++)
                {
                    Color PixelColor = f_image.GetPixel(j, i);
                    int C_gray = (int)(PixelColor.R + PixelColor.G + PixelColor.B) / 3;
                    for (int k = 0; k < 256; k++)
                    {
                        if (C_gray == k)
                        {
                            ss = NoP[k];
                            image.SetPixel(j, i, Color.FromArgb(ss, ss, ss));
                            break;
                        }

                    }
                }
            }
            pictureBox2.Image = image;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < f_image.Height; i++)
            {
                for (int j = 0; j < f_image.Width; j++)
                {
                    Color PixelColor = f_image.GetPixel(i, j);
                    int img = (int)(PixelColor.R + PixelColor.G + PixelColor.B) / 3;
                    int imgX = 128+((i-128) * cos90) + ((j-128) * sin90);
                    int imgY = 128+((i-128) * sin90) + ((j-128) * cos90);
                    image.SetPixel(imgX, imgY, Color.FromArgb(img, img, img));

                }
            }
            pictureBox2.Image = image;
        }

        private void button2_Click(object sender, EventArgs e)
        {
          
            for (int i = 0; i < f_image.Height; i++)
            {
                for (int j = 0; j < f_image.Width; j++)
                {
                    Color PixelColor = f_image.GetPixel(i, j);
                    int img = (int)(PixelColor.R + PixelColor.G + PixelColor.B) / 3;
                    int imgX = 127 + ((i - 128) * 0) + ((j - 128) * -1);
                    int imgY = 127 + ((i - 128) * -1) + ((j - 128) * 0);
                    
                    image.SetPixel(imgX, imgY, Color.FromArgb(img, img, img));

                }
            }
            pictureBox2.Image = image;
        }
    } 
}
