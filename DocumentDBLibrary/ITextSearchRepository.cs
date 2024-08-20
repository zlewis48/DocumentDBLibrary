using System.Collections.Generic;
using System.Threading.Tasks;

public interface ITextSearchRepository<T> : IRepository<T>
{
    Task<IEnumerable<T>> SearchAsync(string searchText);
    Task CreateTextIndexAsync(params string[] fields);
}
