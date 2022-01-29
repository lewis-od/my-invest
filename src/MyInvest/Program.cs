using MyInvest;
using MyInvest.Domain;
using MyInvest.Domain.Account;
using MyInvest.Domain.Client;
using MyInvest.Domain.Id;
using MyInvest.Persistence;
using MyInvest.REST;
using MyInvest.REST.Account;
using MyInvest.REST.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mapperConfig = new AutoMapperConfig();
mapperConfig.RegisterModule(new RestMapperModule());

var mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddSingleton<AccountMapper>();
builder.Services.AddSingleton<IUniqueIdGenerator<AccountId>, AccountIdGenerator>();
builder.Services.AddSingleton<AccountFactory>();
builder.Services.AddSingleton<IAccountRepository, InMemoryAccountRepository>();

builder.Services.AddSingleton<ClientMapper>();
builder.Services.AddSingleton<IUniqueIdGenerator<ClientId>, ClientIdGenerator>();
builder.Services.AddSingleton<IClientRepository, InMemoryClientRepository>();
builder.Services.AddSingleton<ClientService>();

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