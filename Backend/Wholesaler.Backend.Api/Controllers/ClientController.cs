using Microsoft.AspNetCore.Mvc;
using Wholesaler.Backend.Api.Factories.Interfaces;
using Wholesaler.Backend.Domain.Interfaces;
using Wholesaler.Backend.Domain.Repositories;
using Wholesaler.Backend.Domain.Requests.People;
using Wholesaler.Core.Dto.RequestModels;
using Wholesaler.Core.Dto.ResponseModels;

namespace Wholesaler.Backend.Api.Controllers;

[ApiController]
[Route("clients")]
public class ClientController : ControllerBase
{
    private readonly IClientService _clientService;
    private readonly IClientFactory _clientFactory;
    private readonly IClientRepository _clientRepository;

    public ClientController(IClientService clientService, IClientFactory clientFactory, IClientRepository clientRepository)
    {
        _clientService = clientService;
        _clientFactory = clientFactory;
        _clientRepository = clientRepository;
    }

    [HttpPost]
    public async Task<ActionResult<ClientDto>> AddAsync([FromBody] AddClientRequestModel addClientRequest)
    {
        var request = new CreateClientRequest(
            addClientRequest.Name,
            addClientRequest.Surname);

        var client = _clientService.Add(request);

        return _clientFactory.Create(client);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        _clientService.Delete(id);

        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<List<ClientDto>>> GetAllAsync()
    {
        var clients = _clientRepository.GetAll();

        return clients
            .ConvertAll(_clientFactory.Create);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<ClientDto>> GetAsync(Guid id)
    {
        var client = _clientRepository.Get(id);

        return _clientFactory.Create(client);
    }
}
