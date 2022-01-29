using MyInvest.Domain.Account;

namespace MyInvest.Domain.Client;

public class Client
{
    public ClientId ClientId { get; }
    public string Username { get; }
    public IEnumerable<InvestmentAccount> InvestmentAccounts { get; }

    public Client(ClientId clientId, string username, IEnumerable<InvestmentAccount> investmentAccounts)
    {
        ClientId = clientId;
        Username = username;
        InvestmentAccounts = investmentAccounts;
    }
}