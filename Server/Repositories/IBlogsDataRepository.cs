
using static Shared.Models.ServiceResponses;

namespace Server.Repositories;


public interface IBlogsDataRepository<T>
{
    Task<IEnumerable<T>> SortAsync(bool isNewest);
    Task<IEnumerable<T>> GetBlogDataAsync();
    Task<T> GetBlogByTitleAsync(string title);
    Task<GeneralResponse> AddBlogAsync(T model);
}