using Farmacia.Estoque.API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = "Server=localhost;Database=FarmaciaEstoqueDb;Uid=root;Pwd=;";
builder.Services.AddDbContext<EstoqueDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 30))));

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); 
}

app.UseAuthorization();
app.MapControllers();
app.Run();