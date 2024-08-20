public class DocumentService<T>
{
    private readonly ITextSearchRepository<T> _repository;

    public DocumentService(ITextSearchRepository<T> repository)
    {
        _repository = repository;
    }

    public async Task<T> GetDocumentByIdAsync(string id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<T>> SearchDocumentsAsync(string searchText)
    {
        return await _repository.SearchAsync(searchText);
    }

    public async Task CreateDocumentAsync(T document)
    {
        await _repository.AddAsync(document);
    }

    public async Task UpdateDocumentAsync(T document)
    {
        await _repository.UpdateAsync(document);
    }

    public async Task DeleteDocumentAsync(string id)
    {
        await _repository.DeleteAsync(id);
    }
}
