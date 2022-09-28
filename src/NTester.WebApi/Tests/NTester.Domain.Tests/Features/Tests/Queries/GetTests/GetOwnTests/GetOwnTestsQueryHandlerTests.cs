using AutoFixture.NUnit3;
using AutoMapper;
using FluentAssertions;
using MockQueryable.NSubstitute;
using NSubstitute;
using NTester.DataAccess.Data.NTesterDbContext;
using NTester.DataAccess.Entities;
using NTester.Domain.Features.Tests.Queries.GetTests.GetOwnTests;
using NTester.Domain.Tests.Common;
using NUnit.Framework;

namespace NTester.Domain.Tests.Features.Tests.Queries.GetTests.GetOwnTests;

[TestFixture]
public class GetOwnTestsQueryHandlerTests
{
    private INTesterDbContext _dbContext;
    private IMapper _mapper;
    private GetOwnTestsQueryHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _dbContext = Substitute.For<INTesterDbContext>();
        _mapper = MapperFactory.CreateMapper();

        _handler = new GetOwnTestsQueryHandler(_dbContext, _mapper);
    }

    [Test]
    [InlineAutoData(1, 1, 1)]
    [InlineAutoData(1, 2, 2)]
    [InlineAutoData(2, 2, 1)]
    public async Task Handle_Pagination_ShouldReturnCorrectOwnTests(int pageNumber, int pageSize, int expectedCount, Guid userId)
    {
        // Arrange
        var query = new GetOwnTestsQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            UserId = userId,
            Title = "ti",
            Published = true
        };
    
        var testsDbSet = GetDataForTestPublishedTrue(userId);
        _dbContext.Tests.Returns(testsDbSet);
    
        // Act
        var result = await _handler.Handle(query, CancellationToken.None);
    
        // Assert
        result.Tests.Count().Should().Be(expectedCount);
        result.Tests.Should().OnlyContain(x => x.Published && x.UserId == userId && x.Title.Contains("ti"));
    }

    private static IEnumerable<TestEntity> GetDataForTestPublishedTrue(Guid userId)
    {
        return new List<TestEntity>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Title = "title1",
                UserId = userId,
                Published = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "title2",
                UserId = userId,
                Published = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "title3",
                UserId = userId,
                Published = true
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
                Published = false
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "another",
                UserId = userId,
                Published = true
            }
        }.AsQueryable().BuildMockDbSet();
    }
}