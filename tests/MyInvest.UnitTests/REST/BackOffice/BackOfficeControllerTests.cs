using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MyInvest.Domain.BackOffice;
using MyInvest.Domain.Clients;
using MyInvest.REST.BackOffice;
using NUnit.Framework;

namespace MyInvest.UnitTests.REST.BackOffice;

public class BackOfficeControllerTests
{
    private readonly Mock<IBackOfficeService> _backOfficeService = new();

    private readonly BackOfficeController _controller;

    public BackOfficeControllerTests()
    {
        _controller = new BackOfficeController(_backOfficeService.Object);
    }

    [Test]
    public void VerifyAddressReturnsNoContentResponseOnSuccess()
    {
        var clientId = ClientId.From(Guid.NewGuid());
        _backOfficeService.Setup(s => s.VerifyAddress(clientId)).Verifiable();

        var response = _controller.VerifyClientAddress(clientId);

        _backOfficeService.Verify();
        response.Should().BeOfType<NoContentResult>();
    }

    [Test]
    public void VerifyAddressReturnsNotFoundErrorWhenClientIdInvalid()
    {
        var clientId = ClientId.From(Guid.NewGuid());
        _backOfficeService.Setup(s => s.VerifyAddress(clientId)).Throws<ClientDoesNotExistException>();

        var response = _controller.VerifyClientAddress(clientId);

        response.Should().BeOfType<NotFoundResult>();
    }
}
