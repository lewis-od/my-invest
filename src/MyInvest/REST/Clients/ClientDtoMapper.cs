using AutoMapper;
using MyInvest.Domain.Clients;

namespace MyInvest.REST.Clients;

public class ClientDtoMapper
{
    private readonly IMapper _mapper;

    public ClientDtoMapper(IMapper mapper)
    {
        _mapper = mapper;
    }

    public ClientDto MapToDto(Client client) => _mapper.Map<ClientDto>(client);
}