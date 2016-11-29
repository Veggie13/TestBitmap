using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace TestBitmap
{
    public partial class Form1 : Form
    {
        Image x = new Image(20, 20);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            byte[] pixels = new byte[x.Width * x.Height];
            x.Edit((ctx) =>
            {
                ctx[6, 6] = 1;
                ctx[7, 6] = 1;
                ctx[6, 7] = 1;
                ctx[7, 7] = 1;

                ctx[12, 6] = 2;
                ctx[13, 6] = 2;
                ctx[12, 7] = 2;
                ctx[13, 7] = 2;

                ctx[6, 12] = 3;
                ctx[7, 12] = 3;
                ctx[6, 13] = 3;
                ctx[7, 13] = 3;

                ctx[12, 12] = 4;
                ctx[13, 12] = 4;
                ctx[12, 13] = 4;
                ctx[13, 13] = 4;
            });

            var palette = x.Palette;
            palette.Entries[0] = Color.Red;
            palette.Entries[1] = Color.Orange;
            palette.Entries[2] = Color.Yellow;
            palette.Entries[3] = Color.Green;
            palette.Entries[4] = Color.Blue;
            x.Palette = palette;

            pictureBox1.Image = x.TheImage;
        }
    }
}
