using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TK.CoinCropper.Core;

namespace TK.CoinCropper.App
{
    public partial class FindingTheDatePercentages : Form
    {
        public FindingTheDatePercentages()
        {
            InitializeComponent();
        }

        private FixedBmp fixedBmp;

        private void FindingTheDatePercentages_Load(object sender, EventArgs e)
        {
            fixedBmp = new FixedBmp(@"C:\users\school\desktop\source.bmp");

            label5.Text = ".689";
            label6.Text = ".637";
            label7.Text = ".65";
            label8.Text = ".1956";

            pictureBox1.Image = fixedBmp.dateImage;

            fixedBmp.FullImageOfCoin.Save(@"C:\users\school\desktop\fixed.bmp");

            fixedBmp.dateImage.Save(@"C:\users\school\desktop\date.bmp");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text != string.Empty && textBox2.Text != string.Empty && 
                textBox3.Text != string.Empty && textBox4.Text != string.Empty)
            {
                pictureBox1.Image = fixedBmp.GetDateImage(Convert.ToDouble(textBox1.Text), Convert.ToDouble(textBox2.Text), 
                    Convert.ToDouble(textBox3.Text), Convert.ToDouble(textBox4.Text));

                label5.Text = textBox1.Text;
                label6.Text = textBox2.Text;
                label7.Text = textBox3.Text;
                label8.Text = textBox4.Text;


                fixedBmp.dateImage.Save(@"C:\users\school\desktop\date.bmp");
            }
        }
    }
}
