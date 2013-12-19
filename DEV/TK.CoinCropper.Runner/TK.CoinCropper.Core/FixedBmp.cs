using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using AForge.Imaging.Filters;

namespace TK.CoinCropper.Core
{
    public class FixedBmp
    {
        private readonly Color colorBlack = Color.FromArgb(255, 0, 0, 0);

        public Bitmap FullImageOfCoin;

        public Bitmap dateImage;

        private int datePointX;
        private int datePointY;
        private int datePointXLength;
        private int datePointYLength;
        
        public FixedBmp(string path)
        {
            FullImageOfCoin = ScaleImage(new Bitmap(path), 1000, 1000);

            FullImageOfCoin.Save(@"C:\users\school\desktop\Resized.bmp");

            //FullImageOfCoin = new Bitmap(path);
            
            CropOutEdges();

            dateImage = GetDateImage(.689/*make larger to go wider*/, .637/*make smaller to go higher*/, .65, .1956);

        }

        public Bitmap GetDateImage(double pointXPercent, double pointYPercent, double pointXLengthPercent, double pointYLengthPercent)
        {
            datePointX = Convert.ToInt32(FullImageOfCoin.Width * pointXPercent);//make larger to go wider
            datePointY = Convert.ToInt32(FullImageOfCoin.Height * pointYPercent);//make smaller to go higher

            datePointXLength = Convert.ToInt32((FullImageOfCoin.Width - datePointX) * pointXLengthPercent);
            datePointYLength = Convert.ToInt32((FullImageOfCoin.Height - datePointY) * pointYLengthPercent);

            return CropImage(FullImageOfCoin, 
                new Rectangle(datePointX, datePointY, datePointXLength, datePointYLength));
        }

        //Starts the crop process.
        private void CropOutEdges()
        {
            CropTopEdge();
            CropLeftEdge();
            CropBottomEdge();
            CropRightEdge();
        }


        //Looks for the y axis crop point
        private void CropTopEdge()
        {
            var loopBreaker = false;
            //...foreach y pixel...
            for (int y = 0; y < FullImageOfCoin.Width; y++)
            {
                //..foreach x pixel..
                for (int x = 0; x < FullImageOfCoin.Height; x++)
                {
                    //..if it's the right color...
                    if (FullImageOfCoin.GetPixel(x, y) == colorBlack)
                    {
                        //..Crop and save it..
                        loopBreaker = true;
                        FullImageOfCoin = FullImageOfCoin.Clone(new Rectangle(0, y, FullImageOfCoin.Width, FullImageOfCoin.Height - y),
                                                                FullImageOfCoin.PixelFormat);
                        //FullImageOfCoin = CropImage(FullImageOfCoin, new Rectangle(0, y, FullImageOfCoin.Width, FullImageOfCoin.Height - y));
                        break;
                    }
                }
                if (loopBreaker) { break; }
            }

        }

        //Looks for the x axis crop point
        private void CropLeftEdge()
        {
            var loopBreaker = false;
            //...foreach x pixel...
            for (int x = 0; x < FullImageOfCoin.Width; x++)
            {
                //..foreach y pixel...
                for (int y = 0; y < FullImageOfCoin.Height - 1; y++)
                {
                    //..If the colors match..
                    if (FullImageOfCoin.GetPixel(x, y) == colorBlack)
                    {
                        //..Crop and save.
                        loopBreaker = true;
                        FullImageOfCoin = FullImageOfCoin.Clone(new Rectangle(x, 0, FullImageOfCoin.Width - x, FullImageOfCoin.Height),
                                                                FullImageOfCoin.PixelFormat);
                        //FullImageOfCoin = CropImage(FullImageOfCoin, new Rectangle(x, 0, FullImageOfCoin.Width - x, FullImageOfCoin.Height));
                        break;
                    }
                }
                if (loopBreaker) { break; }
            }

        }


        private void CropBottomEdge()
        {
            var loopBreaker = false;

            for (int y = FullImageOfCoin.Height - 1; y > 0; y--)
            {
                for (int x = FullImageOfCoin.Width - 1; x > 0; x--)
                {
                    if (FullImageOfCoin.GetPixel(x, y) == colorBlack)
                    {
                        //..Crop and save.
                        loopBreaker = true;
                        FullImageOfCoin = FullImageOfCoin.Clone(new Rectangle(0, 0, FullImageOfCoin.Width, y),
                                                                FullImageOfCoin.PixelFormat);
                        //FullImageOfCoin = CropImage(FullImageOfCoin, new Rectangle(0, 0, FullImageOfCoin.Width, y));
                        break;
                    }
                }
                if (loopBreaker) { break; }
            }
        }


        private void CropRightEdge()
        {
            var loopBreaker = false;

            for (int x = FullImageOfCoin.Width - 1; x > 0; x--)
            {
                for (int y = FullImageOfCoin.Height - 1; y > 0; y--)
                {
                    if (FullImageOfCoin.GetPixel(x, y) == colorBlack)
                    {
                        //..Crop and save.
                        loopBreaker = true;
                        FullImageOfCoin = FullImageOfCoin.Clone(new Rectangle(0, 0, x, FullImageOfCoin.Height),
                                                                FullImageOfCoin.PixelFormat);
                        //FullImageOfCoin = CropImage(FullImageOfCoin, new Rectangle(0, 0, x, FullImageOfCoin.Height));
                        break;
                    }
                }
                if (loopBreaker) { break; }
            }
        }

        

        //Kindof obvious, crops image.
        public Bitmap CropImage(Bitmap img, Rectangle cropArea)
        {
            var bmpImage = new Bitmap(img);
            return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
        }

        public static Bitmap ScaleImage(Bitmap image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
            return newImage;
        }
    }
}
