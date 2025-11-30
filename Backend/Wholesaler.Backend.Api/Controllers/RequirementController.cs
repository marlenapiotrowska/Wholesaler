using Microsoft.AspNetCore.Mvc;
using Wholesaler.Backend.Api.Factories.Interfaces;
using Wholesaler.Backend.Domain.Interfaces;
using Wholesaler.Backend.Domain.Repositories;
using Wholesaler.Backend.Domain.Requests.Requirements;
using Wholesaler.Core.Dto.RequestModels;
using Wholesaler.Core.Dto.ResponseModels;

namespace Wholesaler.Backend.Api.Controllers;

[ApiController]
[Route("requirements")]
public class RequirementController : ControllerBase
{
    private readonly IRequirementService _service;
    private readonly IRequirementDtoFactory _factory;
    private readonly IRequirementRepository _requirementRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IStorageRepository _storageRepository;

    public RequirementController(
        IRequirementService service,
        IRequirementDtoFactory factory,
        IRequirementRepository requirementRepository,
        IClientRepository clientRepository,
        IStorageRepository storageRepository)
    {
        _service = service;
        _factory = factory;
        _requirementRepository = requirementRepository;
        _clientRepository = clientRepository;
        _storageRepository = storageRepository;
    }

    [HttpPost]
    public async Task<ActionResult<RequirementDto>> AddAsync([FromBody] AddRequirementRequestModel addRequirementRequest)
    {
        var client = _clientRepository.Get(addRequirementRequest.ClientId);
        var storage = _storageRepository.Get(addRequirementRequest.StorageId);

        var request = new CreateRequirementRequest(
            addRequirementRequest.Quantity,
            client.Id,
            storage.Id);

        var requirement = _service.Add(request);

        return _factory.Create(requirement);
    }

    [HttpPatch]
    [Route("{id}")]
    public async Task<ActionResult<RequirementDto>> EditQuantityAsync(Guid id, [FromBody] UdpateRequirementRequestModel updateRequirementRequest)
    {
        var editedRequirement = _service.EditQuantity(id, updateRequirementRequest.Quantity);
        return _factory.Create(editedRequirement);
    }

    [HttpPatch]
    [Route("{id}/actions/complete")]
    public async Task<ActionResult<RequirementDto>> CompleteAsync(Guid id)
    {
        var completedRequirement = _service.Complete(id);
        return _factory.Create(completedRequirement);
    }

    [HttpGet]
    public async Task<ActionResult<List<RequirementDto>>> GetAllAsync()
    {
        var requirements = _requirementRepository.GetAll();
        return requirements
            .ConvertAll(_factory.Create);
    }

    [HttpGet]
    [Route("withStorageId")]
    public async Task<ActionResult<List<RequirementDto>>> GetAsync([FromQuery] Guid storageId)
    {
        var requirements = _requirementRepository.Get(storageId);
        return requirements
            .ConvertAll(_factory.Create);
    }

    [HttpGet]
    [Route("byStatus")]
    public async Task<ActionResult<List<RequirementDto>>> GetByStatusAsync([FromQuery] string status)
    {
        var requirements = _requirementRepository.GetByStatus(status);
        return requirements
            .ConvertAll(_factory.Create);
    }

    [HttpGet]
    [Route("byCustomFilters")]
    public async Task<ActionResult<List<RequirementDto>>> GetByCustomFiltersAsync([FromQuery] Dictionary<string, string> customFilters)
    {
        var response = await _service.GetByCustomFiltersAsync(customFilters);

        return response.Errors.Count != 0
            ? BadRequest(response.Errors)
            : response.Requirements
            .ConvertAll(_factory.Create);
    }
}
