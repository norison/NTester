using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using NTester.DataAccess.Entities;
using NTester.DataContracts.Tests.Create;
using NTester.DataContracts.Tests.Create.Models;
using NTester.Domain.Features.Tests.Commands.Create;
using NTester.Domain.Features.Tests.Commands.Create.Models;

namespace NTester.Domain.Mappings.Tests;

/// <summary>
/// Mappings profile for the tests feature.
/// </summary>
[ExcludeFromCodeCoverage]
public class TestsMappingsProfile : Profile
{
    /// <summary>
    /// Creates and instance of the <see cref="TestsMappingsProfile"/>.
    /// </summary>
    public TestsMappingsProfile()
    {
        ConfigureCreateTest();
    }

    private void ConfigureCreateTest()
    {
        CreateMap<CreateTestRequest, CreateTestCommand>();
        CreateMap<CreateTestQuestionItem, CreateTestQuestionModel>();
        CreateMap<CreateTestAnswerItem, CreateTestAnswerModel>();

        CreateMap<CreateTestCommand, TestEntity>().AfterMap((_, dest) =>
        {
            dest.Id = Guid.NewGuid();
            foreach (var question in dest.Questions)
            {
                question.Id = Guid.NewGuid();
                question.TestId = dest.Id;

                foreach (var answer in question.Answers)
                {
                    answer.Id = Guid.NewGuid();
                    answer.QuestionId = question.Id;
                }
            }
        });

        CreateMap<CreateTestQuestionModel, QuestionEntity>();
        CreateMap<CreateTestAnswerModel, AnswerEntity>();
    }
}