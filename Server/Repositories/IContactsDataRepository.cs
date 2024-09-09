using static Shared.Models.ServiceResponses;

namespace DataAccess.Repositories;

public interface IContactsDataRepository<T>
{
    Task<GeneralResponse> PostDataAsync(T model);
    Task<IEnumerable<T>> GetContactsAsync();
}