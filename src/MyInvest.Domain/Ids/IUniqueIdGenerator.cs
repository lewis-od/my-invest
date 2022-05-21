namespace MyInvest.Domain.Ids;

public interface IUniqueIdGenerator<out T> where T : UniqueId
{
    T Generate();
}