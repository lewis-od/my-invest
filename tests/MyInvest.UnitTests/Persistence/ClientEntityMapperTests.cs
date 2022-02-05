using System;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using MyInvest.Domain.Accounts;
using MyInvest.Domain.Clients;
using MyInvest.Persistence;
using MyInvest.Persistence.Clients;
using NUnit.Framework;

namespace MyInvest.UnitTests.Persistence;

public class ClientEntityMapperTests
{
    private readonly ClientEntityMapper _mapper;

    public ClientEntityMapperTests()
    {
        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<PersistenceMapperProfile>());
        _mapper = new ClientEntityMapper(mapperConfig.CreateMapper());
    }

    [Test]
    public void MapsEntityToDomain()
    {
        var clientId = Guid.NewGuid();
        const string username = "lewis";
        var entity = new ClientEntity {ClientId = clientId, Username = username};

        var client = _mapper.MapFromEntity(entity);

        var expectedClient = new Client(ClientId.From(clientId), username, Enumerable.Empty<InvestmentAccount>());
        client.Should().BeEquivalentTo(expectedClient);
    }

    [Test]
    public void MapsDomainToEntity()
    {
        var clientId = Guid.NewGuid();
        const string username = "lewis";
        var client = new Client(ClientId.From(clientId), username, Enumerable.Empty<InvestmentAccount>());

        var entity = _mapper.MapToEntity(client);

        var expectedEntity = new ClientEntity {ClientId = clientId, Username = username};
        entity.Should().BeEquivalentTo(expectedEntity);
    }
}
