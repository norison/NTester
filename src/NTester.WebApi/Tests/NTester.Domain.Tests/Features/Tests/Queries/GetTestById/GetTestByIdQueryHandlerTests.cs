using AutoMapper;
using FluentAssertions;
using MockQueryable.NSubstitute;
using NSubstitute;
using NTester.DataAccess.Data.NTesterDbContext;
using NTester.DataAccess.Entities;
using NTester.Domain.Exceptions.Tests;
using NTester.Domain.Features.Tests.Queries.GetTestById;
using NTester.Domain.Tests.Common;
using NUnit.Framework;

namespace NTester.Domain.Tests.Features.Tests.Queries.GetTestById;

[TestFixture]
public class GetTestByIdQueryHandlerTests
{
    private INTesterDbContext _dbContext;
    private IMapper _mapper;
    private GetTestByIdQueryHandler _handler;
    
    private static readonly Guid TestOneId = Guid.NewGuid();
    private static readonly Guid TestTwoId = Guid.NewGuid();
    private static readonly Guid UserId = Guid.NewGuid();

    [SetUp]
    public void SetUp()
    {
        _dbContext = Substitute.For<INTesterDbContext>();
        _mapper = MapperFactory.CreateMapper();

        _handler = new GetTestByIdQueryHandler(_dbContext, _mapper);
    }
    
    [Test, TestCaseSource(nameof(TestNotFoundTestData))]
    public async Task Handle_TestDoesNotExistOrNoPermissions_ShouldThrowNotFoundException(Guid testId, Guid userId)
    {
        // Arrange
        var query = new GetTestByIdQuery
        {
            UserId = userId,
            Id = testId
        };
        
        var testsDbSet = GetTestData();
        _dbContext.Tests.ReturnsForAnyArgs(testsDbSet);

        // Act/Assert
        await _handler
            .Invoking(x => x.Handle(query, CancellationToken.None))
            .Should()
            .ThrowAsync<TestNotFoundException>()
            .Where(x => x.Message.Contains(testId.ToString()));
    }

    [Test, TestCaseSource(nameof(TestFoundTestData))]
    public async Task Handle_TestExists_TestShouldBeFound(Guid testId, Guid userId)
    {
        // Arrange
        var query = new GetTestByIdQuery
        {
            UserId = userId,
            Id = testId
        };
        
        var testsDbSet = GetTestData();
        _dbContext.Tests.ReturnsForAnyArgs(testsDbSet);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Id.Should().Be(testId);
    }

    private static IEnumerable<TestCaseData> TestNotFoundTestData => new List<TestCaseData>
    {
        new(TestOneId, Guid.NewGuid()),
        new(Guid.NewGuid(), UserId)
    };
    
    private static IEnumerable<TestCaseData> TestFoundTestData => new List<TestCaseData>
    {
        new(TestOneId, UserId),
        new(TestTwoId, UserId),
        new(TestTwoId, Guid.NewGuid())
    };

    private static IEnumerable<TestEntity> GetTestData()
    {
        return new List<TestEntity>
        {
            new()
            {
                Id = TestOneId,
                Title = "title1",
                UserId = UserId,
                Published = false
            },
            new()
            {
                Id = TestTwoId,
                Title = "title2",
                UserId = Guid.NewGuid(),
                Published = true
            }
        }.AsQueryable().BuildMockDbSet();
    }
}