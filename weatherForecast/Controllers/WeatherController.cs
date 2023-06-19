using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace weatherForecast.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        [HttpGet("{citySearch}")]
        public async Task<WeatherCity> GetDay(string citySearch, string? unitsInput)
        {
            string units;
            if (unitsInput == "Фарингейт")
            {
                units = "imperial";
            }
            else 
            {
                units = "metric";
            }
            WeatherCity weatherCityDay = new WeatherCity();
            string url = "http://api.openweathermap.org/geo/1.0/direct?q=" + citySearch + "&limit=5&appid=76d01ab43b24b2b3e41c7c7e9daaf327";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            var httpClient = new HttpClient();
            var result = httpClient.Send(requestMessage);
            var cityJson = await result.Content.ReadAsStringAsync();
            var citys = JsonConvert.DeserializeObject<List<City>>(cityJson);
            foreach(var city in citys)
            {
                if(city.country=="RU")
                {
                    url = "http://api.openweathermap.org/data/2.5/forecast?lat=" + city.lat+"&lon="+city.lon+ "&appid=76d01ab43b24b2b3e41c7c7e9daaf327&lang=ru&units=" + units;
                    requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                    httpClient = new HttpClient();
                    result = httpClient.Send(requestMessage);
                    var WeatherCityJson = await result.Content.ReadAsStringAsync();
                    weatherCityDay = JsonConvert.DeserializeObject<WeatherCity>(WeatherCityJson);
                    foreach(var weatherCity in weatherCityDay.list)
                    {
                        weatherCity.main.temp = Math.Round(weatherCity.main.temp, 0);
                        weatherCity.main.feels_like=Math.Round(weatherCity.main.feels_like,0);
                        weatherCity.main.temp_max = Math.Round(weatherCity.main.temp_max, 0);
                        weatherCity.main.temp_min = Math.Round(weatherCity.main.temp_min, 0);
                    }

                }
            }
            return weatherCityDay;
        }

        
    }
}
