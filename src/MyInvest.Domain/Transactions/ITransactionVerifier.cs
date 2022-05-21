namespace MyInvest.Domain.Transactions;

public interface ITransactionVerifier
{
    bool VerifyTransaction(Transaction transaction);
}
