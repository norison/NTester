using AutoFixture;
using AutoFixture.NUnit3;

namespace NTester.Domain.Tests.Common;

public class AutoDataExtAttribute : AutoDataAttribute
{
    public AutoDataExtAttribute() : base(() =>
    {
        var fixture = new Fixture();
        fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        return fixture;
    })
    {
    }
}