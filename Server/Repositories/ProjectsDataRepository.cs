
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
        var projectCard = new ProjectCard
        {
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
            return new GeneralResponse(Flag: true, Message: "Added user's message");
        }
        catch (Exception _)
        {
            return new GeneralResponse(Flag: false, Message: "Error adding user's message");
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
            ImgUrl = c.Imgurl,
            Title = c.Title,
            Description = c.Description,
            ProjectLink = c.Projectlink,
            SourceCodeLink = c.Sourcecodelink,
        });
    }

}


