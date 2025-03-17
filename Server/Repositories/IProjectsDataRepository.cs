

using static Shared.Models.ServiceResponses;

namespace DataAccess.Repositories;


public interface IProjectsDataRepository<T>
{
    Task<IEnumerable<T>> GetDataAsync(string? searchTerm);
    Task<GeneralResponse> EditProjectAsync(T model);
    Task<GeneralResponse> DeleteProjectAsync(int id);
    Task<GeneralResponse> AddProjectAsync(T model);
}