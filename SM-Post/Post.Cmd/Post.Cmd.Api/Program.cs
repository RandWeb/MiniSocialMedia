using Post.Cmd.Api;
using Post.Cmd.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var services = builder.Services;

{
    services
        .AddInfrastructureDI()
        .AddApiDI(configuration);
}

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseRouting();
app.MapControllers();
app.Run();


