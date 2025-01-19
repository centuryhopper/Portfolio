
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Server.Repositories;
using Shared.Models;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogsController : ControllerBase
{
    private readonly IBlogsDataRepository<BlogDTO> BlogDataRepo;
    private readonly ILogger<BlogsController> logger;

    public BlogsController(IBlogsDataRepository<BlogDTO> BlogDataRepo, ILogger<BlogsController> logger)
    {
        this.BlogDataRepo = BlogDataRepo;
        this.logger = logger;
    }

    [HttpGet]
    [Route("get-blogs")]
    public async Task<IActionResult> GetAsync()
    {
        // logger.LogWarning("getting blogs");
        var data = await BlogDataRepo.GetBlogDataAsync();
        //logger.LogWarning(JsonConvert.SerializeObject(data));

        return Ok(data);
    }

    [HttpDelete("delete-blogs/{blogId:int}")]
    [Authorize(Roles="Admin")]
    public async Task<IActionResult> DeleteAsync(int blogId)
    {
        var response = await BlogDataRepo.DeleteBlogAsync(blogId: blogId);
        if (!response.Flag)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpGet]
    [Route("sort-blogs/{isNewest:bool}")]
    public async Task<IActionResult> SortAsync(bool isNewest)
    {
        var data = await BlogDataRepo.SortAsync(isNewest);

        return Ok(data);
    }

    [HttpGet]
    [Route("get-blog/{blogId:int}")]
    public async Task<IActionResult> GetBlogById(int blogId)
    {
        var data = await BlogDataRepo.GetBlogById(blogId: blogId);
        return Ok(data);
    }

    [HttpPost("add-blog")]
    [RequestSizeLimit(100_000_000)]
    [Authorize(Roles="Admin")]
    public async Task<IActionResult> PostBlogAsync([FromBody] BlogDTO BlogDTO)
    {
        /*
        example of a working body in code-first approach
        {
            "title": "string",
            "date": "2019-01-06T17:16:40",
            "previewDesc": "string",
            "routeName": "string",
            "videoUrls": [
                {
                    "url": "string"
                }
            ],
            "blogImageDTOs": [
                {
                    "imageData": "iVBORw0KGgoAAAANSUhEUgAAAAUAAAAFCAYAAACNbyblAAAAHElEQVQI12P4",
                    "contentType": "image/jpeg"
                }
            ],
            "fullDesc": "string"
        }

            insert into blog (title, date, preview_desc, route_name, full_desc) values(
                'dummy',
                '2019-01-06T17:16:40',
                'dummy',
                'dummy',
                'dummy'
            );
                    
        */
        var data = await BlogDataRepo.AddBlogAsync(BlogDTO);

        if (!data.Flag)
        {
            return BadRequest(data);
        }

        return Ok(data);
    }


    [HttpPut("edit-blog")]
    [Authorize(Roles="Admin")]
    public async Task<IActionResult> EditBlogAsync([FromBody] BlogDTO BlogDTO)
    {
        var data = await BlogDataRepo.EditBlogAsync(BlogDTO);

        if (!data.Flag)
        {
            return BadRequest(data);
        }

        return Ok(data);
    }
}