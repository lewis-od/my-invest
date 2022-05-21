using MyInvest.Domain.Accounts;
using MyInvest.Domain.Clients;

namespace MyInvest.Domain.BackOffice;

public class BackOfficeService
{
    private readonly IClientRepository _clientRepository;
    private readonly IAccountRepository _accountRepository;
    
    public BackOfficeService(IClientRepository clientRepository, IAccountRepository accountRepository)
    {
        _clientRepository = clientRepository;
        _accountRepository = accountRepository;
    }

    public void VerifyAddress(ClientId clientId)
    {
        var client = GetClient(clientId);
        UpdateAddress(client);
        OpenAccounts(client);
    }

    private Client GetClient(ClientId clientId)
    {
        var client = _clientRepository.GetById(clientId);
        if (client == null)
        {
            throw new ClientDoesNotExistException();
        }

        return client;
    }
    
    private void UpdateAddress(Client client)
    {
        client.VerifyAddress();
        _clientRepository.Update(client);
    }
    
    private void OpenAccounts(Client client)
    {
        foreach (var account in client.InvestmentAccounts)
        {
            account.OpenAccount();
            _accountRepository.Update(account);
        }
    }
}
