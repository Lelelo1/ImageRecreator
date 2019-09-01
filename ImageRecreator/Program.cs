using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
namespace ImageRecreator
{
    class Program
    {
        static void Main(string[] args)
        {
            Debug.WriteLine("start");
            ListImages(10).Print();
            Debug.WriteLine("end");
        }
        // connecting to blob storage https://docs.microsoft.com/en-us/azure/storage/blobs/storage-quickstart-blobs-dotnet
        static List<String> ListImages(int amount)
        {
            return BlobContainer()
                .ListBlobs()
                .ToList()
                .GetRange(0, amount)
                .Select((blobItem) => blobItem.Uri.OriginalString).ToList();

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
    }
}

// fetch all images 
// https://stackoverflow.com/questions/30706550/azure-blob-storage-download-all-files