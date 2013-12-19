using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TK.CoinCropper.Core;

namespace TK.CoinCropper.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            FixedBmp fixedBmp = new FixedBmp(@"C:\users\school\desktop\Source2.bmp");

            fixedBmp.FullImageOfCoin.Save(@"C:\users\school\desktop\fixed.bmp");

            fixedBmp.dateImage.Save(@"C:\users\school\desktop\date.bmp");
            
        }
    }
}
