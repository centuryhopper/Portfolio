using Microsoft.AspNetCore.Mvc;
using Portfolio.Entities;
using Portfolio.Repositories;



namespace Portfolio.Presentation.Server.Controllers;


[Route("api/[controller]")]
[ApiController]
public class SkillsController : ControllerBase
{
    private readonly ISkillsDataRepository<SkillModel> SkillRepo;

    public SkillsController(ISkillsDataRepository<SkillModel> SkillRepo)
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
    public async Task<IActionResult> AddAsync([FromBody] SkillModel model)
    {
        var data = await SkillRepo.AddSkillsAsync(model);

        if (!data.flag)
        {
            return BadRequest(data);
        }

        return Ok(data);
    }
}