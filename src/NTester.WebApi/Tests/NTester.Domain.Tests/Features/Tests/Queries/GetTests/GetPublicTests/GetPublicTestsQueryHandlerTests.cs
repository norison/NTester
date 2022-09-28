using AutoFixture.NUnit3;
using AutoMapper;
using FluentAssertions;
using MockQueryable.NSubstitute;
using NSubstitute;
using NTester.DataAccess.Data.NTesterDbContext;
using NTester.DataAccess.Entities;
using NTester.Domain.Features.Tests.Queries.GetTests.GetPublicTests;
using NTester.Domain.Tests.Common;
using NUnit.Framework;

namespace NTester.Domain.Tests.Features.Tests.Queries.GetTests.GetPublicTests;

[TestFixture]
public class GetPublicTestsQueryHandlerTests
{
    private INTesterDbContext _dbContext;
    private IMapper _mapper;
    private GetPublicTestsQueryHandler _handler;
    
    [SetUp]
    public void SetUp()
    {
        _dbContext = Substitute.For<INTesterDbContext>();
        _mapper = MapperFactory.CreateMapper();

        _handler = new GetPublicTestsQueryHandler(_dbContext, _mapper);
    }
    
    [Test]
    [InlineAutoData(1, 1, 1)]
    [InlineAutoData(1, 2, 2)]
    [InlineAutoData(2, 2, 1)]
    public async Task Handle_Pagination_ShouldReturnCorrectPublicTests(int pageNumber, int pageSize, int expectedCount, Guid userId)
    {
        // Arrange
        var query = new GetPublicTestsQuery()
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            UserId = userId,
            Title = "ti"
        };
    
        var testsDbSet = GetDataForTestPublishedTrue(userId);
        _dbContext.Tests.Returns(testsDbSet);
    
        // Act
        var result = await _handler.Handle(query, CancellationToken.None);
    
        // Assert
        result.Tests.Count().Should().Be(expectedCount);
        result.Tests.Should().OnlyContain(x => x.Published && x.UserId != userId && x.Title.Contains("ti"));
    }
    
    [Test]
    public async Task Handle_EmptyUserId_ShouldReturnCorrectPublicTests()
    {
        // Arrange
        var query = new GetPublicTestsQuery()
        {
            PageNumber = 1,
            PageSize = 5,
            UserId = Guid.Empty,
            Title = "ti"
        };
    
        var testsDbSet = GetDataForTestPublishedTrue(Guid.NewGuid());
        _dbContext.Tests.Returns(testsDbSet);
    
        // Act
        var result = await _handler.Handle(query, CancellationToken.None);
    
        // Assert
        result.Tests.Count().Should().Be(4);
        result.Tests.Should().OnlyContain(x => x.Published && x.Title.Contains("ti"));
    }
    
    private static IEnumerable<TestEntity> GetDataForTestPublishedTrue(Guid userId)
    {
        return new List<TestEntity>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Title = "title1",
                UserId = Guid.NewGuid(),
                Published = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "title2",
                UserId = Guid.NewGuid(),
                Published = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "title3",
                UserId = Guid.NewGuid(),
                Published = false
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "title4",
                UserId = Guid.NewGuid(),
                Published = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "title5",
                UserId = userId,
                Published = true
            }
        }.AsQueryable().BuildMockDbSet();
    }
}