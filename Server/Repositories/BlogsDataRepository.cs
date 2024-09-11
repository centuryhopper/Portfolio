
using Microsoft.EntityFrameworkCore;
using Server.Contexts;
using Server.Entities;
using Shared.Models;
using static Shared.Models.ServiceResponses;

namespace Server.Repositories;

public class BlogsDataRepository : IBlogsDataRepository<BlogDTO>
{
    private readonly PortfolioDBContext portfolioDbContext;

    public BlogsDataRepository(PortfolioDBContext portfolioDbContext)
    {
        this.portfolioDbContext = portfolioDbContext;
    }

    public async Task<GeneralResponse> AddBlogAsync(BlogDTO model)
    {
        var blog = new Blog
        {
            Title = model.Title,
            Date = model.Date.GetValueOrDefault(),
            PreviewDesc = $"{model.FullDesc.Substring(0,Math.Min(10,model.FullDesc.Length))}...",
            RouteName = model.RouteName,
            FullDesc = model.FullDesc,
        };

        try
        {
            await portfolioDbContext.Blogs.AddAsync(blog);
            await portfolioDbContext.SaveChangesAsync();

            int mostRecentInsertedBlogDTO = await portfolioDbContext.Blogs.MaxAsync(b => b.Id);

            foreach (var vUrlModel in model.VideoUrls)
            {
                var videoUrl = new VideoUrl
                {
                    BlogId = mostRecentInsertedBlogDTO,
                    Url = vUrlModel.url,
                };
                await portfolioDbContext.VideoUrls.AddAsync(videoUrl);
            }

            foreach (var blogImage in model.BlogImageDTOs)
            {
                var obj = new Blogimage
                {
                    BlogId = mostRecentInsertedBlogDTO,
                    ImageData = blogImage.ImageData,
                    ContentType = blogImage.ContentType,
                };
                await portfolioDbContext.Blogimages.AddAsync(obj);
            }

            await portfolioDbContext.SaveChangesAsync();
            return new GeneralResponse(Flag: true, Message: "Added my blogs");
        }
        catch (Exception e)
        {
            return new GeneralResponse(Flag: false, Message: e.InnerException.Message);
        }
    }

    public async Task<GeneralResponse> DeleteBlogAsync(int blogId)
    {
        try
        {
            var blog = await portfolioDbContext.Blogs.FindAsync(blogId);
            if (blog == null)
            {
                throw new Exception("blog not found");
            }

            portfolioDbContext.Blogs.Remove(blog);
            await portfolioDbContext.SaveChangesAsync();
        }
        catch (System.Exception ex)
        {
            return new GeneralResponse(Flag: false, Message: ex.Message);
        }

        return new GeneralResponse(Flag: true, Message: "Blog deleted!");
    }

    public async Task<BlogDTO> GetBlogByTitleAsync(string title)
    {
        var blog = await portfolioDbContext.Blogs
        .Include(b => b.VideoUrls)
        .Include(b => b.Blogimages)
        .Where(b => b.Title == title).FirstAsync();

        // var videoUrls = await portfolioDbContext.VideoUrls.Where(v => v.BlogId == blog.Id).ToListAsync();

        return new BlogDTO
        {
            Title = blog.Title,
            Date = blog.Date,
            PreviewDesc = blog.PreviewDesc,
            RouteName = blog.RouteName,
            VideoUrls = blog.VideoUrls.Select(v => new VideoUrlDTO
            {
                url = v.Url
            }).ToList(),
            BlogImageDTOs = blog.Blogimages.Select(b => new BlogImageDTO { 
                ImageData = b.ImageData,
                ContentType = b.ContentType,
             }).ToList(),
            FullDesc = blog.FullDesc
        };
    }

    public async Task<IEnumerable<BlogDTO>> GetBlogDataAsync()
    {
        /*
            select * from blog bl
            left join video_url v on v.blog_id = bl.id
            left join blogimages bi on bi.blog_id = bl.id;
        
        */
        var blogs = await portfolioDbContext.Blogs
        .Include(b => b.VideoUrls)
        .Include(b => b.Blogimages)
        .Select(b =>
            new BlogDTO
            {
                Id = b.Id,
                Title = b.Title,
                Date = b.Date,
                PreviewDesc = b.PreviewDesc,
                RouteName = b.RouteName,
                FullDesc = b.FullDesc,
                BlogImageDTOs = b.Blogimages.Select(bi => new BlogImageDTO {
                    ImageData = bi.ImageData,
                    ContentType = bi.ContentType,
                }).ToList(),
                VideoUrls = b.VideoUrls.Select(v => new VideoUrlDTO
                {
                    url = v.Url
                }).ToList(),
            }).ToListAsync();

        return blogs;
    }

    public async Task<IEnumerable<BlogDTO>> SortAsync(bool isNewest)
    {
        var blogs = await portfolioDbContext.Blogs.Include(b => b.VideoUrls).Select(b =>
            new BlogDTO
            {
                Title = b.Title,
                Date = b.Date,
                PreviewDesc = b.PreviewDesc,
                RouteName = b.RouteName,
                FullDesc = b.FullDesc,
                VideoUrls = b.VideoUrls.Select(v => new VideoUrlDTO
                {
                    url = v.Url
                }).ToList(),
            }).ToListAsync();

        return isNewest ?
        blogs.OrderByDescending(b => b.Date) :
        blogs.OrderBy(b => b.Date);
    }
}


