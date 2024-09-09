
using Microsoft.EntityFrameworkCore;
using Server.Contexts;
using Server.Entities;
using Shared.Models;
using static Shared.Models.ServiceResponses;

namespace Server.Repositories;

public class BlogsDataRepository : IBlogsDataRepository<BlogDTO>
{
    private readonly PortfolioDBContext neondbContext;

    public BlogsDataRepository(PortfolioDBContext neondbContext)
    {
        this.neondbContext = neondbContext;
    }

    public async Task<GeneralResponse> AddBlogAsync(BlogDTO model)
    {
        var blog = new Blog
        {
            Title = model.Title,
            Date = model.Date,
            PreviewDesc = model.PreviewDesc,
            RouteName = model.RouteName,
            FullDesc = model.FullDesc,
        };

        try
        {
            await neondbContext.Blogs.AddAsync(blog);
            await neondbContext.SaveChangesAsync();

            int mostRecentInsertedBlogDTO = await neondbContext.Blogs.MaxAsync(b => b.Id);

            foreach (var vUrlModel in model.VideoUrls)
            {
                var videoUrl = new VideoUrl
                {
                    BlogId = mostRecentInsertedBlogDTO,
                    Url = vUrlModel.url,
                };
                await neondbContext.VideoUrls.AddAsync(videoUrl);
            }

            await neondbContext.SaveChangesAsync();
            return new GeneralResponse(Flag: true, Message: "Added my blogs");
        }
        catch (Exception e)
        {
            return new GeneralResponse(Flag: false, Message: e.Message);
        }
    }

    public async Task<BlogDTO> GetBlogByTitleAsync(string title)
    {
        var blog = await neondbContext.Blogs.Where(b => b.Title == title).FirstAsync();

        var videoUrls = await neondbContext.VideoUrls.Where(v => v.BlogId == blog.Id).Select(v => new VideoUrlDTO
        {
            url = v.Url
            ,
            title = "test"
        }).ToListAsync();

        return new BlogDTO
        {
            Title = blog.Title
            ,
            Date = blog.Date
            ,
            PreviewDesc = blog.PreviewDesc
            ,
            RouteName = blog.RouteName
            ,
            VideoUrls = videoUrls
            ,
            FullDesc = blog.FullDesc
        };
    }

    public async Task<IEnumerable<BlogDTO>> GetBlogDataAsync()
    {
        var blogs = await neondbContext.Blogs.Include(b => b.VideoUrls).Select(b =>
            new BlogDTO
            {
                Title = b.Title,
                Date = b.Date,
                PreviewDesc = b.PreviewDesc,
                RouteName = b.RouteName,
                FullDesc = b.FullDesc,
                VideoUrls = b.VideoUrls.Select(v => new VideoUrlDTO
                {
                    title = "test",
                    url = v.Url
                }).ToList(),
            }).ToListAsync();

        return blogs;
    }

    public async Task<IEnumerable<BlogDTO>> SortAsync(bool isNewest)
    {
        var blogs = await neondbContext.Blogs.Include(b => b.VideoUrls).Select(b =>
            new BlogDTO
            {
                Title = b.Title,
                Date = b.Date,
                PreviewDesc = b.PreviewDesc,
                RouteName = b.RouteName,
                FullDesc = b.FullDesc,
                VideoUrls = b.VideoUrls.Select(v => new VideoUrlDTO
                {
                    title = "test",
                    url = v.Url
                }).ToList(),
            }).ToListAsync();

        return isNewest ?
        blogs.OrderByDescending(b => b.Date) :
        blogs.OrderBy(b => b.Date);
    }
}


