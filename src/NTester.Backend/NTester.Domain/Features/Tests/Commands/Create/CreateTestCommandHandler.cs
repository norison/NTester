using AutoMapper;
using MediatR;
using NTester.DataAccess.Data.NTesterDbContext;
using NTester.DataAccess.Entities;
using NTester.DataContracts.Tests.Create;

namespace NTester.Domain.Features.Tests.Commands.Create;

/// <summary>
/// Handler of the create test command.
/// </summary>
public class CreateTestCommandHandler : IRequestHandler<CreateTestCommand, CreateTestResponse>
{
    private readonly INTesterDbContext _dbContext;
    private readonly IMapper _mapper;

    /// <summary>
    /// Creates an instance of the <see cref="CreateTestCommandHandler"/>.
    /// </summary>
    /// <param name="dbContext">Database context of the application.</param>
    /// <param name="mapper">Mapper of the entities.</param>
    public CreateTestCommandHandler(INTesterDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    /// <inheritdoc cref="IRequestHandler{TRequest,TResponse}.Handle"/>
    public async Task<CreateTestResponse> Handle(CreateTestCommand request, CancellationToken cancellationToken)
    {
        var test = _mapper.Map<TestEntity>(request);

        await _dbContext.Tests.AddAsync(test, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CreateTestResponse
        {
            TestId = test.Id
        };
    }
}