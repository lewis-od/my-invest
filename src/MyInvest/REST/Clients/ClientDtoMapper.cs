using AutoMapper;
using MyInvest.Domain.Clients;

namespace MyInvest.REST.Clients;

public class ClientDtoMapper : IDtoMapper<Client, ClientDto>
{
    private readonly IMapper _mapper;

    public ClientDtoMapper(IMapper mapper)
    {
        _mapper = mapper;
    }

    public ClientDto MapToDto(Client client) => _mapper.Map<ClientDto>(client);

    public PostalAddress MapToDomain(PostalAddressDto dto) => new(dto.Line1, dto.Line2, dto.Postcode);
}