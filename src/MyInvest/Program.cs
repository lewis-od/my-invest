using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using MyInvest;
using MyInvest.Domain.Accounts;
using MyInvest.Domain.BackOffice;
using MyInvest.Domain.Clients;
using MyInvest.Domain.Ids;
using MyInvest.Domain.Transactions;
using MyInvest.Persistence;
using MyInvest.Persistence.Accounts;
using MyInvest.Persistence.Clients;
using MyInvest.Ports.Transactions;
using MyInvest.REST;
using MyInvest.REST.Accounts;
using MyInvest.REST.Clients;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(builder.Configuration));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddTransient<ProblemDetailsFactory, MyInvestProblemDetailsFactory>();

builder.Services.AddDbContext<MyInvestDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Postgres");
    options.UseNpgsql(connectionString);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mapperConfig = AutoMapperConfigFactory.Create();
builder.Services.AddSingleton(mapperConfig.CreateMapper());

builder.Services.AddSingleton<AccountDtoMapper>();
builder.Services.AddSingleton<IUniqueIdGenerator<AccountId>, AccountIdGenerator>();
builder.Services.AddSingleton<AccountFactory>();
builder.Services.AddSingleton<IInvestmentAccountEntityMapper, InvestmentAccountEntityMapper>();
builder.Services.AddScoped<IAccountRepository, InvestmentAccountRepository>();
builder.Services.AddScoped<IInvestmentAccountDao, InvestmentAccountDao>();
builder.Services.AddScoped<AccountOpeningService>();

builder.Services.AddSingleton<ClientDtoMapper>();
builder.Services.AddSingleton<IClientEntityMapper, ClientEntityMapper>();
builder.Services.AddSingleton<IUniqueIdGenerator<ClientId>, ClientIdGenerator>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IClientDao, ClientDao>();
builder.Services.AddScoped<OnboardingService>();
builder.Services.AddScoped<IBackOfficeService, BackOfficeService>();
builder.Services.AddScoped<ITransactionVerifier, NoOpTransactionVerifier>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler("/error");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
