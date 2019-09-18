using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing;
using System.Net;
using System.IO;
using System.Linq;
namespace ImageRecreator
{
    class Program
    {
        static string testFolder = "./images";
        static void Main(string[] args)
        {
            Debug.WriteLine("start");
            /*
            var urls = ImageUrls(10);
            Debug.WriteLine("urls are...");
            urls.Print();
            var images = Images(urls);
            Debug.WriteLine("images created...");
            images.Print();
            Debug.WriteLine("lowering quality...");
            var lowImage = LowQuality(images[0], 1);
            images[0].Save("./original.jpg");
            lowImage.Save("./myjpg.jpg");
            */



            /*
            // testing paint method. works. And argb is a range value 
            var originalUrls = ImageUrls(1);
            var originals = Images(originalUrls);
            originals[0].Name("MyImage0");
            var lowQualityImages = originals[0].LowQualityImages(2);
            lowQualityImages[0].Name("MyImage0q0");
            lowQualityImages[0].Save("./" + lowQualityImages[0].Name() + ".jpg");
            var paintImage = lowQualityImages[0].ToValueArray().Paint(originals[0].Width, originals[0].Height);
            paintImage.Save("./myimage.jpg");
            */


            /*
            // is there more white (-1) in orginal than in original? Yes, original: 325525 and q0: 638055
            var urls = ImageUrls(1);
            var original = Images(urls)[0];
            original.Name("myimg");
            Debug.WriteLine("original argb -1 count: " + original.GetPixelsCountOf(-1));
            var lowQuality = original.LowQualityImages(2)[0];
            lowQuality.Name("myimgq0");
            Debug.WriteLine("low quality q0 -1 count: " + lowQuality.GetPixelsCountOf(-1));
            */

            /*
            // train
            var originalUrl = RandomImages(2);
            var originals = Images(originalUrl);
            for(int i = 0; i < originalUrl.Count; i++)
            {
                originals[i].Name(originalUrl[i]);
            }
            Train.Run(originals);
            */

            /*
            //consume
            var urls = ImageUrls(1);
            var original = Images(urls)[0];
            original.Name("myimg");
            var lowQuality = original.LowQualityImages(2)[0];
            lowQuality.Name("myimgq0");
            var restored = lowQuality.Restore(original);
            original.Save("myimg.jpg");
            lowQuality.Save("myimgq0.jpg");
            restored.Save("myimgq0r.jpg");
            */

            /*
            // q0 is much smaller even with random image
            var original = new Bitmap(2448, 3264);
            original.Name("myrandom");
            original.Random().Save("myrandom.jpg");
            var lowImage = original.LowQualityImages(2)[0];
            lowImage.Name("myrandomq0");
            lowImage.Save("myrandomq0.jpg");
            */

            /*
            // train
            var images = RandomImages(20000, 1, 1);
            Debug.WriteLine("images count: " + images.Count);
            Train.Run(images);
            */

            /*
            // can use bitconverter bak and forth
            byte b = 2;
            byte[] a = new byte[4] { 255, 255, 255, 255 };
            var s = BitConverter.ToSingle(a);
            var c = Color.FromArgb(10, 10, 10, 10);
            Debug.WriteLine("argb: " + c.ToArgb());
            var f = BitConverter.ToSingle(a);
            Debug.WriteLine("f: " + f);
            var arr = BitConverter.GetBytes(f);
            int index = 0; 
            foreach (var by in arr)
            {
                Debug.WriteLine(by);
                index++;
            }
            */
            /*
            // argb vs rgba
            var f = new byte[4] { 135, 135, 136, 135 }.toInt();
            Debug.WriteLine("f: " + f);
            Debug.WriteLine(Color.FromArgb(f).R + ", " + Color.FromArgb(f).G + ", " + Color.FromArgb(f).B + ", " + Color.FromArgb(f).A);
            new Bitmap(200, 200).Paint(f).Save("./animg.jpg");
           */
            

            var bitmap = new Bitmap(1, 1);
            bitmap.Name("original");
            for(int x = 0; x < bitmap.Width; x ++)
            {
                for(int y = 0; y < bitmap.Height ; y ++)
                {
                    bitmap.SetPixel(x, y, Color.Red);
                }
            }
            // bitmap.SetPixel(0, 0, Color.Black);
            Debug.WriteLine("Original size per pixel: " + bitmap.MemorySizePerPixel(ImageFormat.Bmp));
            bitmap.Save("./myimg.jpg");
            var low = bitmap.LowQualityImages(2)[0];
            low.SetPixel(0, 0, Color.Yellow);
            Debug.WriteLine("lowQualityImage size per pixel: " + low.MemorySizePerPixel(ImageFormat.Bmp));
            low.Save("./mymgq0.jpg");
            Debug.WriteLine("end");
        }
        
        static List<Bitmap> RandomImages(int amount, int width, int height)
        {
            var original = new Bitmap(width, height);
            var list = new List<Bitmap>();

            for(int i = 0; i < amount; i++)
            {
                list.Add(original.Random());
            }
            return list;
        }

        // is bounds width: 2448, height: 3264

        // https://stackoverflow.com/questions/11801630/how-can-i-convert-image-url-to-system-drawing-image
        static List<Bitmap> Images(List<string> imageUrls)
        {
            var webClient = new WebClient();
            var list = new List<Bitmap>();
            foreach (var url in imageUrls)
            {
                var image = (Bitmap)Image.FromStream(new MemoryStream(webClient.DownloadData(url)));
                list.Add(image);
            }
            return list;
        }

        
        // fetch all images 
        // https://stackoverflow.com/questions/30706550/azure-blob-storage-download-all-files
        static List<String> ImageUrls(int amount, string and = null) // and - specify a specific url additionally
        {
            var blobItems = BlobContainer()
                .ListBlobs()
                .ToList();
            var urls = blobItems.GetRange(0, amount)
                .Select((blobItem) => blobItem.Uri.OriginalString).ToList();
            if(and != null)
            {
                var found = blobItems.Find((blobItem) => blobItem.Uri.OriginalString.Contains(and));
                urls.Add(found.Uri.OriginalString);
            }
            return urls;
                

        }

        static string containerName = "images";
        static CloudBlobContainer BlobContainer()
        {
            CloudBlobClient cloudBlobClient = Authenticate().CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);
            return cloudBlobContainer;
        }
        static CloudStorageAccount Authenticate()
        {
            var storageConnectionString = Environment.GetEnvironmentVariable("CONNECT_STR");
            Debug.WriteLine("using {0} as storageConnectionString: " + storageConnectionString); // debug and console format works differently https://stackoverflow.com/questions/22819117/why-does-debug-writeline-incorrectly-format-strings
            var storageAccount = CloudStorageAccount.Parse(storageConnectionString);

            return storageAccount;
        }
        
    }
    static class Printer
    {
        public static void Print(this List<string> list)
        {
            foreach(var url in list)
            {
                Debug.WriteLine(url);
            }
        }
        public static void Print(this List<Bitmap> list)
        {
            foreach (var image in list)
            {
                Debug.WriteLine(image);
            }
        }
    }
}
