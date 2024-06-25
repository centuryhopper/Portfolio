
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Entities;
using Portfolio.Repositories;


namespace Portfolio.Controllers;

[Authorize]
public class BlogsController : Controller
{
    private readonly IBlogsDataRepository<BlogModel> BlogDataRepo;

    public BlogsController(IBlogsDataRepository<BlogModel> BlogDataRepo)
    {
        this.BlogDataRepo = BlogDataRepo;
    }

    public async Task<IActionResult> Blogs()
    {
        ViewBag.BlogsList = await BlogDataRepo.GetBlogDataAsync();

        return View();
    }

    public async Task<IActionResult> BlogCard(string title)
    {
        ViewBag.BlogCard = await BlogDataRepo.GetBlogByTitleAsync(title);

        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Blogs(BlogModel vm)
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
                "url": "string",
                "title": "string"
                }
            ],
            "fullDesc": "string"
        }
        
        */
        if (!ModelState.IsValid)
        {

        }

        var data = await BlogDataRepo.AddBlogAsync(vm);

        if (!data.flag)
        {
            return BadRequest(data);
        }

        return Ok(data);
    }
}