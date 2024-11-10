using ApiPostoCombustivel.Database;
using ApiPostoCombustivel.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Configura��o do banco de dados em mem�ria
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("DBPosto"));

//builder.Services.AddScoped<ICombustivelService, CombustivelService>(); N�O PODE

// Adiciona os servi�os do controlador
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configura o pipeline de requisi��es HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
