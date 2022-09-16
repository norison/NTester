using AutoMapper;
using FluentAssertions;
using MockQueryable.NSubstitute;
using NSubstitute;
using NTester.DataAccess.Data.NTesterDbContext;
using NTester.DataAccess.Entities;
using NTester.Domain.Exceptions.Account;
using NTester.Domain.Features.Account.GetUser;
using NTester.Domain.Mappings;
using NTester.Domain.Tests.Common;
using NUnit.Framework;

namespace NTester.Domain.Tests.Features.Account.GetUser;

[TestFixture]
public class GetUserCommandHandlerTests
{
    private INTesterDbContext _dbContext;
    private IMapper _mapper;
    private GetUserCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _dbContext = Substitute.For<INTesterDbContext>();
        _mapper = Substitute.For<IMapper>();

        var configuration = new MapperConfiguration(configure => { configure.AddProfile<ApplicationProfile>(); });
        _mapper.ConfigurationProvider.Returns(configuration);

        _handler = new GetUserCommandHandler(_dbContext, _mapper);
    }

    [Test, AutoDataExt]
    public async Task Handle_UserNotFound_ShouldThrowAnException(Guid userId)
    {
        // Arrange
        var command = new GetUserCommand { UserId = userId };
        var usersDbSet = Array.Empty<UserEntity>().AsQueryable().BuildMockDbSet();
        _dbContext.Users.Returns(usersDbSet);

        // Act/Assert
        await _handler
            .Invoking(x => x.Handle(command, CancellationToken.None))
            .Should()
            .ThrowAsync<UserNotFoundException>();
    }

    [Test, AutoDataExt]
    public async Task Handle_NoErrors_ShouldReturnCorrectResult(UserEntity user)
    {
        // Arrange
        var command = new GetUserCommand { UserId = user.Id };
        var usersDbSet = new List<UserEntity> { user }.AsQueryable().BuildMockDbSet();
        _dbContext.Users.Returns(usersDbSet);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Id.Should().Be(user.Id);
        result.UserName.Should().Be(user.UserName);
        result.Email.Should().Be(user.Email);
        result.Name.Should().Be(user.Name);
        result.Surname.Should().Be(user.Surname);
    }
}