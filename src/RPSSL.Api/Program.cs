using RPSSL.Application;
using RPSSL.Api.Extensions;
using RPSSL.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddAppConfigurations(builder.Environment)
    .Build();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.UseCustomExceptionHandler();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
