namespace MyInvest.Persistence;

public interface IEntityMapper<TEntity, TDomain>
{
    TEntity MapToEntity(TDomain domainObject);
    TDomain MapFromEntity(TEntity entity);
}
