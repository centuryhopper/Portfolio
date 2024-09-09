
using Microsoft.EntityFrameworkCore;
using Server.Contexts;
using Server.Entities;
using Shared.Models;
using static Shared.Models.ServiceResponses;

namespace DataAccess.Repositories;

public class SkillsDataRepository : ISkillsDataRepository<SkillDTO>
{
    private readonly PortfolioDBContext neondbContext;

    public SkillsDataRepository(PortfolioDBContext neondbContext)
    {
        this.neondbContext = neondbContext;
    }

    public async Task<GeneralResponse> AddSkillsAsync(SkillDTO model)
    {
        var SkillDTO = new Skill
        {
            Title = model.Title,
        };

        try
        {
            await neondbContext.Skills.AddAsync(SkillDTO);
            await neondbContext.SaveChangesAsync();

            int mostRecentInsertedSkillDTO = await neondbContext.Skills.MaxAsync(skill => skill.Id);

            var skillDescriptions = model.Descriptions.Select(desc => new SkillDescription
            {
                Id = mostRecentInsertedSkillDTO,
                Description = desc,
            });

            await neondbContext.SkillDescriptions.AddRangeAsync(skillDescriptions);
            await neondbContext.SaveChangesAsync();

            return new GeneralResponse(Flag: true, Message: "Added skills and models");
        }
        catch (Exception _)
        {
            return new GeneralResponse(Flag: false, Message: "Error adding skills and/or skill decriptions");
        }
    }

    public async Task<IEnumerable<SkillDTO>> GetDataAsync()
    {
        var skills = await neondbContext.Skills.Include(skill => skill.SkillDescriptions).Select(
            skill =>
            new SkillDTO
            {
                Title = skill.Title,
                Descriptions = skill.SkillDescriptions.Select(sd => sd.Description)
            }).ToListAsync();

        return skills;
    }
}


