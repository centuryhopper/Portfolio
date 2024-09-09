using Microsoft.AspNetCore.Mvc;
using DataAccess.Repositories;
using Shared.Models;



namespace Portfolio.Presentation.Server.Controllers;


[Route("api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly IProjectsDataRepository<ProjectCardDTO> ProjectRepo;

    public ProjectsController(IProjectsDataRepository<ProjectCardDTO> ProjectRepo)
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
    public async Task<IActionResult> AddAsync([FromBody] ProjectCardDTO model)
    {
        var data = await ProjectRepo.AddProjectAsync(model);

        if (!data.Flag)
        {
            return BadRequest(data);
        }

        return Ok(data);
    }
}