using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Image;
namespace ImageRecreator
{
    public class CreateTrainingSet
    {
        // https://docs.microsoft.com/en-us/dotnet/api/microsoft.ml.imageestimatorscatalog.extractpixels?view=ml-dotnet
        static void Pixels(Image image)
        {
            /*
            for (int i = 0; i < img.GetWidth; i++)
            {
                for (int j = 0; j < img.GetHeight; j++)
                {
                    Color pixel = img.GetPixel(i, j);

                    if (pixel == *somecondition *)
                    {
                        pixel.
                    }
                }
            }
            */
        }
        public static void CreateCSV (List<Image> imageRows) {
            // imageRows[0].
        }
        public float Property { get; set; } = 2;

    }
}
// https://docs.microsoft.com/en-us/dotnet/machine-learning/resources/transforms