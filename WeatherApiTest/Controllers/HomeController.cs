using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WeatherApiTest.Models;
using System.Net.Http;

public class HomeController : Controller
{
    private readonly HttpClient _httpClient;

    public HomeController()
    {
        _httpClient = new HttpClient();
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Index(string city, string state, string country)
    {
        var weatherModel = GetWeatherData(city, state, country);

        return View(weatherModel);
    }

    private WeatherModel GetWeatherData(string city, string countryName, string state = "")
    {
        var key = "94684d2050fd2691b4eed34a07a2679c";

        // Construct the location parameter based on the country and state
        string location = state != "" ? $"{city},{state},{countryName}" : $"{city},{countryName}";

        var weatherURL = $"https://api.openweathermap.org/data/2.5/weather?q={Uri.EscapeDataString(location)}&units=imperial&appid={key}";
        var response = _httpClient.GetStringAsync(weatherURL).Result;

        dynamic formattedResponse = JsonConvert.DeserializeObject(response);

        var weatherModel = new WeatherModel
        {
            City = $"{city}, {countryName}",
            Temperature = (double)formattedResponse["main"]["temp"],
            FeelsLike = (double)formattedResponse["main"]["feels_like"],
            MinTemperature = (double)formattedResponse["main"]["temp_min"],
            MaxTemperature = (double)formattedResponse["main"]["temp_max"]
            // Set other properties as needed
        };

        return weatherModel;
    }
}