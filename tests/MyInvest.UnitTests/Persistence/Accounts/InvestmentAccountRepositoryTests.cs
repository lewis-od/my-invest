using System;
using System.Linq;
using FluentAssertions;
using Moq;
using MyInvest.Domain.Accounts;
using MyInvest.Domain.Clients;
using MyInvest.Persistence.Accounts;
using MyInvest.UnitTests.Domain.Accounts;
using NUnit.Framework;

namespace MyInvest.UnitTests.Persistence.Accounts;

public class InvestmentAccountRepositoryTests
{
    private readonly Mock<IInvestmentAccountDao> _accountDao = new();
    private readonly Mock<IInvestmentAccountEntityMapper> _accountMapper = new();

    private readonly InvestmentAccountRepository _accountRepository;

    public InvestmentAccountRepositoryTests()
    {
        _accountRepository = new InvestmentAccountRepository(_accountDao.Object, _accountMapper.Object);
    }

    [Test]
    public void GetsAllAccounts()
    {
        var accountEntity = new InvestmentAccountEntity();
        _accountDao.Setup(dao => dao.GetAll()).Returns(Enumerable.Repeat(accountEntity, 1));
        var account = TestAccountFactory.NewAccount();
        _accountMapper.Setup(mapper => mapper.MapFromEntity(accountEntity)).Returns(account);

        var allAccounts = _accountRepository.GetAll().ToList();

        allAccounts.Should().HaveCount(1);
        allAccounts.Should().Contain(account);
    }

    [Test]
    public void GetsAccountByAccountId()
    {
        var accountGuid = Guid.NewGuid();
        var accountEntity = new InvestmentAccountEntity();
        _accountDao.Setup(dao => dao.GetById(accountGuid)).Returns(accountEntity);
        var investmentAccount = TestAccountFactory.NewAccount();
        _accountMapper.Setup(mapper => mapper.MapFromEntity(accountEntity)).Returns(investmentAccount);

        var accountId = AccountId.From(accountGuid);
        var retrievedAccount = _accountRepository.GetById(accountId);

        retrievedAccount.Should().Be(investmentAccount);
    }

    [Test]
    public void GetsAccountsByClientId()
    {
        var clientGuid = Guid.NewGuid();
        var accountEntity = new InvestmentAccountEntity();
        _accountDao.Setup(dao => dao.FindByClientId(clientGuid)).Returns(Enumerable.Repeat(accountEntity, 1));
        var account = TestAccountFactory.NewAccount();
        _accountMapper.Setup(mapper => mapper.MapFromEntity(accountEntity)).Returns(account);

        var clientId = ClientId.From(clientGuid);
        var retrievedAccounts = _accountRepository.FindByClientId(clientId).ToList();

        retrievedAccounts.Should().HaveCount(1);
        retrievedAccounts.Should().Contain(account);
    }

    [Test]
    public void CreatesNewAccount()
    {
        var newAccount = TestAccountFactory.NewAccount();
        var newAccountEntity = new InvestmentAccountEntity();
        _accountMapper.Setup(mapper => mapper.MapToEntity(newAccount)).Returns(newAccountEntity);
        
        _accountRepository.Create(newAccount);
        
        _accountDao.Verify(dao => dao.CreateAccount(newAccountEntity));
    }
}
