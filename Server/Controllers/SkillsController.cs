using Microsoft.AspNetCore.Mvc;
using DataAccess.Repositories;

using Shared.Models;


namespace Server.Controllers;


[Route("api/[controller]")]
[ApiController]
public class SkillsController : ControllerBase
{
    private readonly ISkillsDataRepository<SkillDTO> SkillRepo;

    public SkillsController(ISkillsDataRepository<SkillDTO> SkillRepo)
    {
        this.SkillRepo = SkillRepo;
    }

    [HttpGet]
    [Route("get-skills")]
    public async Task<IActionResult> GetAsync()
    {
        var data = await SkillRepo.GetDataAsync();

        return Ok(data);
    }

    [HttpPost]
    [Route("post-skill")]
    public async Task<IActionResult> AddAsync([FromBody] SkillDTO model)
    {
        var data = await SkillRepo.AddSkillsAsync(model);

        if (!data.Flag)
        {
            return BadRequest(data);
        }

        return Ok(data);
    }
}