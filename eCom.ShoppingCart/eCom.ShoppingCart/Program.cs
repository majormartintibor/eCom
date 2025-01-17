using eCom.ShoppingCart.ShoppingCart;
using JasperFx.CodeGeneration;
using JasperFx.Core;
using Marten;
using Marten.Events.Daemon.Resiliency;
using Marten.Exceptions;
using Npgsql;
using Oakton;
using Weasel.Core;
using Wolverine;
using Wolverine.ErrorHandling;
using Wolverine.FluentValidation;
using Wolverine.Http;
using Wolverine.Http.FluentValidation;
using Wolverine.Marten;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddWolverineHttp()
    .AddMarten(opts =>
    {
        var connectionString = builder.Configuration.GetConnectionString("default");
        opts.Connection(connectionString!);

        opts.AutoCreateSchemaObjects = AutoCreate.All;

        opts.ConfigureShoppingCart();

    })
    .AddAsyncDaemon(DaemonMode.Solo)
    .IntegrateWithWolverine();

builder.Host.UseWolverine(opts =>
{
    opts.CodeGeneration.TypeLoadMode = TypeLoadMode.Dynamic;

    //durability for transient errors
    opts.OnException<NpgsqlException>().Or<MartenCommandException>()
        .RetryWithCooldown(50.Milliseconds(), 100.Milliseconds(), 250.Milliseconds());

    // Apply the validation middleware *and* discover and register
    // Fluent Validation validators
    opts.UseFluentValidation();

    // Automatic transactional middleware
    opts.Policies.AutoApplyTransactions();

    // Opt into the transactional inbox for local 
    // queues
    opts.Policies.UseDurableLocalQueues();

    // Opt into the transactional inbox/outbox on all messaging
    // endpoints
    opts.Policies.UseDurableOutboxOnAllSendingEndpoints();

    //add local queue stuff here
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapWolverineEndpoints(opts =>
{
    //Directs Wolverine.HTTP to use Fluent Validation middleware
    //to validate any request bodies where there's a known
    //validator (or many validators)
    opts.UseFluentValidationProblemDetailMiddleware();

    //here you have the possiniliy to add further middleware
    //Example:
    //opts.AddMiddleware(typeof(MyMiddleware));
});

app.UseHttpsRedirection();

return await app.RunOaktonCommands(args);

//Necessary for IntegrationTest project
public partial class Program
{

}