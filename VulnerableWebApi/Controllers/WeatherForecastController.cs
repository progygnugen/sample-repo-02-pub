using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Runtime.Serialization.Formatters.Binary;

namespace VulnerableWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        public void VulnerableApi(string cloudId, string cloudType, byte[] bytes)
        {
            SqlConnection sqlConnection =  new SqlConnection("Server=localhost;Integrated security=SSPI;database=WeatherDB");
            SqlCommand sqlCommand = new SqlCommand($"UPDATE Clouds SET CloudType = '{cloudType}' WHERE CloudId = '{cloudId}'", sqlConnection);
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();

            // BinaryFormatter is bad. See https://aka.ms/binaryformatter
            //BinaryFormatter bf = new BinaryFormatter();
            //bf.Deserialize(new MemoryStream(bytes));
        }
    }
}
