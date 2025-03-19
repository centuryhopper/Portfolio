using Microsoft.AspNetCore.Mvc;
using DataAccess.Repositories;
using Shared.Models;



namespace Server.Controllers;


[Route("api/[controller]")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly IProjectsDataRepository<ProjectCardDTO> ProjectRepo;

    public ProjectsController(IProjectsDataRepository<ProjectCardDTO> ProjectRepo)
    {
        this.ProjectRepo = ProjectRepo;
    }

    [HttpGet("get-projects")]
    public async Task<IActionResult> GetDataAsync([FromQuery] string? searchTerm)
    {
        var data = await ProjectRepo.GetDataAsync(searchTerm);
        return Ok(data);
    }

    [HttpGet("get-project/{projectId:int}")]
    public async Task<IActionResult> GetProjectByIdAsync(int projectId)
    {
        var data = await ProjectRepo.GetProjectByIdAsync(projectId);
        return Ok(data);
    }

    [HttpDelete("delete-project/{id:int}")]
    public async Task<IActionResult> DeleteProjectAsync(int id)
    {
        var data = await ProjectRepo.DeleteProjectAsync(id);
        return Ok(data);
    }

    [HttpPatch("edit-project")]
    public async Task<IActionResult> EditProjectAsync([FromBody] ProjectCardDTO model)
    {
        var data = await ProjectRepo.EditProjectAsync(model);
        return Ok(data);
    }

    [HttpPost("add-project")]
    public async Task<IActionResult> AddProjectAsync([FromBody] ProjectCardDTO model)
    {
        var data = await ProjectRepo.AddProjectAsync(model);
        return Ok(data);
    }
}