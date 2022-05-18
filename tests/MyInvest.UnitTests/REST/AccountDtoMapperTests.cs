using System;
using AutoMapper;
using FluentAssertions;
using MyInvest.Domain.Accounts;
using MyInvest.REST;
using MyInvest.REST.Accounts;
using NUnit.Framework;

namespace MyInvest.UnitTests.REST;

public class AccountDtoMapperTests
{
    private readonly AccountDtoMapper _dtoMapper;

    public AccountDtoMapperTests()
    {
        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<RestMapperProfile>());
        _dtoMapper = new AccountDtoMapper(mapperConfig.CreateMapper());
    }

    [Test]
    public void MapsInvestmentAccountToDto()
    {
        var accountId = Guid.NewGuid();
        var clientId = Guid.NewGuid();
        var balance = 12.34m;
        var investmentAccount = new InvestmentAccount(AccountId.From(accountId), clientId, AccountType.GIA, AccountStatus.Open, balance);

        var actualDto = _dtoMapper.MapToDto(investmentAccount);

        var expectedDto = new AccountDto
            {AccountId = accountId, AccountType = AccountTypeDto.GIA, Status = AccountStatusDto.Open, Balance = balance, Savings = null};
        actualDto.Should().BeEquivalentTo(expectedDto);
    }

    [Test]
    public void MapsSavingsAccountToDto()
    {
        var accountId = Guid.NewGuid();
        var clientId = Guid.NewGuid();
        var balance = 12.34m;
        var savingsAllowance = 200.00m;
        var savingsContributions = 100.00m;
        var savingsAccount = new SavingsAccount(AccountId.From(accountId), clientId, AccountType.ISA, AccountStatus.Open, balance, savingsAllowance,
            savingsContributions);

        var actualDto = _dtoMapper.MapToDto(savingsAccount);

        var expectedDto = new AccountDto
        {
            AccountId = savingsAccount.AccountId,
            AccountType = AccountTypeDto.ISA,
            Status = AccountStatusDto.Open,
            Balance = balance,
            Savings = new SavingsDto {Allowance = savingsAllowance, Contributions = savingsContributions}
        };
        actualDto.Should().BeEquivalentTo(expectedDto);
    }
}
