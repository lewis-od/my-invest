using FluentAssertions;
using MyInvest.Domain.Clients;
using NUnit.Framework;

namespace MyInvest.UnitTests.Domain.Clients;

public class PostalAddressTests
{
    [Test]
    public void StartsUnverified()
    {
        var address = new PostalAddress("line1", "line2", "postcode");
        address.IsVerified.Should().BeFalse();
    }

    [Test]
    public void CreatesAVerifiedCopy()
    {
        var unverifiedAddress = new PostalAddress("line1", "line2", "postcode");

        var verifiedAddress = unverifiedAddress.Verified();

        verifiedAddress.Line1.Should().Be(unverifiedAddress.Line1);
        verifiedAddress.Line2.Should().Be(unverifiedAddress.Line2);
        verifiedAddress.Postcode.Should().Be(unverifiedAddress.Postcode);
        verifiedAddress.IsVerified.Should().BeTrue();
    }
}
