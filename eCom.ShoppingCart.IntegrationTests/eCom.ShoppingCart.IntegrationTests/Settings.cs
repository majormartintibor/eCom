using Oakton;
using Xunit.Abstractions;
using Xunit.Sdk;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
[assembly: TestFramework("eCom.ShoppingCart.IntegrationTests.AssemblyFixture", "eCom.ShoppingCart.IntegrationTests")]

namespace eCom.ShoppingCart.IntegrationTests;

public sealed class AssemblyFixture : XunitTestFramework
{
    public AssemblyFixture(IMessageSink messageSink)
        : base(messageSink)
    {
        OaktonEnvironment.AutoStartHost = true;
    }
}