using System;
using FluentAssertions;
using Moq;
using MyInvest.Domain.Account;
using MyInvest.Domain.Client;
using MyInvest.UnitTests.Domain.Id;
using NUnit.Framework;

namespace MyInvest.UnitTests.Domain.Account;

public class AccountOpeningServiceTests
{
    private readonly ClientId _clientId = ClientId.From(Guid.NewGuid());
    private readonly AccountId _accountId = AccountId.From(Guid.NewGuid());
    private readonly Mock<IAccountRepository> _accountRepository = new();

    private readonly AccountOpeningService _openingService;

    public AccountOpeningServiceTests()
    {
        var accountIdGenerator = new FixedIdGenerator<AccountId>(_accountId);
        var accountFactory = new AccountFactory(accountIdGenerator);

        _openingService = new AccountOpeningService(accountFactory, _accountRepository.Object);
    }

    [Test]
    public void CreatesNewInvestmentAccountForClient()
    {
        var sipp = new InvestmentAccount(AccountId.From(Guid.NewGuid()), _clientId, AccountType.SIPP, AccountStatus.Open, 100.0m);
        _accountRepository.Setup(repo => repo.FindByClientId(_clientId)).Returns(new[] {sipp});

        var openedAccount = _openingService.OpenAccount(_clientId, AccountType.GIA);

        var expectedAccount = new InvestmentAccount(_accountId, _clientId, AccountType.GIA, AccountStatus.PreOpen, 0.0m);
        openedAccount.Should().BeEquivalentTo(expectedAccount);
        _accountRepository.Verify(repo => repo.Create(It.Is<InvestmentAccount>( account => Equals(account.AccountId, expectedAccount.AccountId))));
    }

    [Test]
    public void ThrowsExceptionIfClientAlreadyHasAccountOfSameType()
    {
        var accountType = AccountType.SIPP;
        var sipp = new InvestmentAccount(AccountId.From(Guid.NewGuid()), _clientId, accountType, AccountStatus.Open, 100.0m);
        _accountRepository.Setup(repo => repo.FindByClientId(_clientId)).Returns(new[] {sipp});

        Assert.Throws<ClientAlreadyOwnsAccountException>(() => _openingService.OpenAccount(_clientId, accountType));
    }
}
