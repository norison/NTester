using FluentValidation;
using FluentValidation.TestHelper;

namespace NTester.Domain.Tests.Common;

public abstract class ValidatorTestBase<TModel, TValidator> where TValidator : IValidator<TModel>, new()
{
    protected abstract TModel CreateValidObject();
    
    protected TestValidationResult<TModel> Validate(Action<TModel> mutate)
    {
        var model = CreateValidObject();
        mutate(model);
        return new TValidator().TestValidate(model);
    }
    
    protected TestValidationResult<TModel> Validate()
    {
        var model = CreateValidObject();
        return new TValidator().TestValidate(model);
    }
}