using System;
using FluentAssertions;
using Moq;
using MyInvest.Domain.Accounts;
using MyInvest.Domain.Transactions;
using NUnit.Framework;

namespace MyInvest.UnitTests.Domain.Accounts;

public class CashServiceTests
{
    private const decimal TransactionAmount = 150.0m;
    
    private readonly Mock<IAccountRepository> _accountRepository = new();
    private readonly Mock<ITransactionVerifier> _transactionVerifier = new();
    
    private readonly CashService _cashService;

    public CashServiceTests()
    {
        _cashService = new CashService(_accountRepository.Object, _transactionVerifier.Object);
    }
        
    [Test]
    public void AddCashIncreasesAccountBalanceWhenTransactionIsValidAndAccountIsOpen()
    {
        var transaction = GivenTransactionIsValid(true);
        var accountId = GivenAccountWithStatusExists(AccountStatus.Open);

        InvestmentAccount? updatedAccount = null;
        _accountRepository.Setup(r => r.Update(It.IsAny<InvestmentAccount>()))
            .Callback<InvestmentAccount>(acc => updatedAccount = acc);
        
        _cashService.AddCashToAccount(accountId, transaction);
        
        Assert.NotNull(updatedAccount);
        updatedAccount!.Balance.Should().Be(TransactionAmount);
    }

    [Test]
    public void AddCashThrowsExceptionWhenTransactionIsInvalid()
    {
        var transaction = GivenTransactionIsValid(false);
        var accountId = GivenAccountWithStatusExists(AccountStatus.Open);

        Assert.Throws<InvalidTransactionException>(() => _cashService.AddCashToAccount(accountId, transaction));
    }

    [Test]
    public void AddCashThrowsExceptionWhenAccountIsNotOpen([Values(AccountStatus.PreOpen, AccountStatus.Closed)] AccountStatus accountStatus)
    {
        var transaction = GivenTransactionIsValid(true);
        var accountId = GivenAccountWithStatusExists(accountStatus);

        Assert.Throws<AccountNotOpenException>(() => _cashService.AddCashToAccount(accountId, transaction));
    }
    
    private AccountId GivenAccountWithStatusExists(AccountStatus status)
    {
        var existingAccount = TestAccountFactory.InvestmentAccountWithStatus(status);
        var accountId = existingAccount.AccountId;
        _accountRepository.Setup(r => r.GetById(accountId))
            .Returns(existingAccount);
        return accountId;
    }

    private Transaction GivenTransactionIsValid(bool isValid)
    {
        var transaction = new Transaction
        {
            TransactionId = TransactionId.From(Guid.NewGuid()),
            MessageAuthenticationCode = "foo",
            Amount = TransactionAmount,
        };
        _transactionVerifier.Setup(v => v.VerifyTransaction(transaction))
            .Returns(isValid);
        return transaction;
    }

    [Test]
    public void AddCashThrowsExceptionIfAccountDoesntExist()
    {
        var accountId = AccountId.From(Guid.NewGuid());
        _accountRepository.Setup(r => r.GetById(accountId))
            .Returns<InvestmentAccount?>(null);

        Assert.Throws<AccountDoesNotExistException>(() => _cashService.AddCashToAccount(accountId, new Transaction()));
    }
}
