using MyInvest.Domain.Transactions;

namespace MyInvest.Domain.Accounts;

public interface ICashService
{
    void AddCashToAccount(AccountId accountId, Transaction transaction);
}

public class CashService : ICashService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionVerifier _transactionVerifier;
    
    public CashService(IAccountRepository accountRepository, ITransactionVerifier transactionVerifier)
    {
        _accountRepository = accountRepository;
        _transactionVerifier = transactionVerifier;
    }

    public void AddCashToAccount(AccountId accountId, Transaction transaction)
    {
        var account = GetAccountOrThrow(accountId);
        if (!_transactionVerifier.VerifyTransaction(transaction))
        {
            throw new InvalidTransactionException();
        }

        account.CreditBalance(transaction.Amount);
        _accountRepository.Update(account);
    }

    private InvestmentAccount GetAccountOrThrow(AccountId accountId)
    {
        var account = _accountRepository.GetById(accountId);
        if (account == null)
        {
            throw new AccountDoesNotExistException();
        }

        return account;
    }
}
