
using static Shared.Models.ServiceResponses;

namespace Client.Services;

public interface IContactsService<T>
{
    Task<GeneralResponse> PostDataAsync(T model);
    // Task<IEnumerable<T>> GetContactsAsync();
}