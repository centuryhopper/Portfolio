
using Microsoft.EntityFrameworkCore;
using Server.Contexts;
using Server.Entities;
using Shared.Models;
using static Shared.Models.ServiceResponses;

namespace DataAccess.Repositories;

public class SkillsDataRepository : ISkillsDataRepository<SkillDTO>
{
    private readonly PortfolioDBContext portfolioDBContext;

    public SkillsDataRepository(PortfolioDBContext portfolioDBContext)
    {
        this.portfolioDBContext = portfolioDBContext;
    }

    public async Task<GeneralResponse> AddSkillsAsync(SkillDTO model)
    {
        var SkillDTO = new Skill
        {
            Title = model.Title,
        };

        try
        {
            await portfolioDBContext.Skills.AddAsync(SkillDTO);
            await portfolioDBContext.SaveChangesAsync();

            int mostRecentInsertedSkillDTO = await portfolioDBContext.Skills.MaxAsync(skill => skill.Id);

            var skillDescriptions = model.Descriptions.Select(desc => new SkillDescription
            {
                Id = mostRecentInsertedSkillDTO,
                Description = desc,
            });

            await portfolioDBContext.SkillDescriptions.AddRangeAsync(skillDescriptions);
            await portfolioDBContext.SaveChangesAsync();

            return new GeneralResponse(Flag: true, Message: "Added skills and models");
        }
        catch (Exception ex)
        {
            return new GeneralResponse(Flag: false, Message: ex.Message);
        }
    }

    public async Task<IEnumerable<SkillDTO>> GetDataAsync()
    {
        var skills = await portfolioDBContext.Skills.Include(skill => skill.SkillDescriptions).Select(
            skill =>
            new SkillDTO
            {
                Title = skill.Title,
                Descriptions = skill.SkillDescriptions.Select(sd => sd.Description)
            }).ToListAsync();

        return skills;
    }
}


