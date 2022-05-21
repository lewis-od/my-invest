using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using MyInvest.Domain.Accounts;
using MyInvest.Domain.BackOffice;
using MyInvest.Domain.Clients;
using MyInvest.UnitTests.Domain.Accounts;
using MyInvest.UnitTests.Utils;
using NUnit.Framework;

namespace MyInvest.UnitTests.Domain.BackOffice;

public class BackOfficeServiceTests
{
    private readonly Mock<IClientRepository> _clientRepository = new();
    private readonly Mock<IAccountRepository> _accountRepository = new();

    private readonly BackOfficeService _service;

    public BackOfficeServiceTests()
    {
        _service = new BackOfficeService(_clientRepository.Object, _accountRepository.Object);
    }

    [Test]
    public void VerifyAddressMarksAddressAsVerified()
    {
        var clientId = ClientId.From(Guid.NewGuid());
        GivenClientWithAccountsExists(clientId, Enumerable.Empty<InvestmentAccount>());

        PostalAddress? updatedAddress = null;
        _clientRepository.Setup(r => r.Update(It.IsAny<Client>())).Callback<Client>(c => updatedAddress = c.Address);
        
        _service.VerifyAddress(clientId);

        Assert.IsTrue(updatedAddress?.IsVerified);
    }

    [Test]
    public void VerifyAddressSetsAccountStatusesToOpen()
    {
        var clientId = ClientId.From(Guid.NewGuid());
        var gia = TestAccountFactory.NewAccount(clientId, AccountType.GIA);
        var sipp = TestAccountFactory.NewAccount(clientId, AccountType.SIPP);
        GivenClientWithAccountsExists(clientId, new[] {gia, sipp});

        var updatedAccounts = new List<InvestmentAccount>();
        _accountRepository.Setup(r => r.Update(It.IsAny<InvestmentAccount>())).Callback<InvestmentAccount>(a => updatedAccounts.Add(a));

        _service.VerifyAddress(clientId);
        
        updatedAccounts.Should().HaveCount(2);
        Assert.IsTrue(updatedAccounts.Select(a => a.AccountStatus).All(status => status == AccountStatus.Open));
    }

    [Test]
    public void VerifyAddressThrowsExceptionIfClientDoesntExist()
    {
        ClientId clientId = ClientId.From(Guid.NewGuid());
        _clientRepository.Setup(r => r.GetById(clientId)).Returns<Client>(null);

        Assert.Throws<ClientDoesNotExistException>(() => _service.VerifyAddress(clientId));
    }

    private void GivenClientWithAccountsExists(ClientId clientId, IEnumerable<InvestmentAccount> accounts)
    {
        var client = new Client(clientId, "lewis", FakeData.Address, accounts);
        _clientRepository.Setup(r => r.GetById(clientId)).Returns(client);
    }
}
