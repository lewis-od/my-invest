using AutoMapper;
using MyInvest.Domain.Clients;

namespace MyInvest.REST.Clients;

public class ClientMapper
{
    private readonly IMapper _mapper;

    public ClientMapper(IMapper mapper)
    {
        _mapper = mapper;
    }

    public ClientDto MapToDto(Client client) => _mapper.Map<ClientDto>(client);
}