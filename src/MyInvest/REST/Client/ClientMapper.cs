using AutoMapper;

namespace MyInvest.REST.Client;

public class ClientMapper
{
    private readonly IMapper _mapper;

    public ClientMapper(IMapper mapper)
    {
        _mapper = mapper;
    }

    public ClientDto MapToDto(Domain.Client.Client client) => _mapper.Map<ClientDto>(client);
}