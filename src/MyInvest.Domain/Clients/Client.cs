using MyInvest.Domain.Accounts;

namespace MyInvest.Domain.Clients;

public class Client
{
    public ClientId ClientId { get; }
    public string Username { get; }
    public PostalAddress Address { get; private set; }
    public IEnumerable<InvestmentAccount> InvestmentAccounts { get; }

    public Client(ClientId clientId, string username, PostalAddress postalAddress, IEnumerable<InvestmentAccount> investmentAccounts)
    {
        ClientId = clientId;
        Username = username;
        Address = postalAddress;
        InvestmentAccounts = investmentAccounts;
    }

    public void VerifyAddress()
    {
        Address = Address.Verified();
    }
}