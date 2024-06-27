
using Microsoft.AspNetCore.Mvc;
using Portfolio.Entities;
using Portfolio.Repositories;


namespace Portfolio.Controllers;


public class ContactsController : Controller
{
    private readonly IContactsDataRepository<ContactMeModel> contactRepo;
    private readonly ILogger<ContactsController> logger;

    public ContactsController(IContactsDataRepository<ContactMeModel> contactRepo, ILogger<ContactsController> Logger)
    {
        this.contactRepo = contactRepo;
        logger = Logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    [Route("get-contacts")]
    public async Task<IActionResult> GetAsync()
    {
        var contacts = await contactRepo.GetContactsAsync();
        return Ok(contacts);
    }

    [HttpPost]
    public async Task<IActionResult> Index(ContactMeModel model)
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

        if (!data.flag)
        {
            return BadRequest(data);
        }

        return Ok(data);
    }
}