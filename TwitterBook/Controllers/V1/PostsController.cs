using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwitterBook.Contracts.V1;
using TwitterBook.Contracts.V1.Requests;
using TwitterBook.Contracts.V1.Response;
using TwitterBook.Domain;
using TwitterBook.Extensions;
using TwitterBook.Services;

namespace TwitterBook.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Poster")]
    public class PostsController : Controller
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public PostsController(IPostService postService, IMapper mapper)
        {
            _postService = postService;
            _mapper = mapper;
        }

        [HttpGet(ApiRoutes.Posts.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var posts = await _postService.GetPostsAsync();
            var postResponse = _mapper.Map<List<PostResponse>>(posts);
            return Ok(postResponse);
        }

        [HttpGet(ApiRoutes.Posts.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid postId)
        {
            var post = await _postService.GetPostByIdAsync(postId);
            if (post == null)
                return NotFound();
            return Ok(_mapper.Map<PostResponse>(post));
        }

        [HttpPut(ApiRoutes.Posts.Update)]
        public async Task<IActionResult> Update([FromBody] UpdatePostRequest request)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(request.PostId, HttpContext.GetUserId());
            if (!userOwnsPost)
            {
                return BadRequest(new { error = "You do not own this post" });
            }

            var post = await _postService.GetPostByIdAsync(request.PostId);
            post.Name = request.Name;

            var updated = await _postService.UpdatePostAsync(post);
            if (updated)
                return Ok(_mapper.Map<PostResponse>(post));

            return NotFound();
        }

        [HttpDelete(ApiRoutes.Posts.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid postId)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserId());
            if (!userOwnsPost)
            {
                return BadRequest(new { error = "You do not own this post" });
            }

            var deleted = await _postService.DeletePostAsync(postId);
            if (deleted)
                return NoContent();
            return NotFound();
        }

        [HttpPost(ApiRoutes.Posts.Create)]
        public async Task<IActionResult> Create([FromBody] CreatePostRequest postRequest)
        {
            var newpostId = Guid.NewGuid();
            var post = new Post
            {
                Id = newpostId,
                Name = postRequest.Name,
                UserId = HttpContext.GetUserId(),
                PostTags = postRequest.tagNames.Select(x => new PostTag { PostId = newpostId, TagName = x }).ToList()
            };
            if (!await _postService.CreatePostAsync(post))
                return BadRequest();

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            var locationUri = baseUrl + "/" + ApiRoutes.Posts.Get.Replace("{postId}", post.Id.ToString());
            var response = _mapper.Map<PostResponse>(post);
            return Created(locationUri, response);
        }
    }
}