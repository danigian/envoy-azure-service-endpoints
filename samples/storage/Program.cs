using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace storage
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string proxyUrl = Environment.GetEnvironmentVariable("proxyUrl");
            string storageName = Environment.GetEnvironmentVariable("storageName");
            string storageKey = Environment.GetEnvironmentVariable("storageKey");
            string containerName = Environment.GetEnvironmentVariable("containerName");
            string fileName = "proxyfile.txt";
            string fileContent = "Written through Envoy Proxy";
            try
            {
                BlobHelper _blobHelper = new BlobHelper(proxyUrl, storageName, storageKey);
                await _blobHelper.CreateContainer(containerName);
                await _blobHelper.WriteBlob(containerName, fileName, fileContent);
                Console.WriteLine("*** Read blob '{0}' from container '{1}'***", fileName, containerName);
                Console.WriteLine("*** Blob content is: '{0}' ***", await _blobHelper.ReadBlob(containerName, fileName));

            }
            catch (StorageException ex)
            {
                Console.WriteLine("Error returned from the service: {0}", ex.Message);
            }
        }
    }

    class BlobHelper
    {
        private CloudBlobClient cloudBlobClient { get; set; }

        public BlobHelper(string proxyUrl, string storageName, string storageKey)
        {
            var storCred = new StorageCredentials(storageName, storageKey);
            var storUri = new StorageUri(new Uri(proxyUrl));
            this.cloudBlobClient = new CloudBlobClient(storUri, storCred);
        }

        public async Task CreateContainer(string containerName)
        {
            Console.WriteLine("*** Creating container '{0}' ***", containerName);
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);
            await cloudBlobContainer.CreateIfNotExistsAsync();
            Console.WriteLine("*** Created container '{0}' ***", containerName);
        }

        public async Task WriteBlob(string containerName, string fileName, string content)
        {
            Console.WriteLine("*** Writing blob '{0}' to container '{1}' ***", fileName, containerName);

            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);
            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
            await cloudBlockBlob.UploadTextAsync(content);

            Console.WriteLine("*** Written blob '{0}' to container '{1}' ***", fileName, containerName);
        }

        public async Task<string> ReadBlob(string containerName, string fileName)
        {
            Console.WriteLine("*** Reading blob '{0}' from container '{1}' ***", fileName, containerName);

            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);
            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
            return await cloudBlockBlob.DownloadTextAsync();
        }
    }
}
