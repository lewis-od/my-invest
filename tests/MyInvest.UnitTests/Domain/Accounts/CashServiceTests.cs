using System;
using System.Security.Principal;
using FluentAssertions;
using Moq;
using MyInvest.Domain.Accounts;
using MyInvest.Domain.Transactions;
using NUnit.Framework;

namespace MyInvest.UnitTests.Domain.Accounts;

public class CashServiceTests
{
    private readonly Mock<IAccountRepository> _accountRepository = new();
    private readonly Mock<ITransactionVerifier> _transactionVerifier = new();
    
    private readonly CashService _cashService;

    public CashServiceTests()
    {
        _cashService = new CashService(_accountRepository.Object, _transactionVerifier.Object);
    }
        
    [Test]
    public void AddCashIncreasesAccountBalanceWhenTransactionIsValid()
    {
        var transaction = new Transaction
        {
            TransactionId = TransactionId.From(Guid.NewGuid()),
            MessageAuthenticationCode = "foo",
            Amount = 150.0m,
        };
        GivenTransactionIsValid(transaction, true);
        var accountId = GivenAccountExists();

        InvestmentAccount? updatedAccount = null;
        _accountRepository.Setup(r => r.Update(It.IsAny<InvestmentAccount>()))
            .Callback<InvestmentAccount>(acc => updatedAccount = acc);
        
        _cashService.AddCashToAccount(accountId, transaction);
        
        Assert.NotNull(updatedAccount);
        updatedAccount!.Balance.Should().Be(transaction.Amount);
    }

    [Test]
    public void AddCashThrowsExceptionWhenTransactionIsInvalid()
    {
        var transaction = new Transaction
        {
            TransactionId = TransactionId.From(Guid.NewGuid()),
            MessageAuthenticationCode = "foo",
            Amount = 150.0m,
        };
        GivenTransactionIsValid(transaction, false);
        var accountId = GivenAccountExists();

        Assert.Throws<InvalidTransactionException>(() => _cashService.AddCashToAccount(accountId, transaction));
    }

    [Test]
    public void AddCashThrowsExceptionIfAccountDoesntExist()
    {
        var accountId = AccountId.From(Guid.NewGuid());
        _accountRepository.Setup(r => r.GetById(accountId))
            .Returns<InvestmentAccount?>(null);

        Assert.Throws<AccountDoesNotExistException>(() => _cashService.AddCashToAccount(accountId, new Transaction()));
    }

    private AccountId GivenAccountExists()
    {
        var existingAccount = TestAccountFactory.NewAccount();
        var accountId = existingAccount.AccountId;
        _accountRepository.Setup(r => r.GetById(accountId))
            .Returns(existingAccount);
        return accountId;
    }

    private void GivenTransactionIsValid(Transaction transaction, bool isValid)
    {
        _transactionVerifier.Setup(v => v.VerifyTransaction(transaction))
            .Returns(isValid);
    }
}
