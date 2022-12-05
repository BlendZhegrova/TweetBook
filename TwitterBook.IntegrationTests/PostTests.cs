using System.Net;
using FluentAssertions;
using TwitterBook.Contracts.V1;
using TwitterBook.Domain;

namespace TwitterBook.IntegrationTests;

public class PostTests : IntegrationTest
{
    
    [Fact]
    public async Task GetAll_WithoutAnyPostsReturnsEmpty()
    {
        //Arrange
        await AuthenticateAsync();
        //Act
        var response = await TestClient.GetAsync(ApiRoutes.Posts.GetAll);
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<List<Post>>()).Should().BeNull();
    }

    [Fact]
    public async Task GetAllReturnsPosts()
    {
        //Arrange
        await AuthenticateAsync();
        //Act
        var response = await TestClient.GetAsync(ApiRoutes.Posts.GetAll);
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadAsAsync<List<Post>>()).Should().NotBeEmpty();   
    }

    [Theory]
    [InlineData("23")]
    public async Task ShouldReturn404PostNotFound(string value)
    {
        await AuthenticateAsync();

        var response = await TestClient.GetAsync($"{ApiRoutes.Posts.Get}/{value}");
        (await response.Content.ReadAsAsync<Post>()).Should().Be(HttpStatusCode.BadRequest);   

    }
    [Theory]
    [InlineData("C3B96F7F-123A-4DFC-205B-08DAD3AC93D5")]
    public async Task ShoudlReturnAPost(string value)
    {
        await AuthenticateAsync();

        var response = await TestClient.GetAsync($"api/v1/posts/{value}");
        (await response.Content.ReadAsAsync<Post>()).Should().Be(HttpStatusCode.OK);   

    }
}