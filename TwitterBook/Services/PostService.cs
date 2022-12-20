using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.Swagger.Model;
using TwitterBook.Contracts.V1.Requests;
using TwitterBook.Data;
using TwitterBook.Domain;
using TwitterBook.Extensions;

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
    public async Task<bool> CreatePostAsync(CreatePostRequest postRequest, string userId)
    {
        var post = new Post
        {
            Id = Guid.NewGuid(),
            Name = postRequest.Name,
            UserId = userId,
        };
        await _dataContext.Posts.AddAsync(post);
        foreach (var tag in postRequest.tagNames)
        {
            PostTag NewTag = new()
            {
                TagName = tag,
                PostId = post.Id,
            };
            await _dataContext.PostTags.AddAsync(NewTag);
        }

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
    
    public async Task<List<Tags>> GetAllTagsAsync()
    {
        return await _dataContext.Tags.ToListAsync();
    }

    public async Task<Tags> GetTagById(string tagId)
    {
        return await _dataContext.Tags.SingleOrDefaultAsync(x => x.Id == tagId);
    }
    public async Task<bool> CreateTagsAsync(List<string> tagsList, Guid postId, string userId)
    {
        if (tagsList.Count == 0 && postId == null)
        {
            return false;
        }
        foreach (var tag in tagsList)
        {
            Tags NewTag = new()
            {
                TagName = tag,
                PostId = postId,
                CreatorId = userId
            };
            _dataContext.Tags.Add(NewTag);
        }

        await _dataContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateTagAsync(Tags tagToUpdate)
    {
        _dataContext.Tags.Update(tagToUpdate);
        var updated = await _dataContext.SaveChangesAsync();
        return updated > 0;
    }
    public async Task<bool> DeleteTagAsync(string tagId)
    {
        var tag = await _dataContext.Tags.SingleOrDefaultAsync(x=> x.Id == tagId);
        _dataContext.Tags.Remove(tag);
        var deleted = await _dataContext.SaveChangesAsync();
        return deleted > 0;
    }
}