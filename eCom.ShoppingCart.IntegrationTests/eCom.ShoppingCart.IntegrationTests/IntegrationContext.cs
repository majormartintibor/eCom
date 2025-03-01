﻿using Alba;
using Bogus;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Wolverine.Tracking;

namespace eCom.ShoppingCart.IntegrationTests;

// xUnit specific junk
[CollectionDefinition("integration")]
public class IntegrationCollection : ICollectionFixture<AppFixture>
{
}

[Collection("integration")]
public abstract class IntegrationContext : IAsyncLifetime
{
    private readonly AppFixture _fixture;
    protected readonly Faker _faker;

    protected IntegrationContext(AppFixture fixture)
    {
        _fixture = fixture;
        _faker = new Faker();
    }

    public IAlbaHost Host => _fixture.Host;

    public IDocumentStore Store => _fixture.Host.Services.GetRequiredService<IDocumentStore>();

    public virtual async Task InitializeAsync()
    {
        // Using Marten, wipe out all data and reset the state
        // back to exactly what we described in BaselineData
        await Store.Advanced.ResetAllData();
    }

    // This is required because of the IAsyncLifetime 
    // interface. Note that I do *not* tear down database
    // state after the test. That's purposeful
    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    // This is just delegating to Alba to run HTTP requests
    // end to end
    public async Task<IScenarioResult> Scenario(Action<Scenario> configure)
    {
        return await Host.Scenario(configure);
    }

    // This method allows us to make HTTP calls into our system
    // in memory with Alba, but do so within Wolverine's test support
    // for message tracking to both record outgoing messages and to ensure
    // that any cascaded work spawned by the initial command is completed
    // before passing control back to the calling test
    protected async Task<(ITrackedSession, IScenarioResult)> TrackedHttpCall(Action<Scenario> configuration)
    {
        IScenarioResult result = null;

        // The outer part is tying into Wolverine's test support
        // to "wait" for all detected message activity to complete
        var tracked = await Host.ExecuteAndWaitAsync(async () =>
        {
            // The inner part here is actually making an HTTP request
            // to the system under test with Alba
            result = await Host.Scenario(configuration);
        });

        return (tracked, result);
    }
}