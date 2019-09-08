using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Diagnostics;
namespace ImageRecreator
{
    public class Train
    {
        // creats model
        public static void Run(Bitmap[] images)
        {
            var mlContext = new MLContext();
            //var imageData = ImageData(images[0]);
        }
        static List<Data> ImageData(Bitmap original, Bitmap[] lowImages)
        {
            var imageData = new List<Data>();
            Debug.WriteLine("original: " + original.Name());
            for(int i = 0; i < lowImages.Length; i ++)
            {
                Debug.WriteLine("low quality " + i);
                for (int x = 0; x < lowImages[i].Width; x++)
                {
                    for (int y = 0; y < lowImages[i].Height; y++)
                    {
                        var data = new Data()
                        {
                            total = lowImages[i],
                            index = new int[] { x, y },
                            value = (float)lowImages[i].GetPixel(x, y).ToArgb(),
                            original = (float)original.GetPixel(x, y).ToArgb()
                        };
                        imageData.Add(data);
                    }
                }
            }

            Debug.WriteLine("created imageData: " + imageData.Count + " = " + lowImages.Length + " * " + " width: " + original.Width + " + " + " height: " + original.Height);
            return imageData;
        }
        //static Pixel Pixel()
        static void Data()
        {

        }
        // make train already existing model
    }
}
