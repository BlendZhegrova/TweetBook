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
            (await response.Content.ReadAsAsync<List<Post>>()).Should().BeEmpty();
        }
}