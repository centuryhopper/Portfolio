
using static Shared.Models.ServiceResponses;

namespace DataAccess.Repositories;

public interface ISkillsDataRepository<T>
{
    Task<IEnumerable<T>> GetDataAsync();
    Task<GeneralResponse> AddSkillsAsync(T model);
}