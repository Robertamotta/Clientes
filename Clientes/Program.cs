using Clientes.Api.Consumers;
using Clientes.Aplicacao.Servicos;
using Clientes.Dominio.Interfaces;
using Clientes.Infraestrutura.Dados;
using Clientes.Infraestrutura.Interfaces;
using Clientes.Infraestrutura.Servico;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuração do MySQL
var connectionString = builder.Configuration.GetConnectionString("StringConnectionMySql");

builder.Services.AddDbContext<ClientesContext>(opts =>
 opts.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Dependency Resolver 
builder.Services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
builder.Services.AddScoped<IEmissaoCartaoCreditoServico, EmissaoCartaoCreditoServico>();
builder.Services.AddScoped<IMensageria, Mensageria>();
builder.Services.AddScoped<IPropostaCreditoServico, PropostaCreditoServico>();
builder.Services.AddScoped<IClienteServico, ClienteServico>();

builder.Services.AddHostedService<EmissaoCartaoCreditoConsumer>();
builder.Services.AddHostedService<PropostaCreditoConsumer>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
