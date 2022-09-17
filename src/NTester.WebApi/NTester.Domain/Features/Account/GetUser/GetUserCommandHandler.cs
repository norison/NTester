using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NTester.DataContracts.Account.GetUser;
using NTester.Domain.Exceptions.Account;
using NTester.Domain.Services.UserManager;

namespace NTester.Domain.Features.Account.GetUser;

/// <summary>
/// Handler of <see cref="GetUserCommand"/>.
/// </summary>
public class GetUserCommandHandler : IRequestHandler<GetUserCommand, GetUserResponse>
{
    private readonly IUserManager _userManager;
    private readonly IMapper _mapper;

    /// <summary>
    /// Creates an instance of the <see cref="GetUserCommandHandler"/>.
    /// </summary>
    /// <param name="userManager">Manager of the users.</param>
    /// <param name="mapper">Entities mapper.</param>
    public GetUserCommandHandler(IUserManager userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    /// <inheritdoc cref="IRequestHandler{TRequest,TResponse}.Handle"/>
    public async Task<GetUserResponse> Handle(GetUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _userManager.Users
            .Where(x => x.Id == request.UserId)
            .ProjectTo<GetUserResponse>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (result == null)
        {
            throw new UserNotFoundException(request.UserId);
        }

        return result;
    }
}