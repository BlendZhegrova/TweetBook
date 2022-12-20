using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwitterBook.Contracts.V1;
using TwitterBook.Contracts.V1.Requests;
using TwitterBook.Contracts.V1.Response;
using TwitterBook.Extensions;
using TwitterBook.Services;
using TwitterBook.Domain;
using TwitterBook.Extensions;
using TwitterBook.Services;

namespace TwitterBook.Controllers.V1;
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Admin")]
public class Tags : Controller
{
    private readonly IPostService _postService;
    public Tags(IPostService postService)
    {
        _postService = postService;
    }
    [HttpGet(ApiRoutes.Tags.GetAll)]
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok(await _postService.GetAllTagsAsync());
    }
    [HttpGet(ApiRoutes.Tags.Get)]
    public async Task<IActionResult> GetTag(string tagId)
    {
        var tag = await _postService.GetTagById(tagId);
        if (tag == null)
            return NotFound();
        return Ok(new TagResponse 
        {
            Name = tag.TagName,
        });
    }
    [HttpPut(ApiRoutes.Tags.Create)]
    
    [HttpPut(ApiRoutes.Tags.Update)]
    public async Task<IActionResult> UpdateTagAsync([FromBody] UpdateTagRequest request)
    {
        var tag = await _postService.GetTagById(request.Id);

        if (HttpContext.GetUserId() != request.CreatorId)
        {
            return BadRequest();
        }
        var updated = await _postService.UpdateTagAsync(tag);
        if (updated)
        {
            return Ok(new TagResponse
            {
                Name = tag.TagName,
            });
        }
        return NotFound();
    }
    [HttpDelete(ApiRoutes.Tags.Delete)]
    public async Task<IActionResult> DeleteTagAsync([FromRoute]string tagId)
    {
        var tag = await _postService.GetTagById(tagId);
        if (HttpContext.GetUserId() != tag.CreatorId)
        {
            return BadRequest();
        }
        var deleted =await _postService.DeleteTagAsync(tagId);
        if (deleted)
        {
            return NoContent();
        }
        return NotFound();
    }
}