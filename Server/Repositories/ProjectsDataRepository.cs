
using Microsoft.EntityFrameworkCore;
using Server.Contexts;
using Server.Entities;
using Shared.Models;
using static Shared.Models.ServiceResponses;

namespace DataAccess.Repositories;

public class ProjectsDataRepository : IProjectsDataRepository<ProjectCardDTO>
{
    private readonly PortfolioDBContext portfolioDBContext;

    public ProjectsDataRepository(PortfolioDBContext portfolioDBContext)
    {
        this.portfolioDBContext = portfolioDBContext;
    }

    public async Task<GeneralResponse> AddProjectAsync(ProjectCardDTO model)
    {
        var id = await portfolioDBContext.ProjectCards.MaxAsync(x=>x.Id);
        var projectCard = new ProjectCard
        {
            Id = id + 1,
            Imgurl = model.ImgUrl,
            Title = model.Title,
            Description = model.Description,
            Projectlink = model.ProjectLink,
            Sourcecodelink = model.SourceCodeLink
        };

        try
        {
            await portfolioDBContext.ProjectCards.AddAsync(projectCard);
            await portfolioDBContext.SaveChangesAsync();
            return new GeneralResponse(Flag: true, Message: "Added project card!");
        }
        catch (Exception ex)
        {
            return new GeneralResponse(Flag: false, Message: ex.Message);
        }
    }

    public async Task<GeneralResponse> DeleteProjectAsync(int id)
    {
        try
        {
            var project = await portfolioDBContext.ProjectCards.FindAsync(id);
            if (project is null)
            {
                throw new Exception("project record couldn't be found");
            }
            portfolioDBContext.ProjectCards.Remove(project);
            await portfolioDBContext.SaveChangesAsync();
            return new GeneralResponse(Flag: true, Message: "Deleted project card!");
        }
        catch (Exception ex)
        {
            return new GeneralResponse(Flag: false, Message: ex.Message);
        }
    }

    public async Task<GeneralResponse> EditProjectAsync(ProjectCardDTO model)
    {
        try
        {
            var project = await portfolioDBContext.ProjectCards.FindAsync(model.Id);
            if (project is null)
            {
                throw new Exception("project record couldn't be found");
            }
            project.Imgurl = model.ImgUrl;
            project.Title = model.Title;
            project.Description = model.Description;
            project.Projectlink = model.ProjectLink;
            project.Sourcecodelink = model.SourceCodeLink;
            await portfolioDBContext.SaveChangesAsync();
            return new GeneralResponse(Flag: true, Message: "Updated project card!");
        }
        catch (Exception ex)
        {
            return new GeneralResponse(Flag: false, Message: ex.Message);
        }
    }

    public async Task<IEnumerable<ProjectCardDTO>> GetDataAsync(string? searchTerm)
    {
        List<ProjectCard> projectCards = new();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            projectCards = await portfolioDBContext.ProjectCards.Where(p => p.Title == searchTerm).AsNoTracking().ToListAsync();
        }
        else
        {
            projectCards = await portfolioDBContext.ProjectCards.AsNoTracking().ToListAsync();
        }

        return projectCards.Select(c => new ProjectCardDTO
        {
            Id = c.Id,
            ImgUrl = c.Imgurl,
            Title = c.Title,
            Description = c.Description,
            ProjectLink = c.Projectlink,
            SourceCodeLink = c.Sourcecodelink,
        });
    }

}


