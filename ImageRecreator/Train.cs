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
        public static void Run(List<Bitmap> originalImages)
        {
            var original = originalImages[0];

            var mlContext = new MLContext();
            var imageData = ImageData(original, original.LowQualityImages(2));
            
            for (int i = 0; i < imageData.Count; i ++)
            {
                imageData[i].Print();
            }
            
        }
        static List<Data> ImageData(Bitmap original, List<Bitmap> lowImages)
        {
            var imageData = new List<Data>();
            Debug.WriteLine("original: " + original.Name());
            for(int i = 0; i < lowImages.Count; i ++)
            {
                lowImages[i].Name(original.Name() + "q" + i); // for test purpose
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
                        // data.Print();
                    }
                }
            }

            Debug.WriteLine("created imageData: " + imageData.Count + " = " + lowImages.Count + " * " + " width: " + original.Width + " * " + " height: " + original.Height);
            return imageData;
        }
        //static Pixel Pixel()
        static void Data()
        {

        }
        // make train already existing model
    }
}
