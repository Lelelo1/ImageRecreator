using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Drawing.Imaging;
using System.IO;
using System.Diagnostics;
using Microsoft.ML.AutoML;
using Microsoft.ML.Data;

namespace ImageRecreator
{
    public static class Extensions
    {

        public static void Print(this ExperimentResult<RegressionMetrics> result)
        {
            Console.WriteLine("Regession metric...");
            var details = result.RunDetails.ToList();
            Console.WriteLine("details");
            foreach(var detail in details)
            {
                Console.WriteLine(detail);
            }
            Console.WriteLine("best run...");
            Console.WriteLine(result.BestRun);
            Console.WriteLine("runtimeSeconds: " + result.BestRun.RuntimeInSeconds);
            Console.WriteLine("estimator: " + result.BestRun.Estimator);
            Console.WriteLine("trainerName " + result.BestRun.TrainerName);
            Console.WriteLine("validationMetrics " + result.BestRun.ValidationMetrics);

        }
        public static void Print(this Data d)
        {

            Console.WriteLine("average: " + d.average +
            ", index: (x: " + d.x + ", y: " + d.y + "), value: " + d.value + ", original: " + d.original);

        }
 
        public static List<Bitmap> LowQualityImages(this Bitmap image, int parts)
        {
            var list = new List<Bitmap>();
            int s = 100 / parts; // since starting from 0
            Debug.WriteLine("parts: " + parts);
            for(int q = 0; q < 100; q += s)
            {
                list.Add(LowQualityImage(image, q));
                Debug.WriteLine("quality: " + q);
            }

            Debug.WriteLine("Created {0} low quality images for " + image.Name(), list.Count);
            return list;
        }
        
        // https://stackoverflow.com/questions/4161873/reduce-image-size-c-sharp
        static Bitmap LowQualityImage(Bitmap image, int quality)
        {
            //var img = new Bitmap(image); // can't use clone
            var img = (Bitmap)image.Clone();
            if (quality < 0 || quality > 100)
                throw new ArgumentOutOfRangeException("quality must be between 0 and 100.");

            // Encoder parameter for image quality 
            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            // JPEG image codec 
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");
            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;
            var memoryStream = new MemoryStream();
            img.Save(memoryStream, jpegCodec, encoderParams); https://stackoverflow.com/questions/22235156/reducing-jpeg-image-quality-without-saving
            var lowQualityImage = (Bitmap)Image.FromStream(memoryStream);
            // memoryStream.Close(); causing error and does not seem to have to be called: https://stackoverflow.com/questions/4274590/memorystream-close-or-memorystream-dispose
            return lowQualityImage;

        }
        static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats 
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec 
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];

            return null;
        }

   
        static List<TestImage> list = new List<TestImage>();
        public static void Name(this Bitmap bitmap, string name) {
            if (list.Exists((t) => t.bitmap == bitmap || t.name == name)) {
                System.Diagnostics.Debug.WriteLine("Could not name bitmap as " + name);
                return;
            }
            list.Add(new TestImage() { bitmap = bitmap, name = name }); 
        }
        public static string Name(this Bitmap bitmap)
        {
            return list.Find((t) => t.bitmap == bitmap).name;
        }
        public static string Name(this float[] imageFloatArray)
        {
            return list.Find((t) => Enumerable.SequenceEqual(t.bitmap.ToValueArray(), imageFloatArray)).name;
        }
        public static float[] ToValueArray(this Bitmap image)
        {
            var array = new float[image.Width * image.Height];
            /* too slow
            for (int i = 0; i < array.Length; i ++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    for (int y = 0; y < image.Height; y++)
                    {
                        array[i] = image.GetPixel(x, y).ToArgb();
                    }
                }
            }
            */

            var float2DArray = image.To2DValueArray();

        https://stackoverflow.com/questions/5132397/fast-way-to-convert-a-two-dimensional-array-to-a-list-one-dimensional
            // buffer copy does not effect the order of the elements - so 2d -> 1d -> 2D is possible.
            Buffer.BlockCopy(float2DArray, 0, array, 0, array.Length * sizeof(float));

            return array;
        }
        static float[,] To2DValueArray(this Bitmap image)
        {
            var array = new float[image.Width, image.Height];
            for(int x = 0; x < image.Width; x ++)
            {
                for(int y = 0; y < image.Height; y ++)
                {
                    var pixel = image.GetPixel(x, y);
                    Color.FromArgb(pixel.ToArgb());
                    
                    array[x, y] = image.GetPixel(x, y).ToArgb();
                }
            }
            return array;
        }


    }
    
    class TestImage
    {
        public Bitmap bitmap;
        public string name;
    }
}
