using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace pluralsightfuncs
{
    public static class GenerateLicenseFile
    {
        //[Blob("licenses/{rand-guid}.lic")]TextWriter outputBlob
        //storage creds: username: contosoadmin password: il0v3Sec1ty#
        [FunctionName("GenerateLicenseFile")]
        public static async Task Run([QueueTrigger("orders", Connection = "AzureWebJobsStorage")]Order order,
        IBinder binder,
        ILogger log)
        {
            var secret = "adfFAD@#$aasdf2344/0()'";
            var outputBlob = await binder.BindAsync<TextWriter>(
                new BlobAttribute(blobPath:$"licenses/{order.OrderId}.lic")
                {
                    Connection = "AzureWebJobsStorage"
                });

            outputBlob.WriteLine($"OrderId: {order.OrderId}");
            outputBlob.WriteLine($"Email: {order.Email}");
            outputBlob.WriteLine($"ProductId: {order.ProductId}");
            outputBlob.WriteLine($"PurchaseDate: {DateTime.UtcNow}");
            var md5 = System.Security.Cryptography.MD5.Create();
            var hash = md5.ComputeHash(
                System.Text.Encoding.UTF8.GetBytes(order.Email + "secret"));
            outputBlob.WriteLine($"SecretCode: {secret}");
            //outputBlob.WriteLine($"SecretCode: {BitConverter.ToString(hash).Replace("-","")}");
        }
    }
}