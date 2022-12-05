using Swashbuckle.Swagger.Model;
using TwitterBook.Domain;

namespace TwitterBook.Services;

public interface IPostService
{
    Task <List<Post>> GetPostsAsync();
    Task<Post> GetPostByIdAsync(Guid postId);
    Task<bool> UpdatePostAsync(Post postToUpdate);
    Task<bool> DeletePostAsync(Guid postId);
    Task<bool> CreatePostAsync(Post post);
    Task<bool> UserOwnsPostAsync(Guid postId, string userId);
    Task<List<Tags>> GetAllTagsAsync();
}