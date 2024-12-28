using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

public class RickAndMortyServiceTests
{
    [Fact]
    public async Task GetEpisodesAsync_ValidPage_ReturnsEpisodes()
    {
        // Arrange
        var mockHttp = new HttpClient(new MockHttpMessageHandler((request) =>
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"results\": [{\"id\": 1, \"name\": \"Pilot\"}]}")
            };
        }));
        var service = new RickAndMortyService(mockHttp);

        // Act
        var result = await service.GetEpisodesAsync(1);

        // Assert
        Assert.NotNull(result);
    }

   [Fact]
public async Task GetEpisodesAsync_InvalidPage_ThrowsArgumentException()
{
    // Arrange
    var mockHttp = new HttpClient(new MockHttpMessageHandler((request) =>
    {
        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{\"results\": []}")
        };
    }));
    var service = new RickAndMortyService(mockHttp);

    // Act & Assert
    await Assert.ThrowsAsync<ArgumentException>(() => service.GetEpisodesAsync(0));
}

}

public class MockHttpMessageHandler : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, HttpResponseMessage> _handler;

    public MockHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> handler)
    {
        _handler = handler;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
    {
        return Task.FromResult(_handler(request));
    }
}
