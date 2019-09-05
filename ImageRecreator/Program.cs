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
        static void Main(string[] args)
        {
            Debug.WriteLine("start");
            var urls = ImageUrls(10, "lowquality1");
            Debug.WriteLine("urls are...");
            urls.Print();
            var images = Images(urls);
            Debug.WriteLine("images created...");
            images.Print();
            Debug.WriteLine("lowering quality...");
            SaveJpeg("./myimg.jpg", images[10], 1);
            Debug.WriteLine("end");
        }
        
        // https://stackoverflow.com/questions/4161873/reduce-image-size-c-sharp
        static void SaveJpeg(string path, Image img, int quality)
        {
            if (quality < 0 || quality > 100)
                throw new ArgumentOutOfRangeException("quality must be between 0 and 100.");

            // Encoder parameter for image quality 
            EncoderParameter qualityParam = new EncoderParameter(Encoder.Quality, quality);
            // JPEG image codec 
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");
            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;
            img.Save(path, jpegCodec, encoderParams);
            
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
        // https://stackoverflow.com/questions/11801630/how-can-i-convert-image-url-to-system-drawing-image
        static List<Image> Images(List<string> imageUrls)
        {
            var webClient = new WebClient();
            var list = new List<Image>();
            foreach (var url in imageUrls)
            {   
                list.Add(Image.FromStream(new MemoryStream(webClient.DownloadData(url))));
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
        public static void Print(this List<Image> list)
        {
            foreach (var image in list)
            {
                Debug.WriteLine(image);
            }
        }
    }
}
