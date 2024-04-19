namespace AspireAppv6.Web;

public class WeatherApiClient(HttpClient httpClient)
{
    public async Task<WeatherForecast[]> GetWeatherAsync(
        int maxItems = 10,
        CancellationToken cancellationToken = default
    )
    {
        return await GetWeatherInternalAsync("/weatherforecast", 10, cancellationToken);
    }

    public async Task<WeatherForecast[]> GetWeatherHttpsAsync(
        int maxItems = 10,
        CancellationToken cancellationToken = default
    )
    {
        return await GetWeatherInternalAsync("/weatherforecast_https", 10, cancellationToken);
    }

    public async Task<WeatherForecast[]> GetWeatherInternalAsync(
        string url,
        int maxItems = 10,
        CancellationToken cancellationToken = default
    )
    {
        List<WeatherForecast>? forecasts = null;

        await foreach (
            var forecast in httpClient.GetFromJsonAsAsyncEnumerable<WeatherForecast>(
                url,
                cancellationToken
            )
        )
        {
            if (forecasts?.Count >= maxItems)
            {
                break;
            }
            if (forecast is not null)
            {
                forecasts ??= [];
                forecasts.Add(forecast);
            }
        }

        return forecasts?.ToArray() ?? [];
    }
}

public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
