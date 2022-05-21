namespace MyInvest.REST;

public interface IDtoMapper<in TDomainType, out TDtoType>
{
    TDtoType MapToDto(TDomainType domainObject);
}
