using Copitos20241209;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDb>(options => options.UseSqlServer("Data Source=.\\;Initial Catalog=copitos20241209;Integrated Security=true;trust server certificate=true"));

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
