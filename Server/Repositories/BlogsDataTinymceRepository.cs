
using Microsoft.EntityFrameworkCore;
using Server.Contexts;
using Server.Entities;
using Server.Utils;
using Shared.Models;
using static Shared.Models.ServiceResponses;

namespace Server.Repositories;

public class BlogsDataTinymceRepository : IBlogsDataRepository<BlogDTO>
{
    private readonly PortfolioDBContext portfolioDbContext;

    public BlogsDataTinymceRepository(PortfolioDBContext portfolioDbContext)
    {
        this.portfolioDbContext = portfolioDbContext;
    }

    public async Task<GeneralResponse> AddBlogAsync(BlogDTO model)
    {
        var blog = new Blog
        {
            Title = model.Title,
            Date = model.Date.ToDateTime(TimeOnly.MinValue),
            PreviewDesc = model.PreviewDesc,
            FullDesc = model.FullDesc,
        };

        // since we're not using blog image and and video url tables anymore then we dont need transaction here
        // but i left it anyway just to know how to use one next time
        var transaction = await portfolioDbContext.Database.BeginTransactionAsync();

        try
        {
            await portfolioDbContext.Blogs.AddAsync(blog);
            await portfolioDbContext.SaveChangesAsync();

            await transaction.CommitAsync();
            return new GeneralResponse(Flag: true, Message: "Added my blogs");
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
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

    public async Task<GeneralResponse> EditBlogAsync(BlogDTO model)
    {
        try
        {
            var blog = await portfolioDbContext.Blogs.FindAsync(model.Id);
            if (blog is null)
            {
                throw new Exception("blog not found in db");
            }

            blog.Title = model.Title;
            blog.Date = model.Date.ToDateTime(TimeOnly.MinValue);
            blog.FullDesc = model.FullDesc;
            blog.PreviewDesc = model.PreviewDesc;

            await portfolioDbContext.SaveChangesAsync();

            return new GeneralResponse(Flag: true, Message: "Blog updated!");
        }
        catch (System.Exception ex)
        {
            return new GeneralResponse(Flag: false, Message: ex.Message);
        }
    }

    public async Task<BlogDTO?> GetBlogById(int blogId)
    {
        try
        {
            var blog = await portfolioDbContext.Blogs.FindAsync(blogId);
            return new BlogDTO
            {
                Title = blog!.Title,
                Date = DateOnly.FromDateTime(blog.Date),
                PreviewDesc = blog.PreviewDesc,
                FullDesc = blog.FullDesc,
                Id = blog.Id,
            };
        }
        catch (System.Exception ex)
        {
            return null;
        }
    }

    public async Task<BlogDTO> GetBlogByTitleAsync(string title)
    {
        var blog = await portfolioDbContext.Blogs
        .Where(b => b.Title == title).FirstAsync();

        return new BlogDTO
        {
            Title = blog.Title,
            Date = DateOnly.FromDateTime(blog.Date),
            PreviewDesc = blog.PreviewDesc,
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
        .Select(b =>
            new BlogDTO
            {
                Id = b.Id,
                Title = b.Title,
                Date = DateOnly.FromDateTime(b.Date),
                PreviewDesc = b.PreviewDesc,
                FullDesc = b.FullDesc,
            }).ToListAsync();

        return blogs;
    }

    public async Task<IEnumerable<BlogDTO>> SortAsync(bool isNewest)
    {
        var blogs = await portfolioDbContext.Blogs.Select(b =>
            new BlogDTO
            {
                Title = b.Title,
                Date = DateOnly.FromDateTime(b.Date),
                PreviewDesc = b.PreviewDesc,
                FullDesc = b.FullDesc,
            }).ToListAsync();

        return isNewest ?
        blogs.OrderByDescending(b => b.Date) :
        blogs.OrderBy(b => b.Date);
    }
}


