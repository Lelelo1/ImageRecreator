using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Diagnostics;
namespace ImageRecreator
{
    public static class PaintExtensions
    {
        /* test */
        public static Bitmap Paint(this float[] oneDimensionalValueArray, int width, int height)
        {
            // original/imageToRecreate bounds is the same bounds as any low copy

            // -120000 is red
            // -1 seem to be white. Yes! Confirmed
            // -60000 is pink

            float[,] paintArray = new float [width, height];
            Buffer.BlockCopy(oneDimensionalValueArray, 0, paintArray, 0, paintArray.Length * sizeof(float));

            Bitmap bitmap = new Bitmap(width, height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++) {
                    
                    if(x < 100 && y < 100)
                    {
                        bitmap.SetPixel(x, y, Color.FromArgb(-120000));
                    }
                    else
                    {
                        bitmap.SetPixel(x, y, Color.FromArgb(200));
                    }
                    
                }
            }
            return bitmap;
        }
        /* */
        public static Bitmap Paint(this Bitmap bitmap, int value)
        {
            var byteArray = value.toBytes();
            foreach(var b in byteArray)
            {
                Debug.WriteLine("byte: " + b);
            }
            for(int x = 0; x < bitmap.Width; x ++)
            {
                for(int y = 0; y < bitmap.Height; y ++)
                {
                    bitmap.SetPixel(x, y, Color.FromArgb(byteArray[3], byteArray[0], byteArray[1], byteArray[2]));
                }
            }
            return bitmap;
        }
        public static void Paint(List<Data> imageData)
        {
            /*
            // var imageData2dArray = Buffer.BlockCopy(
            var imageData2dArray = new int[];
            for (int x = 0; x < 
            */
        }
        public static int GetPixelsCountOf(this Bitmap image, int argb)
        {
            int count = 0;
            for(int x = 0; x < image.Width; x ++)
            {
                for(int y = 0; y < image.Height; y ++)
                {
                    if(image.GetPixel(x, y).ToArgb() == argb)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        /* log when creating 5 random images with: bounds width: 2448, height: 3264 
         *  Random image. average color Color [A=44, R=127, G=127, B=127]
            Random image. average color Color [A=44, R=127, G=127, B=127]
            Random image. average color Color [A=43, R=130, G=130, B=130]
            Random image. average color Color [A=44, R=127, G=127, B=127]   
            Random image. average color Color [A=44, R=127, G=127, B=127]
         */
        public static Bitmap Random(this Bitmap bitmap)
        {
            for(int x = 0; x < bitmap.Width; x ++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    var r = Random();
                    var g = Random();
                    var b = Random();
                    var a = Random();
                    // Debug.WriteLine("random color: " + r + ", " + g + " , " + b + ", " + a);
                    bitmap.SetPixel(x, y, Color.FromArgb(r, g, b, a));
                }
            }
            // Debug.WriteLine("Random image. average color " + bitmap.GetAverageColor());
            return bitmap;
        }
        private static Random random = new Random();
        static int Random()
        {
            return random.Next(0, 255);
        }

        // https://stackoverflow.com/questions/1068373/how-to-calculate-the-average-rgb-color-values-of-a-bitmap
        public static Color GetAverageColor(this Bitmap image)
        {
            Bitmap bmp = new Bitmap(1, 1);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                // updated: the Interpolation mode needs to be set to 
                // HighQualityBilinear or HighQualityBicubic or this method
                // doesn't work at all.  With either setting, the results are
                // slightly different from the averaging method.
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, new Rectangle(0, 0, 1, 1));
            }
            return bmp.GetPixel(0, 0);
        }
    }

}
