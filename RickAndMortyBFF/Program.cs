var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddHttpClient<RickAndMortyService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://localhost:4200", "https://localhost:7247")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});


var app = builder.Build();

// Use CORS policy
app.UseCors("AllowFrontend");

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Endpoint para episodios
app.MapGet("/episodes", async (RickAndMortyService service, int page, string? name) =>
{
    try
    {
        var episodes = await service.GetEpisodesAsync(page, name);
        return Results.Ok(episodes);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
    catch (HttpRequestException ex)
    {
        return Results.Json(new { error = ex.Message }, statusCode: 502);
    }
    catch (Exception ex)
    {
        return Results.Json(new { error = "An unexpected error occurred." }, statusCode: 500);
    }
})
.WithName("GetEpisodes")
.WithOpenApi();

// Root endpoint
app.MapGet("/", () => "Welcome to the Rick and Morty BFF API!");

app.Run();
