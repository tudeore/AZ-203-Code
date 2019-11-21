using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureSqlDatabase
{
    class Program
    {
        static string conn_string = "DefaultEndpointsProtocol=https;AccountName=blostoragesample;AccountKey=qKBx5i8T3HddXh5Vj5CZDxShZ96DzLIiYGneY3UMz5TO5kfDBjGcyh6nBiDrqdh5yf3mdVEUax55Exm0YryolQ==;EndpointSuffix=core.windows.net";


        static void Main(string[] args)
        {
            //CreateContainer("wizhlabsdemonew").GetAwaiter().GetResult();
            //UploadBlob("wizhlabsdemonew", "Sample.txt", "D://Sample.txt").GetAwaiter().GetResult();
            GetBlobProperties("wizhlabsdemonew", "Sample.txt").GetAwaiter().GetResult();
        }

        static async Task GetBlobProperties(string containerName, string blobName)
        {
            CloudStorageAccount wizhlabs_storage = CloudStorageAccount.Parse(conn_string);
            CloudBlobClient wizhlabs_blobClient = wizhlabs_storage.CreateCloudBlobClient();
            CloudBlobContainer container = wizhlabs_blobClient.GetContainerReference(containerName);

            BlobContinuationToken blobContinuationToken = null;
            do
            {
                var results = await container.ListBlobsSegmentedAsync(null, blobContinuationToken);
                blobContinuationToken = results.ContinuationToken;
                foreach (CloudBlockBlob item in results.Results)
                {
                    Console.WriteLine(item.Uri);
                    Console.WriteLine(item.Properties.Created);
                    Console.WriteLine(item.Properties.ETag);
                }
            } while (blobContinuationToken != null);

            Console.ReadKey();
        }

        static async Task UploadBlob(string p_containerName, string p_fileName, string p_path)
        {
            CloudStorageAccount wizhlabs_storage = CloudStorageAccount.Parse(conn_string);
            CloudBlobClient wizhlabs_blobClient = wizhlabs_storage.CreateCloudBlobClient();
            CloudBlobContainer container = wizhlabs_blobClient.GetContainerReference(p_containerName);

            CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(p_fileName);
            await cloudBlockBlob.UploadFromFileAsync(p_path);

            Console.WriteLine("Object uploaded");
            Console.ReadKey();
        }

        static async Task CreateContainer(string p_Name)
        {
            CloudStorageAccount wizhlabs_storage = CloudStorageAccount.Parse(conn_string);
            CloudBlobClient wizhlabs_blobClient = wizhlabs_storage.CreateCloudBlobClient();
            CloudBlobContainer container = wizhlabs_blobClient.GetContainerReference(p_Name);
            await container.CreateAsync();

            BlobContainerPermissions wizhlabs_permission = new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            };

            await container.SetPermissionsAsync(wizhlabs_permission);
            Console.WriteLine("Container Created");
        }
    }
}
