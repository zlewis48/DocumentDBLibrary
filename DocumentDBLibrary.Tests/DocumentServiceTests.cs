using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

public class DocumentServiceTests
{
    private readonly Mock<ITextSearchRepository<SampleDocument>> _mockRepository;
    private readonly DocumentService<SampleDocument> _documentService;

    public DocumentServiceTests()
    {
        _mockRepository = new Mock<ITextSearchRepository<SampleDocument>>();
        _documentService = new DocumentService<SampleDocument>(_mockRepository.Object);
    }

    [Fact]
    public async Task GetDocumentByIdAsync_ShouldReturnDocument_WhenDocumentExists()
    {
        var document = new SampleDocument { Id = "1", Name = "Test Document" };
        _mockRepository.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(document);

        var result = await _documentService.GetDocumentByIdAsync("1");

        Assert.NotNull(result);
        Assert.Equal("1", result.Id);
        Assert.Equal("Test Document", result.Name);
    }

    [Fact]
    public async Task SearchDocumentsAsync_ShouldReturnDocuments_WhenMatchingDocumentsExist()
    {
        var documents = new List<SampleDocument>
        {
            new SampleDocument { Id = "1", Name = "Document 1" },
            new SampleDocument { Id = "2", Name = "Document 2" }
        };
        _mockRepository.Setup(r => r.SearchAsync("Document")).ReturnsAsync(documents);

        var result = await _documentService.SearchDocumentsAsync("Document");

        Assert.NotNull(result);
        Assert.Equal(2, ((List<SampleDocument>)result).Count);
    }

    [Fact]
    public async Task CreateDocumentAsync_ShouldCallAddAsync()
    {
        var document = new SampleDocument { Id = "1", Name = "New Document" };

        await _documentService.CreateDocumentAsync(document);

        _mockRepository.Verify(r => r.AddAsync(document), Times.Once);
    }
}
