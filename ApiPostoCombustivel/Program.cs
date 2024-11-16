using ApiPostoCombustivel.Database;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json.Serialization;

Log.Logger = new LoggerConfiguration()
    // Configura o log para ser exibido no console.
    .WriteTo.Console()
    // Configura o log para ser gravado em arquivos, com rota��o di�ria.
    .WriteTo.File(
        path: "logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    // Define o n�vel m�nimo de log como Debug, ou seja, logs com n�vel Debug ou superior ser�o registrados.
    .MinimumLevel.Debug()
    // Cria a configura��o do logger.
    .CreateLogger();

try
{
    // Loga a mensagem informando que a aplica��o est� sendo inicializada.
    Log.Information("Inicializando aplica��o...");

    // Cria o builder da aplica��o web.
    var builder = WebApplication.CreateBuilder(args);

    // Configura o host para usar o Serilog como o mecanismo de logging.
    builder.Host.UseSerilog();

    // Configura o servi�o de contexto de banco de dados para usar um banco de dados em mem�ria.
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseInMemoryDatabase("DBPosto")); // Define o nome do banco como "DBPosto".

    // Adiciona os servi�os para o uso de controladores (APIs).
    builder.Services.AddControllers();
    // Adiciona suporte para explorar os pontos finais da API.
    builder.Services.AddEndpointsApiExplorer();
    // Adiciona o Swagger para documenta��o da API.
    builder.Services.AddSwaggerGen();

    // Cria a aplica��o web.
    var app = builder.Build();

    // Se o ambiente for de desenvolvimento, habilita o Swagger para documenta��o e testes interativos da API.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger(); // Ativa o Swagger.
        app.UseSwaggerUI(); // Ativa a interface do usu�rio do Swagger.
    }

    // Habilita a autoriza��o na aplica��o (necess�rio para autentica��o e autoriza��o de recursos protegidos).
    app.UseAuthorization();
    // Mapeia os controladores para os pontos finais da API.
    app.MapControllers();

    // Loga a mensagem de sucesso indicando que a aplica��o foi iniciada corretamente.
    Log.Information("Aplica��o iniciada com sucesso.");
    // Inicia a execu��o da aplica��o web.
    app.Run();
}
catch (Exception ex)
{
    // Em caso de falha ao iniciar a aplica��o, registra o erro e a mensagem de falha.
    Log.Fatal(ex, "A aplica��o falhou ao iniciar.");
}
finally
{
    // Garante que o logger seja fechado e todos os logs sejam gravados corretamente.
    Log.CloseAndFlush();
}
