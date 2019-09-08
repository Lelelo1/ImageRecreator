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
            var original = Images(originalUrl)[0];
            original.Name("MyImage");
            original.LowQualityImages(12);

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
