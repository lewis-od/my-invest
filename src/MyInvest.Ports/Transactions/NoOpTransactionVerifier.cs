using MyInvest.Domain.Transactions;

namespace MyInvest.Ports.Transactions;

public class NoOpTransactionVerifier : ITransactionVerifier
{
    public bool VerifyTransaction(Transaction transaction) => true;
}
