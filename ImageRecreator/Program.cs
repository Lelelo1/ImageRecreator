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
            
            CreateTrainingSet.Create();
            // Consume.Run();
            var originalUrl = ImageUrls(1);
            var originals = Images(originalUrl);
            originals[0].Name("MyImage0");
            Train.Run(originals);
            
            /* testing paint method. works. And argb is a range value 
            var originalUrls = ImageUrls(1);
            var originals = Images(originalUrls);
            originals[0].Name("MyImage0");
            var lowQualityImages = originals[0].LowQualityImages(2);
            lowQualityImages[0].Name("MyImage0q0");
            lowQualityImages[0].Save("./" + lowQualityImages[0].Name() + ".jpg");
            var paintImage = lowQualityImages[0].ToFloatArray().Paint(originals[0].Width, originals[0].Height);
            paintImage.Save("./myimage.jpg");
            */
            /*
            var my2DArray = new int[3, 2];
            my2DArray[0, 0] = 1;
            my2DArray[1, 0] = 2;
            my2DArray[2, 0] = 3;
            my2DArray[0, 1] = 4;
            my2DArray[1, 1] = 5;
            my2DArray[2, 1] = 6;

            Console.WriteLine("original 2DArray...");
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    Console.WriteLine(my2DArray[x, y]);
                }
            }


            var my1DArray = new int[3 * 2];
            Buffer.BlockCopy(my2DArray, 0, my1DArray, 0, my1DArray.Length * sizeof(int));
            Console.WriteLine("one dimensional array...");
            foreach (var v in my1DArray)
            {
                Console.WriteLine(v);
            }

            var backTo2DArray = new int[3, 2];

            Buffer.BlockCopy(my1DArray, 0, backTo2DArray, 0, backTo2DArray.Length * sizeof(int));

            Console.WriteLine("backTo2DArray...");
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    Console.WriteLine(backTo2DArray[x, y]);
                }
            }
            */
            Debug.WriteLine("end");
        }
        
        

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
