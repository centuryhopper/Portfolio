
using Microsoft.AspNetCore.Mvc;
using DataAccess.Repositories;
using Shared.Models;


namespace Portfolio.Presentation.Server.Controllers;


[Route("api/[controller]")]
[ApiController]
public class ContactsController : ControllerBase
{
    private readonly IContactsDataRepository<ContactMeDTO> contactRepo;
    private readonly ILogger<ContactsController> logger;

    public ContactsController(IContactsDataRepository<ContactMeDTO> contactRepo, ILogger<ContactsController> Logger)
    {
        this.contactRepo = contactRepo;
        logger = Logger;
    }

    [HttpGet]
    [Route("get-contacts")]
    public async Task<IActionResult> GetAsync()
    {
        var contacts = await contactRepo.GetContactsAsync();
        return Ok(contacts);
    }

    [HttpPost]
    [Route("post-contact")]
    public async Task<IActionResult> PostAsync([FromBody] ContactMeDTO model)
    {
        /*
        {
        "name": "scooby doo",
        "email": "scooby_doo@mystery_inc.org",
        "subject": "scooby snacks",
        "message": "ree hee hee rooby shnacks!"
        }
        */
        var data = await contactRepo.PostDataAsync(model);

        if (!data.Flag)
        {
            return BadRequest(data);
        }

        return Ok(data);
    }
}