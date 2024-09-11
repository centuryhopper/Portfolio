
using Microsoft.EntityFrameworkCore;
using Server.Contexts;
using Server.Entities;
using Shared.Models;
using static Shared.Models.ServiceResponses;

namespace DataAccess.Repositories;

public class ContactsDataRepository : IContactsDataRepository<ContactMeDTO>
{
    private readonly PortfolioDBContext portfolioDBContext;

    public ContactsDataRepository(PortfolioDBContext portfolioDBContext)
    {
        this.portfolioDBContext = portfolioDBContext;
    }

    public async Task<IEnumerable<ContactMeDTO>> GetContactsAsync()
    {
        var contacts = await portfolioDBContext.Contacttables.AsNoTracking().ToListAsync();
        return contacts.Select(c => new ContactMeDTO
        {
            Name = c.Name,
            Message = c.Message,
            Email = c.Email,
            Subject = c.Subject,
        });
    }

    public async Task<GeneralResponse> PostDataAsync(ContactMeDTO model)
    {
        var contactTable = new Contacttable
        {
            Name = model.Name!,
            Email = model.Email!,
            Subject = model.Subject!,
            Message = model.Message!
        };

        try
        {
            await portfolioDBContext.Contacttables.AddAsync(contactTable);
            await portfolioDBContext.SaveChangesAsync();
            return new GeneralResponse(Flag: true, Message: "Thank you! Your message has been sent.");
        }
        catch (Exception ex)
        {
            return new GeneralResponse(Flag: false, Message: ex.Message);
        }
    }

}


