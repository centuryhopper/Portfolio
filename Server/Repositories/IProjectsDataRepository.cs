

using static Shared.Models.ServiceResponses;

namespace DataAccess.Repositories;


public interface IProjectsDataRepository<T>
{
    Task<IEnumerable<T>> GetDataAsync(string? searchTerm);
    Task<GeneralResponse> AddProjectAsync(T model);
}