using System.Reflection;
using Fiap.TechChallenge.Command.v1.Contato;
using Fiap.TechChallenge.CommandStore;
using Fiap.TechChallenge.Contato;
using Fiap.TechChallenge.Contract.v1.Contato.AtualizarContato;
using Fiap.TechChallenge.Contract.v1.Contato.CriarContato;
using Fiap.TechChallenge.Contract.v1.Contato.ObterContatoPorId;
using Fiap.TechChallenge.Contract.v1.Contato.ObterContatosPorDdd;
using Fiap.TechChallenge.Contract.v1.Contato.RemoverContato;
using Fiap.TechChallenge.DataBaseContext;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Configurar a string de conexão MySQL no appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registrar o DbContext para usar o MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Configurar o controle de serialização para ignorar valores nulos e tratar loops de referência
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    // Ignorar valores nulos durante a serialização
    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;

    // Tratar loops de referência durante a serialização
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
});

// Configuração do Swagger para documentação da API
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Cadastro de Contatos API",
        Version = "v1",
        Description =
            "API para o gerenciamento de contatos regionais, incluindo funcionalidades de criação, consulta, atualização e remoção de contatos. Cada contato pode ser associado a um DDD específico, permitindo a segmentação de contatos por região.",
        Contact = new OpenApiContact
        {
            Name = "Grupo 12"
        }
    });

    // Incluir comentários XML no Swagger (opcional)
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath, true);
});

// Configuração do versionamento de API
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true; // Exibir versões suportadas no cabeçalho da resposta
    options.AssumeDefaultVersionWhenUnspecified = true; // Usar a versão padrão caso nenhuma seja especificada
    options.DefaultApiVersion = new ApiVersion(1, 0); // Versão padrão (v1.0)
    options.ApiVersionReader = new UrlSegmentApiVersionReader(); // Versionamento via segmento da URL (ex.: v1)
});

// Configuração do API Explorer para versionamento
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV"; // Agrupamento por 'v1', 'v2', etc.
    options.SubstituteApiVersionInUrl = true; // Substituir {version} na rota pela versão real
});

// Registrar serviços com ciclo de vida 'Transient'
builder.Services.AddTransient<IContatoService, ContatoService>();
builder.Services.AddTransient<IContatoCommandStore, ContatoCommandStore>();
builder.Services.AddTransient<IContatoQueryStore, ContatoQueryStore>();

// Registrar Handlers para comandos e queries
builder.Services.AddTransient<AtualizarContatoCommandHandler>();
builder.Services.AddTransient<CriarContatoCommandHandler>();
builder.Services.AddTransient<RemoverContatoCommandHandler>();
builder.Services.AddTransient<ObterContatoPorIdQueryHandler>();
builder.Services.AddTransient<ObterContatosPorDddQueryHandler>();

// Configuração de validação com FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<AtualizarContatoCommand>();
builder.Services.AddValidatorsFromAssemblyContaining<CriarContatoCommand>();
builder.Services.AddValidatorsFromAssemblyContaining<RemoverContatoCommand>();
builder.Services.AddValidatorsFromAssemblyContaining<ObterContatoPorIdQueryRequest>();
builder.Services.AddValidatorsFromAssemblyContaining<ObterContatosPorDddQueryRequest>();

var app = builder.Build();

// Adiciona o middleware do Prometheus
app.UseHttpMetrics(); // Coleta métricas HTTP padrão (inclui latência, contadores, etc.)

// Endpoint para expor métricas do Prometheus
app.MapMetrics();


// Garantir que o banco de dados e as tabelas sejam criados se ainda não existirem
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated(); // Cria o banco de dados e tabelas, se não existirem
}

// Configuração do pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Exibir informações detalhadas sobre erros em ambiente de desenvolvimento
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cadastro de Contatos API v1"); });
}

app.UseHttpsRedirection();

app.UseCors(policy =>
    policy.AllowAnyOrigin() // Permitir qualquer origem
        .AllowAnyMethod() // Permitir qualquer método HTTP
        .AllowAnyHeader()); // Permitir qualquer cabeçalho

app.UseAuthorization(); // Ativar a autorização

app.MapControllers(); // Mapear as rotas dos controladores

app.Run(); // Iniciar a aplicação