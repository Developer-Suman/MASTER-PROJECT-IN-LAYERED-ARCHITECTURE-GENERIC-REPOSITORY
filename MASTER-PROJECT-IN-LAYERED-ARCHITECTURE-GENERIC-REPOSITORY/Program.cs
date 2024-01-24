using Master_BLL;
using Master_DAL;

var builder = WebApplication.CreateBuilder(args);



ConfigurationManager configuration = builder.Configuration;
builder.Services
    .AddBLL()
    .AddDAL(configuration);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
