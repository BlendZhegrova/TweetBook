using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwitterBook.Contracts.V1;
using TwitterBook.Services;

namespace TwitterBook.Controllers.V1;
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class Tags : Controller
{
    private readonly IPostService _postService;
    public Tags(IPostService postService)
    {
        _postService = postService;
    }

    [HttpGet(ApiRoutes.Tags.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _postService.GetAllTagsAsync());
    }
}