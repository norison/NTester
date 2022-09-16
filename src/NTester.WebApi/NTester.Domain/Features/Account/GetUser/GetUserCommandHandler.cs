using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NTester.DataAccess.Data.NTesterDbContext;
using NTester.DataContracts.Account.GetUser;
using NTester.Domain.Exceptions.Account;

namespace NTester.Domain.Features.Account.GetUser;

/// <summary>
/// Handler of <see cref="GetUserCommand"/>.
/// </summary>
public class GetUserCommandHandler : IRequestHandler<GetUserCommand, GetUserResponse>
{
    private readonly INTesterDbContext _dbContext;
    private readonly IMapper _mapper;

    /// <summary>
    /// Creates an instance of the <see cref="GetUserCommandHandler"/>.
    /// </summary>
    /// <param name="dbContext">Database context of the application.</param>
    /// <param name="mapper">Entities mapper.</param>
    public GetUserCommandHandler(INTesterDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    /// <inheritdoc cref="IRequestHandler{TRequest,TResponse}.Handle"/>
    public async Task<GetUserResponse> Handle(GetUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Users
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