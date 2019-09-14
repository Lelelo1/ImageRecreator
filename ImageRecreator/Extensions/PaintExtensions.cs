using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

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
    }
}
