using Swashbuckle.Swagger.Model;
using TwitterBook.Contracts.V1.Requests;
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

    Task<bool> CreateTagsAsync(Tags tag);
    Task<List<Tags>> GetAllTagsAsync();
    public Task<bool> DeleteTagAsync(string tagId);
    public Task<bool> UpdateTagAsync(Tags tagToUpdate);
    public Task<Tags> GetTagById(string tagId);
}