using System;
using AutoMapper;
using FluentAssertions;
using MyInvest.Domain.Accounts;
using MyInvest.Domain.Clients;
using MyInvest.REST;
using MyInvest.REST.Clients;
using MyInvest.UnitTests.Domain.Accounts;
using MyInvest.UnitTests.Utils;
using NUnit.Framework;

namespace MyInvest.UnitTests.REST;

public class ClientDtoMapperTests
{
    private readonly ClientDtoMapper _dtoMapper;

    public ClientDtoMapperTests()
    {
        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<RestMapperProfile>());
        _dtoMapper = new ClientDtoMapper(mapperConfig.CreateMapper());
    }

    [Test]
    public void MapsClientToDto()
    {
        var clientId = ClientId.From(Guid.NewGuid());
        var account = TestAccountFactory.NewAccount(clientId, AccountType.GIA);
        var client = new Client(clientId, "lewis", FakeData.Address, new[] {account});

        var actualDto = _dtoMapper.MapToDto(client);
        
        var expectedDto = new ClientDto
        {
            ClientId = clientId.Value,
            Username = "lewis",
        };
        // Don't assert on InvestmentAccounts as account mapping functionality is covered by AccountDtoMapperTests
        actualDto.Should().BeEquivalentTo(expectedDto, opts => opts.Excluding(c => c.InvestmentAccounts));
        actualDto.InvestmentAccounts.Should().HaveCount(1);
    }

    [Test]
    public void MapsPostalAddressDtoToPostalAddress()
    {
        const string line1 = "line1";
        const string line2 = "line2";
        const string postcode = "postcode";
        var addressDto = new PostalAddressDto
        {
            Line1 = line1,
            Line2 = line2,
            Postcode = postcode
        };

        var actualAddress = _dtoMapper.MapToDomain(addressDto);

        var expectedAddress = new PostalAddress(line1, line2, postcode);
        actualAddress.Should().Be(expectedAddress);
    }
}
