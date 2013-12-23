using System;
using System.Drawing;
using System.Drawing.Imaging;
using AForge.Imaging;
using AForge.Imaging.Filters;
using Puma.Net;

// compile with: /unsafe

namespace PumaOcr.Tests
{
    internal class Program
    {
        public static Bitmap CropImage(Bitmap source, Rectangle section)
        {
            // An empty bitmap which will hold the cropped image
            Bitmap bmp = new Bitmap(section.Width, section.Height);

            Graphics g = Graphics.FromImage(bmp);

            // Draw the given area (section) of the source image
            // at location 0,0 on the empty bitmap (bmp)
            g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);

            return bmp;
        }

        private static void Main(string[] args)
        {


            Program program = new Program();


            Bitmap bmpFinal2 = new Bitmap(@"C:\users\school\desktop\source4.bmp");


            //Change pixel format if not acceptable

            //Bitmap clone = new Bitmap(bmpFinal2.Width, bmpFinal2.Height, PixelFormat.Format16bppGrayScale);

            //using (Graphics gr = Graphics.FromImage(clone))
            //{
            //    gr.DrawImage(bmpFinal2, new Rectangle(0, 0, clone.Width, clone.Height));
            //}

            //bmpFinal2 = MakeGrayscale3(bmpFinal2);
            Threshold threshold = new Threshold();

            threshold.ThresholdValue = program.getOtsuThreshold(bmpFinal2);

            threshold.ApplyInPlace(bmpFinal2);

            Invert invert = new Invert();

            invert.ApplyInPlace(bmpFinal2);

            bmpFinal2.Save(@"C:\users\school\desktop\fixedsource3.bmp");

            //var pumaPage = new PumaPage(@"c:\users\school\desktop\date.bmp");

            //using (pumaPage)
            //{
            //    pumaPage.FileFormat = PumaFileFormat.TxtAscii;

            //    pumaPage.EnableSpeller = false;

            //    pumaPage.Language = PumaLanguage.Digits;

            //    pumaPage.RecognizeToFile(@"c:\users\school\desktop\one.rtf");
            //}




            //var bmpFinal2 = CropImage(bmp, new Rectangle(2666, 2151, 30, 30));

            //var color1 = getDominantColor(bmpFinal1);

            //var color2 = getDominantColor(bmpFinal2);

            //Console.WriteLine(AreColorsSimilar(color1, color2, 2));

            //Console.Read();





            //var bmp = new Bitmap(@"C:\users\school\desktop\firstcut.bmp");
            //            CropImage(bmp, new Rectangle(1438, 1444, 34, 56))
            //                .Save(@"C:\users\school\desktop\saved1.bmp");

            //var bmp2 = new Bitmap(@"C:\users\school\desktop\firstcut.bmp");
            //            CropImage(bmp, new Rectangle(2343, 1455, 34, 56))
            //                .Save(@"C:\users\school\desktop\saved2.bmp");




            

            //var pumaPage1 = new PumaPage(@"c:\users\school\desktop\2010_BW_INVERT.bmp");

            //using (pumaPage1)
            //{
            //    pumaPage1.FileFormat = PumaFileFormat.TxtAscii;

            //    pumaPage1.EnableSpeller = false;

            //    pumaPage1.Language = PumaLanguage.English;

            //    pumaPage1.RecognizeToFile(@"c:\users\school\desktop\2010_BW_INVERT.txt");
            //}

            //var pumaPage2 = new PumaPage(@"c:\users\school\desktop\2010_INVERT.bmp");

            //using (pumaPage2)
            //{
            //    pumaPage2.FileFormat = PumaFileFormat.TxtAscii;

            //    pumaPage2.EnableSpeller = false;

            //    pumaPage2.Language = PumaLanguage.English;

            //    pumaPage2.RecognizeToFile(@"c:\users\school\desktop\2010_INVERT.txt");
            //}
        }
        
        public int getOtsuThreshold(Bitmap bmp)
        {
            byte t = 0;
            float[] vet = new float[256];
            int[] hist = new int[256];
            vet.Initialize();

            float p1, p2, p12;
            int k;

            BitmapData bmData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                                             ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            unsafe
            {
                byte* p = (byte*) (void*) bmData.Scan0.ToPointer();

                getHistogram(p, bmp.Width, bmp.Height, bmData.Stride, hist);


                for (k = 1; k != 255; k++)
                {
                    p1 = Px(0, k, hist);
                    p2 = Px(k + 1, 255, hist);
                    p12 = p1*p2;
                    if (p12 == 0)
                        p12 = 1;
                    float diff = (Mx(0, k, hist)*p2) - (Mx(k + 1, 255, hist)*p1);
                    vet[k] = (float) diff*diff/p12;

                }
            }
            bmp.UnlockBits(bmData);

            t = (byte) findMax(vet, 256);

            bmp.Dispose();

            return t;
        }

        private unsafe void getHistogram(byte* p, int w, int h, int ws, int[] hist)
        {
            hist.Initialize();
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w * 3; j += 3)
                {
                    int index = i * ws + j;
                    hist[p[index]]++;
                }
            }
        }

        private int findMax(float[] vec, int n)
        {
            float maxVec = 0;
            int idx = 0;
            int i;

            for (i = 1; i <= n - 1; i++)
            {
                if (vec[i] > maxVec)
                {
                    maxVec = vec[i];
                    idx = i;
                }
            }
            return idx;
        }

        private float Px(int init, int end, int[] hist)
        {
            int sum = 0;
            int i;
            for (i = init; i <= end; i++)
                sum += hist[i];

            return (float)sum;
        }

        // function is used to compute the mean values in the equation (mu)
        private float Mx(int init, int end, int[] hist)
        {
            int sum = 0;
            int i;
            for (i = init; i <= end; i++)
                sum += i * hist[i];

            return (float)sum;
        }

        public static Bitmap MakeGrayscale3(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(newColorMatrix: 
                new[]{
                    new float[] {.3f, .3f, .3f, 0, 0},
                    new float[] {.59f, .59f, .59f, 0, 0},
                    new float[] {.11f, .11f, .11f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1}
                });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }
    }
}

//    public static Color getDominantColor(Bitmap bmp)
    //    {
    //        //Used for tally
    //        int r = 0;
    //        int g = 0;
    //        int b = 0;

    //        int total = 0;

    //        for (int x = 0; x < bmp.Width; x++)
    //        {
    //            for (int y = 0; y < bmp.Height; y++)
    //            {
    //                Color clr = bmp.GetPixel(x, y);
    //                r += clr.R;
    //                g += clr.G;
    //                b += clr.B;
    //                total++;
    //            }
    //        }

    //        //Calculate average
    //        r /= total;
    //        g /= total;
    //        b /= total;

    //        return Color.FromArgb(r, g, b);
    //    }
    //    public static bool AreColorsSimilar(Color c1, Color c2, int tolerance)
    //    {
    //        return Math.Abs(c1.R - c2.R) < tolerance &&
    //               Math.Abs(c1.G - c2.G) < tolerance &&
    //               Math.Abs(c1.B - c2.B) < tolerance;
    //    }
    //} 
//}
