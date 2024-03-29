using AutoMapper;
using FluentAssertions;
using MockQueryable.NSubstitute;
using NSubstitute;
using NTester.DataAccess.Entities;
using NTester.Domain.Exceptions.Account;
using NTester.Domain.Features.Account.Queries.GetUser;
using NTester.Domain.Services.UserManager;
using NTester.Domain.Tests.Common;
using NUnit.Framework;

namespace NTester.Domain.Tests.Features.Account.Queries.GetUser;

[TestFixture]
public class GetUserQueryHandlerTests
{
    private IUserManager _userManager;
    private IMapper _mapper;
    private GetUserQueryHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _userManager = Substitute.For<IUserManager>();
        _mapper = MapperFactory.CreateMapper();

        _handler = new GetUserQueryHandler(_userManager, _mapper);
    }

    [Test, AutoDataExt]
    public async Task Handle_UserNotFound_ShouldThrowAnException(Guid userId)
    {
        // Arrange
        var command = new GetUserQuery { UserId = userId };
        var usersDbSet = Array.Empty<UserEntity>().AsQueryable().BuildMockDbSet();
        _userManager.Users.Returns(usersDbSet);

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
        var command = new GetUserQuery { UserId = user.Id };
        var usersDbSet = new List<UserEntity> { user }.AsQueryable().BuildMockDbSet();
        _userManager.Users.Returns(usersDbSet);

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