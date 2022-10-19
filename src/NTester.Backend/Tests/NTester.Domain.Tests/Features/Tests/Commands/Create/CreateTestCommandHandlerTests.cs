using AutoFixture.NUnit3;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using NTester.DataAccess.Data.NTesterDbContext;
using NTester.DataAccess.Entities;
using NTester.Domain.Features.Tests.Commands.Create;
using NTester.Domain.Tests.Common;
using NUnit.Framework;

namespace NTester.Domain.Tests.Features.Tests.Commands.Create;

[TestFixture]
public class CreateTestCommandHandlerTests
{
    private INTesterDbContext _dbContext;
    private IMapper _mapper;
    private CreateTestCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _dbContext = Substitute.For<INTesterDbContext>();
        _mapper = MapperFactory.CreateMapper();

        _handler = new CreateTestCommandHandler(_dbContext, _mapper);
    }

    [Test, AutoData]
    public async Task Handle_NoErrors_ShouldReturnCorrectResult(CreateTestCommand createTestCommand)
    {
        // Arrange
        TestEntity capturedTest = null!;

        _dbContext.Tests
            .WhenForAnyArgs(x => x.AddAsync(default!))
            .Do(x => capturedTest = x.Arg<TestEntity>());

        // Act
        var result = await _handler.Handle(createTestCommand, CancellationToken.None);

        // Assert
        await _dbContext.Tests.Received().AddAsync(capturedTest, CancellationToken.None);
        await _dbContext.Received().SaveChangesAsync();

        result.TestId.Should().Be(capturedTest.Id);

        capturedTest.Title.Should().Be(createTestCommand.Title);
        capturedTest.Description.Should().Be(createTestCommand.Description);
        capturedTest.UserId.Should().Be(createTestCommand.UserId);
        capturedTest.Questions.Should().NotBeEmpty();

        foreach (var question in createTestCommand.Questions)
        {
            var testQuestion = capturedTest.Questions
                .Should()
                .ContainSingle(x => x.Content == question.Content && x.TestId == capturedTest.Id)
                .Which;

            foreach (var answer in question.Answers)
            {
                testQuestion.Answers
                    .Should()
                    .ContainSingle(x =>
                        x.Content == answer.Content && x.IsCorrect == answer.IsCorrect &&
                        x.QuestionId == testQuestion.Id);
            }
        }
    }
}