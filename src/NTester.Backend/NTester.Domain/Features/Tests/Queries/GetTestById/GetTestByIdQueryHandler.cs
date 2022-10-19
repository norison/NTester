using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NTester.DataAccess.Data.NTesterDbContext;
using NTester.DataContracts.Tests.GetTestById;
using NTester.Domain.Exceptions.Tests;

namespace NTester.Domain.Features.Tests.Queries.GetTestById;

/// <summary>
/// Handler for <see cref="GetTestByIdQuery"/>.
/// </summary>
public class GetTestByIdQueryHandler : IRequestHandler<GetTestByIdQuery, GetTestByIdResponse>
{
    private readonly INTesterDbContext _dbContext;
    private readonly IMapper _mapper;

    /// <summary>
    /// Creates an instance of the <see cref="GetTestByIdQueryHandler"/>.
    /// </summary>
    /// <param name="dbContext">Database context of the application.</param>
    /// <param name="mapper">Mapper of the entities.</param>
    public GetTestByIdQueryHandler(INTesterDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    /// <inheritdoc cref="IRequestHandler{TRequest,TResponse}.Handle"/>
    public async Task<GetTestByIdResponse> Handle(GetTestByIdQuery request, CancellationToken cancellationToken)
    {
        var test = await _dbContext.Tests
            .Include(x => x.Questions)
            .ThenInclude(x => x.Answers)
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (test == null || !test.Published && test.UserId != request.UserId)
        {
            throw new TestNotFoundException(request.Id);
        }

        return _mapper.Map<GetTestByIdResponse>(test);
    }
}