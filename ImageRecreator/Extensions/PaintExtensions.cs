using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ImageRecreator
{
    public static class PaintExtensions
    {

        public static Bitmap Paint(this int[] oneDimensionalValueArray, int width, int height)
        {
            // original/imageToRecreate bounds is the same bounds as any low copy

            int[,] paintArray = new int [width, height];
            Buffer.BlockCopy(oneDimensionalValueArray, 0, paintArray, 0, paintArray.Length * sizeof(int));

            Bitmap bitmap = new Bitmap(width, height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++) {
                    bitmap.SetPixel(x, y, Color.FromArgb(paintArray[x, y]));
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
    }
}
