using AutoMapper;
using MediatR;
using NTester.DataAccess.Data.NTesterDbContext;
using NTester.DataContracts.Tests.GetTests;

namespace NTester.Domain.Features.Tests.Queries.GetTests.GetOwnTests;

/// <summary>
/// Handler for <see cref="GetOwnTestsQuery"/>.
/// </summary>
public class GetOwnTestsQueryHandler : GetTestsQueryHandlerBase, IRequestHandler<GetOwnTestsQuery, GetTestsResponse>
{
    private readonly INTesterDbContext _dbContext;

    /// <summary>
    /// Creates an instance of the <see cref="GetOwnTestsQueryHandler"/>.
    /// </summary>
    /// <param name="dbContext">Database context of the application.</param>
    /// <param name="mapper">Mapper of the entities.</param>
    public GetOwnTestsQueryHandler(INTesterDbContext dbContext, IMapper mapper) : base(mapper)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc cref="IRequestHandler{TRequest,TResponse}.Handle"/>
    public async Task<GetTestsResponse> Handle(GetOwnTestsQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Tests.Where(x => x.UserId == request.UserId && x.Published == request.Published);

        if (!string.IsNullOrEmpty(request.Title))
        {
            query = query.Where(x => x.Title.Contains(request.Title));
        }

        return await GetTestsResponse(request, query, cancellationToken);
    }
}