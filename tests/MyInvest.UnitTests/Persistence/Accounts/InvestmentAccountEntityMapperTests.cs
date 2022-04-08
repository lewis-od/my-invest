using System;
using AutoMapper;
using FluentAssertions;
using MyInvest.Domain.Accounts;
using MyInvest.Persistence;
using MyInvest.Persistence.Accounts;
using MyInvest.UnitTests.Domain.Ids;
using NUnit.Framework;

namespace MyInvest.UnitTests.Persistence.Accounts;

public class InvestmentAccountEntityMapperTests
{
    private static readonly Guid AccountGuid = Guid.NewGuid();
    private static readonly Guid ClientGuid = Guid.NewGuid();
    private static readonly AccountFactory Factory = new(new FixedIdGenerator<AccountId>(AccountId.From(AccountGuid)));
        
    private readonly InvestmentAccountEntityMapper _mapper;

    public InvestmentAccountEntityMapperTests()
    {
        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new PersistenceMapperProfile()));
        _mapper = new InvestmentAccountEntityMapper(mapperConfig.CreateMapper(), Factory);
    }

    [Test]
    public void MapsDomainToEntity()
    {
        var domainAccount = Factory.CreateAccount(AccountId.From(AccountGuid), ClientGuid, AccountType.GIA, AccountStatus.Open, 15.0m);

        var accountEntity = _mapper.MapToEntity(domainAccount);

        var expectedEntity = new InvestmentAccountEntity
        {
            AccountId = AccountGuid,
            ClientId = ClientGuid,
            AccountType = "GIA",
            AccountStatus = "Open",
            Balance = 15.0m,
        };
        accountEntity.Should().BeEquivalentTo(expectedEntity);
    }

    [Test]
    public void MapsEntityToDomain()
    {
        var entity = new InvestmentAccountEntity
        {
            AccountId = AccountGuid,
            ClientId = ClientGuid,
            AccountType = "GIA",
            AccountStatus = "Open",
            Balance = 15.0m,
        };

        var account = _mapper.MapFromEntity(entity);
        
        var expectedAccount = Factory.CreateAccount(AccountId.From(AccountGuid), ClientGuid, AccountType.GIA, AccountStatus.Open, 15.0m);
        account.Should().BeEquivalentTo(expectedAccount);
    }
}
