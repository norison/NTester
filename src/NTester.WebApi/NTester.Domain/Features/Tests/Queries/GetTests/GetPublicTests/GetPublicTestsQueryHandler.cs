using AutoMapper;
using MediatR;
using NTester.DataAccess.Data.NTesterDbContext;
using NTester.DataContracts.Tests.GetTests;

namespace NTester.Domain.Features.Tests.Queries.GetTests.GetPublicTests;

/// <summary>
/// Handler for <see cref="GetPublicTestsQuery"/>.
/// </summary>
public class GetPublicTestsQueryHandler :
    GetTestsQueryHandlerBase,
    IRequestHandler<GetPublicTestsQuery, GetTestsResponse>
{
    private readonly INTesterDbContext _dbContext;

    /// <summary>
    /// Creates an instance of the <see cref="GetPublicTestsQueryHandler"/>.
    /// </summary>
    /// <param name="dbContext">Database context of the application.</param>
    /// <param name="mapper">Mapper of the entities.</param>
    public GetPublicTestsQueryHandler(INTesterDbContext dbContext, IMapper mapper) : base(mapper)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc cref="IRequestHandler{TRequest,TResponse}.Handle"/>
    public async Task<GetTestsResponse> Handle(GetPublicTestsQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Tests.Where(x => x.Published && x.UserId != request.UserId);

        if (!string.IsNullOrEmpty(request.Title))
        {
            query = query.Where(x => x.Title.Contains(request.Title));
        }

        return await GetTestsResponse(request, query, cancellationToken);
    }
}