
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Entities;
using Portfolio.Repositories;
using Portfolio.Utilities;


namespace Portfolio.Controllers;

[Authorize]
public class BlogsController : Controller
{
    private readonly IBlogsDataRepository<BlogModel> BlogDataRepo;

    public BlogsController(IBlogsDataRepository<BlogModel> BlogDataRepo)
    {
        this.BlogDataRepo = BlogDataRepo;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        ViewBag.BlogsList = await BlogDataRepo.GetBlogDataAsync();

        return View();
    }

    [AllowAnonymous]
    public async Task<IActionResult> BlogCard(string title)
    {
        ViewBag.BlogCard = await BlogDataRepo.GetBlogByTitleAsync(title);

        return View();
    }


    public IActionResult AddBlog()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> AddBlog(BlogModel vm)
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

        vm.PreviewDesc = vm.FullDesc.Substring(0, vm.FullDesc.IndexOf('!')+1);

        var data = await BlogDataRepo.AddBlogAsync(vm);

        if (!data.flag)
        {
            TempData[TempDataKeys.ALERT_ERROR] = data.message;
            return View();
        }

        TempData[TempDataKeys.ALERT_SUCCESS] = "Blog Added!";

        return View();
    }
}