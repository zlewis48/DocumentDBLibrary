using Moq;
using MongoDB.Driver;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

public class RepositoryTests
{
    private readonly Mock<IMongoCollection<SampleDocument>> _mockCollection;
    private readonly Mock<IMongoDatabase> _mockDatabase;
    private readonly Repository<SampleDocument> _repository;

    public RepositoryTests()
    {
        _mockCollection = new Mock<IMongoCollection<SampleDocument>>();
        _mockDatabase = new Mock<IMongoDatabase>();

        _mockDatabase
            .Setup(db => db.GetCollection<SampleDocument>(It.IsAny<string>(), It.IsAny<MongoCollectionSettings>()))
            .Returns(_mockCollection.Object);

        _repository = new Repository<SampleDocument>(_mockDatabase.Object, "SampleCollection");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnDocument_WhenDocumentExists()
    {
        // Arrange
        var document = new SampleDocument { Id = "1", Name = "Test Document" };
        var documents = new List<SampleDocument> { document };
        var mockFindFluent = new Mock<IFindFluent<SampleDocument, SampleDocument>>();

        mockFindFluent
            .Setup(f => f.FirstOrDefaultAsync(default))
            .ReturnsAsync(document);

        _mockCollection
            .Setup(c => c.Find(It.IsAny<FilterDefinition<SampleDocument>>(), It.IsAny<FindOptions<SampleDocument>>()))
            .Returns(mockFindFluent.Object);

        // Act
        var result = await _repository.GetByIdAsync("1");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("1", result.Id);
        Assert.Equal("Test Document", result.Name);
    }

    [Fact]
    public async Task AddAsync_ShouldCallInsertOneAsync()
    {
        // Arrange
        var document = new SampleDocument { Id = "1", Name = "New Document" };

        // Act
        await _repository.AddAsync(document);

        // Assert
        _mockCollection.Verify(c => c.InsertOneAsync(document, null, default), Times.Once);
    }
}
