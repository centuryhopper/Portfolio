using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Shared.Models;
using System.Net.Http.Json;
using static Shared.Models.ServiceResponses;

namespace Client.Services;

public class ContactsService : IContactsService<ContactMeDTO>
{
    private readonly HttpClient client;

    public ContactsService(IHttpClientFactory httpClientFactory)
    {
        this.client = httpClientFactory.CreateClient("ServerAPI");
    }

    public async Task<GeneralResponse> PostDataAsync(ContactMeDTO model)
    {
        System.Console.WriteLine($"creating contact user: {model}");
        var response = await client.PostAsJsonAsync("/api/Contacts/post-contact", model);

        if (response.IsSuccessStatusCode)
        {
            // Console.WriteLine("successfully inserted contact into db");
            var result = await response.Content.ReadFromJsonAsync<GeneralResponse>();
            return result;
        }

        return new GeneralResponse(false, "error posting user message from browser");
    }

}

