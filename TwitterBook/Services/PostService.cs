using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.Swagger.Model;
using TwitterBook.Data;
using TwitterBook.Domain;

namespace TwitterBook.Services;

public class PostService : IPostService
{
    private readonly DataContext _dataContext;

    public PostService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<List<Post>> GetPostsAsync()
    {
        return await _dataContext.Posts.ToListAsync();
    }

    public async Task<Post> GetPostByIdAsync(Guid postId)
    {
        return await _dataContext.Posts.SingleOrDefaultAsync(x => x.Id == postId);
    }

    public async Task<bool> CreatePostAsync(Post post)
    {
        await _dataContext.Posts.AddAsync(post);
        var created = await _dataContext.SaveChangesAsync();
        return created > 0;
    }

    public async Task<bool> UserOwnsPostAsync(Guid postId, string userId)
    {
        var post = await _dataContext.Posts.AsNoTracking().SingleOrDefaultAsync(x => x.Id == postId);
        if (post == null)
        {
            return false;
        }

        if (post.UserId != userId)
        {
            return false;
        }

        return true;
    }

    public Task<List<Tags>> GetAllTagsAsync()
    {
        var tags = _dataContext.Tags.AsQueryable().DistinctBy(x=>x.TagName).ToList();
        return Task.FromResult(tags);
    }

    public async Task<bool> UpdatePostAsync(Post postToUpdate)
    {
        _dataContext.Posts.Update(postToUpdate);
        var updated = await _dataContext.SaveChangesAsync();
        return updated > 0;
    }

    public async Task<bool> DeletePostAsync(Guid postId)
    {
        var post = await GetPostByIdAsync(postId);
        _dataContext.Posts.Remove(post);
        var deleted = await _dataContext.SaveChangesAsync();
        return deleted > 0;
    }
}