using Microsoft.AspNetCore.Mvc;
using Wholesaler.Backend.Api.Factories.Interfaces;
using Wholesaler.Backend.Domain.Interfaces;
using Wholesaler.Backend.Domain.Repositories;
using Wholesaler.Backend.Domain.Requests.Storage;
using Wholesaler.Core.Dto.RequestModels;
using Wholesaler.Core.Dto.ResponseModels;

namespace Wholesaler.Backend.Api.Controllers;

[ApiController]
[Route("storages")]

public class StorageController : ControllerBase
{
    private readonly IStorageService _service;
    private readonly IStorageDtoFactory _storageDtoFactory;
    private readonly IStorageRepository _repository;

    public StorageController(IStorageService serivce, IStorageDtoFactory storageDtoFactory, IStorageRepository repository)
    {
        _service = serivce;
        _storageDtoFactory = storageDtoFactory;
        _repository = repository;
    }

    [HttpPost]
    public async Task<ActionResult<StorageDto>> AddAsync([FromBody] AddStorageRequestModel addStorageRequestModel)
    {
        var request = new CreateStorageRequest(addStorageRequestModel.Name);

        var storage = _service.Add(request);
        return _storageDtoFactory.Create(storage);
    }

    [HttpGet]
    public async Task<ActionResult<List<StorageDto>>> GetAllAsync()
    {
        var storages = _repository.GetAll();
        return storages
            .ConvertAll(_storageDtoFactory.Create);
    }

    [HttpPatch]
    [Route("{id}/actions/deliver")]
    public async Task<ActionResult<StorageDto>> MushroomsDeliveryAsync(Guid id, [FromBody] UpdateStorageRequestModel updateStorageRequestModel)
    {
        var storageDelivery = _service.Deliver(id, updateStorageRequestModel.Quantity, updateStorageRequestModel.PersonId);
        return _storageDtoFactory.Create(storageDelivery);
    }
}
