using System;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using MyInvest.Domain.Accounts;
using MyInvest.Domain.Clients;
using MyInvest.UnitTests.Domain.Ids;
using NUnit.Framework;

namespace MyInvest.UnitTests.Domain.Accounts;

public class AccountOpeningServiceTests
{
    private readonly ClientId _clientId = ClientId.From(Guid.NewGuid());
    private readonly AccountId _accountId = AccountId.From(Guid.NewGuid());
    private readonly Mock<IAccountRepository> _accountRepository = new();
    private readonly Mock<IClientRepository> _clientRepository = new();

    private readonly AccountOpeningService _openingService;

    public AccountOpeningServiceTests()
    {
        var accountIdGenerator = new FixedIdGenerator<AccountId>(_accountId);
        var accountFactory = new AccountFactory(accountIdGenerator);

        _openingService = new AccountOpeningService(
            accountFactory,
            _accountRepository.Object,
            _clientRepository.Object,
            Mock.Of<ILogger<AccountOpeningService>>()
        );
    }

    [Test]
    public void CreatesNewInvestmentAccountForClientWithUnverifiedAddress()
    {
        GivenClientExistsWithUnverifiedAddress();
        GivenClientHasSipp();

        var openedAccount = _openingService.OpenAccount(_clientId, AccountType.GIA);

        var expectedAccount = new InvestmentAccount(_accountId, _clientId, AccountType.GIA, AccountStatus.PreOpen, 0.0m);
        openedAccount.Should().BeEquivalentTo(expectedAccount);
        _accountRepository.Verify(repo => repo.Create(It.Is<InvestmentAccount>(account => Equals(account.AccountId, expectedAccount.AccountId))));
    }
    
    [Test]
    public void CreatesNewInvestmentAccountForClientWithVerifiedAddress()
    {
        GivenClientExistsWithVerifiedAddress();
        GivenClientHasSipp();

        var openedAccount = _openingService.OpenAccount(_clientId, AccountType.GIA);

        var expectedAccount = new InvestmentAccount(_accountId, _clientId, AccountType.GIA, AccountStatus.Open, 0.0m);
        openedAccount.Should().BeEquivalentTo(expectedAccount);
        _accountRepository.Verify(repo => repo.Create(It.Is<InvestmentAccount>(account => Equals(account.AccountId, expectedAccount.AccountId))));
    }

    [Test]
    public void ThrowsExceptionIfClientAlreadyHasAccountOfSameType()
    {
        GivenClientExists();
        GivenClientHasSipp();

        Assert.Throws<ClientAlreadyOwnsAccountException>(() => _openingService.OpenAccount(_clientId, AccountType.SIPP));
    }

    private void GivenClientExistsWithUnverifiedAddress() => GivenClientExists(false);
    
    private void GivenClientExistsWithVerifiedAddress() => GivenClientExists(true);

    private void GivenClientExists(bool addressIsVerified = false)
    {
        var postalAddress = new PostalAddress("some", "dummy", "address");
        if (addressIsVerified)
        {
            postalAddress = postalAddress.Verified();
        }

        var client = new Client(_clientId, "username", postalAddress, Enumerable.Empty<InvestmentAccount>());
        _clientRepository.Setup(repo => repo.GetById(_clientId)).Returns(client);
    }

    private void GivenClientHasSipp()
    {
        var sipp = new InvestmentAccount(AccountId.From(Guid.NewGuid()), _clientId, AccountType.SIPP, AccountStatus.Open, 100.0m);
        _accountRepository.Setup(repo => repo.FindByClientId(_clientId)).Returns(new[] {sipp});
    }

    [Test]
    public void ThrowsExceptionIfClientDoesntExist()
    {
        _clientRepository.Setup(repo => repo.GetById(_clientId)).Returns((Client?) null);

        Assert.Throws<ClientDoesNotExistException>(() => _openingService.OpenAccount(_clientId, AccountType.GIA));
    }
}
