using ApiPostoCombustivel.Database;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json.Serialization;

Log.Logger = new LoggerConfiguration()
    // Configura o log para ser exibido no console.
    .WriteTo.Console()
    // Configura o log para ser gravado em arquivos, com rotação diária.
    .WriteTo.File(
        path: "logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    // Define o nível mínimo de log como Debug, ou seja, logs com nível Debug ou superior serão registrados.
    .MinimumLevel.Debug()
    // Cria a configuração do logger.
    .CreateLogger();

try
{
    // Loga a mensagem informando que a aplicação está sendo inicializada.
    Log.Information("Inicializando aplicação...");

    // Cria o builder da aplicação web.
    var builder = WebApplication.CreateBuilder(args);

    // Configura o host para usar o Serilog como o mecanismo de logging.
    builder.Host.UseSerilog();

    // Configura o serviço de contexto de banco de dados para usar um banco de dados em memória.
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseInMemoryDatabase("DBPosto")); // Define o nome do banco como "DBPosto".

    // Adiciona os serviços para o uso de controladores (APIs).
    builder.Services.AddControllers();
    // Adiciona suporte para explorar os pontos finais da API.
    builder.Services.AddEndpointsApiExplorer();
    // Adiciona o Swagger para documentação da API.
    builder.Services.AddSwaggerGen();

    // Cria a aplicação web.
    var app = builder.Build();

    // Se o ambiente for de desenvolvimento, habilita o Swagger para documentação e testes interativos da API.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger(); // Ativa o Swagger.
        app.UseSwaggerUI(); // Ativa a interface do usuário do Swagger.
    }

    // Habilita a autorização na aplicação (necessário para autenticação e autorização de recursos protegidos).
    app.UseAuthorization();
    // Mapeia os controladores para os pontos finais da API.
    app.MapControllers();

    // Loga a mensagem de sucesso indicando que a aplicação foi iniciada corretamente.
    Log.Information("Aplicação iniciada com sucesso.");
    // Inicia a execução da aplicação web.
    app.Run();
}
catch (Exception ex)
{
    // Em caso de falha ao iniciar a aplicação, registra o erro e a mensagem de falha.
    Log.Fatal(ex, "A aplicação falhou ao iniciar.");
}
finally
{
    // Garante que o logger seja fechado e todos os logs sejam gravados corretamente.
    Log.CloseAndFlush();
}
