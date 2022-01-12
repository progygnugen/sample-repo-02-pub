using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace pluralsightfuncs
{
    public class Order
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string OrderId { get; set;}
        public string ProductId { get; set;}
        public string Email { get; set;}
        public decimal Price { get; set;}
    }
}