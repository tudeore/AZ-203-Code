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
            CreateContainer("wizhlabsdemonew").GetAwaiter().GetResult();
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
