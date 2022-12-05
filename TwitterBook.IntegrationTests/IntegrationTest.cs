using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TwitterBook.Contracts.V1;
using TwitterBook.Contracts.V1.Requests;
using TwitterBook.Contracts.V1.Response;
using TwitterBook.Data;
using TwitterBook.Domain;

namespace TwitterBook.IntegrationTests;

public class IntegrationTest
{
    protected readonly HttpClient TestClient;

    protected IntegrationTest()
    {
                    var root = new InMemoryDatabaseRoot();
        var appFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // var descriptor = services.SingleOrDefault(x =>
                    //     x.ServiceType == typeof));
                    // if (descriptor == null)
                    //     services.Remove(descriptor);
                    services.RemoveAll(typeof(DbContextOptions<DbContext>));
                    services.AddDbContext<DataContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDb", root);
                    });
                });
            });
        TestClient = appFactory.CreateClient();
    }

    protected async Task AuthenticateAsync()
    {
        TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAsync());
    }

    private async Task<String> GetJwtAsync()
    {
        var response = await TestClient.PostAsJsonAsync(ApiRoutes.Identity.Register, new UserRegistrationRequest
        {
            Email = "qwert@test.com",
            Password = "Blendi123."
        });
        var registrationResponse = await response.Content.ReadAsAsync<AuthSuccesResponse>();
        return registrationResponse.Token;
    }
}