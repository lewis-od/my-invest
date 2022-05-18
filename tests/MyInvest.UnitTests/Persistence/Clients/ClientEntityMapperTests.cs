using System;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using MyInvest.Domain.Accounts;
using MyInvest.Domain.Clients;
using MyInvest.Persistence;
using MyInvest.Persistence.Clients;
using MyInvest.UnitTests.Domain.Accounts;
using NUnit.Framework;

namespace MyInvest.UnitTests.Persistence.Clients;

public class ClientEntityMapperTests
{
    private static readonly PostalAddress Address = new("line1", "line2", "postcode");
    
    private readonly ClientEntityMapper _mapper;

    public ClientEntityMapperTests()
    {
        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<PersistenceMapperProfile>());
        _mapper = new ClientEntityMapper(mapperConfig.CreateMapper());
    }

    [Test]
    public void MapsEntityWithUnverifiedAddressToDomain()
    {
        var clientId = Guid.NewGuid();
        const string username = "lewis";
        var entity = new ClientEntity
        {
            ClientId = clientId,
            Username = username,
            AddressLine1 = "line1",
            AddressLine2 = "line2",
            AddressPostcode = "postcode",
            AddressIsVerified = false,
        };
        var accounts = new[] { TestAccountFactory.NewAccount(clientId, AccountType.GIA) };

        var client = _mapper.MapFromEntity(entity, accounts);

        var expectedClient = new Client(ClientId.From(clientId), username, Address, accounts);
        client.Should().BeEquivalentTo(expectedClient);
    }
    
    [Test]
    public void MapsEntityWithVerifiedAddressToDomain()
    {
        var clientId = Guid.NewGuid();
        const string username = "lewis";
        var entity = new ClientEntity
        {
            ClientId = clientId,
            Username = username,
            AddressLine1 = "line1",
            AddressLine2 = "line2",
            AddressPostcode = "postcode",
            AddressIsVerified = true,
        };
        var accounts = new[] { TestAccountFactory.NewAccount(clientId, AccountType.GIA) };

        var client = _mapper.MapFromEntity(entity, accounts);

        var expectedClient = new Client(ClientId.From(clientId), username, Address.Verified(), accounts);
        client.Should().BeEquivalentTo(expectedClient);
    }

    [Test]
    public void MapsDomainToEntity()
    {
        var clientId = Guid.NewGuid();
        const string username = "lewis";
        var client = new Client(ClientId.From(clientId), username, Address, Enumerable.Empty<InvestmentAccount>());

        var entity = _mapper.MapToEntity(client);

        var expectedEntity = new ClientEntity
        {
            ClientId = clientId,
            Username = username,
            AddressLine1 = "line1",
            AddressLine2 = "line2",
            AddressPostcode = "postcode",
            AddressIsVerified = false,
        };
        entity.Should().BeEquivalentTo(expectedEntity);
    }
}
