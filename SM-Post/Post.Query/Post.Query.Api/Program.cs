using Post.Query.Api;
using Post.Query.Infrastructure;

var builder = WebApplication.CreateBuilder(args);


var services = builder.Services;
var configuration = builder.Configuration;
{
    services
        .AddInfrastructureDI(configuration)
        .AddApiDI();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.Run();

