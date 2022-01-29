namespace MyInvest.Domain.Id;

public interface IUniqueIdGenerator<out T> where T : UniqueId
{
    T Generate();
}