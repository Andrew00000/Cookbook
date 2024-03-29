using Cookbook.API.ErrorHandeling;
using Cookbook.API.Mapping;
using Cookbook.Application;
using Cookbook.Application.Utilities;
using Cookbook.Repository.Repositories;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddControllers(options =>
        options.Filters.Add<ExceptionFilter>());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddDatabase(config);
builder.Services.AddRepositories();

var app = builder.Build();

var initializables = app.Services.GetRequiredService<IEnumerable<IInitializable>>();
foreach (var initializable in initializables)
{
    initializable.Initialize();
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ValidationMappingMiddleware>();
app.MapControllers();

app.Run();
