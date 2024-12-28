using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class RickAndMortyService
{
    private readonly HttpClient _httpClient;

    public RickAndMortyService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

  public async Task<object> GetEpisodesAsync(int page, string? name = null)
{
    // Validar que el número de página sea válido
    if (page < 1)
    {
        throw new ArgumentException("Page number must be 1 or greater.");
    }

    var url = $"https://rickandmortyapi.com/api/episode?page={page}";
    if (!string.IsNullOrEmpty(name))
    {
        url += $"&name={Uri.EscapeDataString(name)}";
    }

    var response = await _httpClient.GetAsync(url);
    if (!response.IsSuccessStatusCode)
    {
        throw new HttpRequestException($"Error fetching episodes: {response.ReasonPhrase}");
    }

    var content = await response.Content.ReadAsStringAsync();
    return JsonSerializer.Deserialize<object>(content, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }) ?? new { results = Array.Empty<object>() };
}

    
}
