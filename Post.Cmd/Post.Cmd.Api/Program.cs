using Post.Cmd.Api;
using Post.Cmd.Api.Commands;
using Post.Cmd.Infrastructure;
using Post.Cmd.Infrastructure.Settings;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var services = builder.Services;

{
    services.Configure<MongoDbSettings>(configuration.GetSection(nameof(MongoDbSettings)));

    services
        .AddInfrastructureDI()
        .AddApiDI();
}

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.Run();


