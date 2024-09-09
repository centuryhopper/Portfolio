
using Microsoft.EntityFrameworkCore;
using Server.Contexts;
using Server.Entities;
using Shared.Models;
using static Shared.Models.ServiceResponses;

namespace DataAccess.Repositories;

public class ContactsDataRepository : IContactsDataRepository<ContactMeDTO>
{
    private readonly PortfolioDBContext neondbContext;

    public ContactsDataRepository(PortfolioDBContext neondbContext)
    {
        this.neondbContext = neondbContext;
    }

    public async Task<IEnumerable<ContactMeDTO>> GetContactsAsync()
    {
        var contacts = await neondbContext.Contacttables.AsNoTracking().ToListAsync();
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
            Name = model.Name!
                ,
            Email = model.Email!
                ,
            Subject = model.Subject!
                ,
            Message = model.Message!
        };

        try
        {
            await neondbContext.Contacttables.AddAsync(contactTable);
            await neondbContext.SaveChangesAsync();
            return new GeneralResponse(Flag: true, Message: "Added user's message");
        }
        catch (Exception _)
        {
            return new GeneralResponse(Flag: false, Message: "Error adding user's message");
        }
    }

}


