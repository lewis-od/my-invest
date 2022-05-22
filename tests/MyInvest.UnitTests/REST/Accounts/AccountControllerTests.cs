using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using MyInvest.Domain.Accounts;
using MyInvest.Domain.Transactions;
using MyInvest.REST;
using MyInvest.REST.Accounts;
using NUnit.Framework;

namespace MyInvest.UnitTests.REST.Accounts;

public class AccountControllerTests
{
    private const string MAC = "message auth code";
    private const decimal Amount = 123.0m;

    private readonly Mock<IAccountRepository> _accountRepository = new();
    private readonly Mock<IAccountOpeningService> _accountOpeningService = new();
    private readonly Mock<ICashService> _cashService = new();
    private readonly Mock<IDtoMapper<InvestmentAccount, AccountDto>> _accountDtoMapper = new();
    private readonly ILogger<AccountController> _logger = new NullLogger<AccountController>();

    private readonly AccountController _controller;

    public AccountControllerTests()
    {
        _controller = new AccountController(
            _accountRepository.Object,
            _accountOpeningService.Object,
            _cashService.Object,
            _accountDtoMapper.Object,
            _logger);
    }

    [Test]
    public void AddCashReturnsNoContentResponseOnSuccess()
    {
        var accountId = AccountId.From(Guid.NewGuid());
        var transaction = NewTransaction();
        _cashService.Setup(cs => cs.AddCashToAccount(accountId, transaction)).Verifiable();

        var request = AddCashRequest(transaction.TransactionId);
        var response = _controller.AddCash(accountId, request);

        _cashService.Verify();
        response.Should().BeOfType<NoContentResult>();
    }

    [Test]
    public void AddCashReturnsNotFoundWhenAccountDoesntExist()
    {
        var accountId = AccountId.From(Guid.NewGuid());
        var transaction = NewTransaction();
        _cashService.Setup(cs => cs.AddCashToAccount(accountId, transaction)).Throws<AccountDoesNotExistException>();

        var request = AddCashRequest(transaction.TransactionId);
        var response = _controller.AddCash(accountId, request);

        response.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public void AddCashReturnsBadRequestWhenTransactionIsInvalid()
    {
        var accountId = AccountId.From(Guid.NewGuid());
        var transaction = NewTransaction();
        _cashService.Setup(cs => cs.AddCashToAccount(accountId, transaction)).Throws<InvalidTransactionException>();

        var request = AddCashRequest(transaction.TransactionId);
        var response = _controller.AddCash(accountId, request);

        response.Should().BeOfType<BadRequestResult>();
    }
    
    [Test]
    public void AddCashReturnsConflictWhenAccountIsNotOpen()
    {
        var accountId = AccountId.From(Guid.NewGuid());
        var transaction = NewTransaction();
        _cashService.Setup(cs => cs.AddCashToAccount(accountId, transaction)).Throws<AccountNotOpenException>();

        var request = AddCashRequest(transaction.TransactionId);
        var response = _controller.AddCash(accountId, request);

        response.Should().BeOfType<ConflictResult>();
    }

    private static Transaction NewTransaction() =>
        new()
        {
            TransactionId = TransactionId.From(Guid.NewGuid()),
            MessageAuthenticationCode = MAC,
            Amount = Amount,
        };

    private static AddCashRequestDto AddCashRequest(TransactionId transactionId) =>
        new()
        {
            TransactionId = transactionId,
            MAC = MAC,
            Amount = Amount,
        };
}
