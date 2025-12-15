
using CadastroFuncionarios.Api.Data;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

DotNetEnv.Env.Load("../.env");

var builder = WebApplication.CreateBuilder(args);

// .env
var dbName = "empresas_db";
var dbHost = Environment.GetEnvironmentVariable("MYSQL_HOST");
var dbPort = Environment.GetEnvironmentVariable("MYSQL_PORT");
var dbUser = Environment.GetEnvironmentVariable("MYSQL_USER");
var dbPassword = Environment.GetEnvironmentVariable("MYSQL_PASSWORD");

// CONNECTION STRING
var connectionString =
    $"server={dbHost};Port={dbPort};Database={dbName};User={dbUser};Password={dbPassword};" +
    $"SslMode=None;AllowPublicKeyRetrieval=True";

// DB CONTEXT
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

//Controllers
builder.Services.AddControllers();

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Porta
var url = Environment.GetEnvironmentVariable("FUNCIONARIOS_API_URL");
builder.WebHost.UseUrls(url ?? "http://localhost:5002");

var app = builder.Build();

// middleware de tratamento global de exceções
app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        var problem = new ProblemDetails
        {
            Instance = context.Request.Path
        };

        switch (exception)
        {
            case MySqlException:
                problem.Status = StatusCodes.Status503ServiceUnavailable;
                problem.Title = "Banco de dados indisponível";
                problem.Detail = "Não foi possível conectar ao banco de dados.";
                break;

            case DbUpdateException:
                problem.Status = StatusCodes.Status400BadRequest;
                problem.Title = "Erro ao persistir dados";
                problem.Detail = "Falha ao salvar as informações.";
                break;

            default:
                problem.Status = StatusCodes.Status500InternalServerError;
                problem.Title = "Erro interno do servidor";
                problem.Detail = "Ocorreu um erro inesperado.";
                break;
        }

        context.Response.StatusCode = problem.Status.Value;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(problem);
    });
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});
app.MapControllers();

app.Run();
