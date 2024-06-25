using Microsoft.AspNetCore.Mvc;
using Portfolio.Entities;
using Portfolio.Repositories;



namespace Portfolio.Presentation.Server.Controllers;


[Route("api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly IProjectsDataRepository<ProjectCardModel> ProjectRepo;

    public ProjectsController(IProjectsDataRepository<ProjectCardModel> ProjectRepo)
    {
        this.ProjectRepo = ProjectRepo;
    }

    [HttpGet]
    [Route("get-projects")]
    public async Task<IActionResult> GetAsync()
    {
        var data = await ProjectRepo.GetDataAsync("");

        return Ok(data);
    }

    [HttpPost]
    [Route("post-project")]
    public async Task<IActionResult> AddAsync([FromBody] ProjectCardModel model)
    {
        var data = await ProjectRepo.AddProjectAsync(model);

        if (!data.flag)
        {
            return BadRequest(data);
        }

        return Ok(data);
    }
}