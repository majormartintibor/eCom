using Marten.Schema;
using Marten;

namespace eCom.ShoppingCart.IntegrationTests;

internal sealed class BaselineData : IInitialData
{
    public Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        return Task.CompletedTask;
    }
}