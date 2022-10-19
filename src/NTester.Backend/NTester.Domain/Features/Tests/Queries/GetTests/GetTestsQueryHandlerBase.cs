using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using NTester.DataAccess.Entities;
using NTester.DataContracts.Tests.GetTests;
using NTester.DataContracts.Tests.GetTests.Models;
using NTester.Domain.Features.Tests.Queries.Common;

namespace NTester.Domain.Features.Tests.Queries.GetTests;

/// <summary>
/// Base query handler for the get tests queries.
/// </summary>
public abstract class GetTestsQueryHandlerBase
{
    private readonly IMapper _mapper;

    /// <summary>
    /// Creates an instance of the <see cref="GetTestsQueryHandlerBase"/>.
    /// </summary>
    /// <param name="mapper">Mapper.</param>
    protected GetTestsQueryHandlerBase(IMapper mapper)
    {
        _mapper = mapper;
    }

    /// <summary>
    /// Gets a page for 
    /// </summary>
    /// <param name="pageQuery">Page query.</param>
    /// <param name="query">Query with configured actions.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Total count and list of the tests.</returns>
    protected async Task<GetTestsResponse> GetTestsResponse(
        PageQuery pageQuery,
        IQueryable<TestEntity> query,
        CancellationToken cancellationToken)
    {
        var total = await query.CountAsync(cancellationToken);

        var tests = await query
            .OrderByDescending(x => x.CreationDateTime)
            .Skip(pageQuery.PageNumber * pageQuery.PageSize - pageQuery.PageSize)
            .Take(pageQuery.PageSize)
            .ProjectTo<GetTestsResponseItem>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new GetTestsResponse
        {
            Total = total,
            Tests = tests
        };
    }
}