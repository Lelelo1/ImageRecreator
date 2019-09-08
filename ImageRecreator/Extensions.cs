using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Drawing.Imaging;
using System.IO;
using System.Diagnostics;
namespace ImageRecreator
{
    public static class Extensions
    {

        public static void Print(this Data d)
        {
            Console.WriteLine("total: " + d.total.Name() +
                ", index: (x: " + d.index[0] + ", y: " + d.index[1] + "), value: " + d.value + ", original: " + d.original);
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
    }
    class TestImage
    {
        public Bitmap bitmap;
        public string name;
    }
}
