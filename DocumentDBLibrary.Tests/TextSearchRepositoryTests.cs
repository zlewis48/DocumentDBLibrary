using Moq;
using MongoDB.Driver;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

public class TextSearchRepositoryTests
{
    private readonly Mock<IMongoCollection<SampleDocument>> _mockCollection;
    private readonly Mock<IMongoDatabase> _mockDatabase;
    private readonly TextSearchRepository<SampleDocument> _textSearchRepository;

    public TextSearchRepositoryTests()
    {
        _mockCollection = new Mock<IMongoCollection<SampleDocument>>();
        _mockDatabase = new Mock<IMongoDatabase>();

        _mockDatabase
            .Setup(db => db.GetCollection<SampleDocument>(It.IsAny<string>(), null))
            .Returns(_mockCollection.Object);

        _textSearchRepository = new TextSearchRepository<SampleDocument>(_mockDatabase.Object, "SampleCollection");
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnDocuments_WhenMatchingDocumentsExist()
    {
        var documents = new List<SampleDocument>
        {
            new SampleDocument { Id = "1", Name = "Document 1" },
            new SampleDocument { Id = "2", Name = "Document 2" }
        };
        var mockCursor = new Mock<IAsyncCursor<SampleDocument>>();

        mockCursor.Setup(_ => _.Current).Returns(documents);
        mockCursor
            .SetupSequence(_ => _.MoveNext(It.IsAny<System.Threading.CancellationToken>()))
            .Returns(true)
            .Returns(false);

        _mockCollection
            .Setup(c => c.FindAsync(It.IsAny<FilterDefinition<SampleDocument>>(), null, default))
            .ReturnsAsync(mockCursor.Object);

        var result = await _textSearchRepository.SearchAsync("Document");

        Assert.NotNull(result);
        Assert.Equal(2, ((List<SampleDocument>)result).Count);
    }

    [Fact]
    public async Task CreateTextIndexAsync_ShouldCallCreateOneAsync()
    {
        await _textSearchRepository.CreateTextIndexAsync("Name");

        _mockCollection.Verify(c => c.Indexes.CreateOneAsync(It.IsAny<CreateIndexModel<SampleDocument>>(), null, default), Times.Once);
    }
}
