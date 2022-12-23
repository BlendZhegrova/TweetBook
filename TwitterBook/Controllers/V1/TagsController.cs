using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwitterBook.Contracts.V1;
using TwitterBook.Contracts.V1.Requests;
using TwitterBook.Contracts.V1.Response;
using TwitterBook.Extensions;
using TwitterBook.Services;

namespace TwitterBook.Controllers.V1;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Poster")]
public class Tags : Controller
{
    private readonly IPostService _postService;
    private readonly IMapper _mapper;
    public Tags(IPostService postService, IMapper mapper)
    {
        _postService = postService;
        _mapper = mapper;
    }

    [HttpGet(ApiRoutes.Tags.GetAll)]
    public async Task<IActionResult> GetAllAsync()
    {
        var tags = await _postService.GetAllTagsAsync();
        // var tagsResponse = tags.Select(x => new TagResponse
        // {
        //     TagName = x.TagName
        // }).ToList();
        var tagsResponse = _mapper.Map<List<TagResponse>>(tags);
        return Ok(tagsResponse);
    }

    [HttpGet(ApiRoutes.Tags.Get)]
    public async Task<IActionResult> GetTag(string tagId)
    {
        var tag = await _postService.GetTagById(tagId);
        if (tag == null)
            return NotFound();
        return Ok(_mapper.Map<TagResponse>(tag));
    }

    [HttpPost(ApiRoutes.Tags.Create)]
    public async Task<IActionResult> Create([FromBody] CreateTagRequest request)
    {
        var userId = HttpContext.GetUserId();
        Domain.Tags newTag = new()
        {
            Id = Guid.NewGuid().ToString(),
            TagName = request.TagName,
            CreatorId = userId
        };
        var created = await _postService.CreateTagsAsync(newTag);
        if (!created)
            return BadRequest();

        var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
        var locationUri = baseUrl + "/" + ApiRoutes.Tags.Get.Replace("{tagId}", newTag.Id);
        var response = _mapper.Map<TagResponse>(newTag);
        return Created(locationUri, response);
    }

    [HttpPut(ApiRoutes.Tags.Update)]    
    public async Task<IActionResult> UpdateTagAsync([FromBody] UpdateTagRequest request)
    {
        var tag = await _postService.GetTagById(request.Id);
        tag.TagName = request.tagName;
        
        var updated = await _postService.UpdateTagAsync(tag);
        
        if (updated)
        {
            return Ok(_mapper.Map<TagResponse>(tag));
        }

        return NotFound();
    }

    [HttpDelete(ApiRoutes.Tags.Delete)]
    [Authorize(Policy = "company.com")]
    public async Task<IActionResult> DeleteTagAsync([FromRoute] string tagId)
    {
        var tag = await _postService.GetTagById(tagId);
        if (HttpContext.GetUserId() != tag.CreatorId)
        {
            return BadRequest();
        }

        var deleted = await _postService.DeleteTagAsync(tagId);
        if (deleted)
        {
            return NoContent();
        }

        return NotFound();
    }
}