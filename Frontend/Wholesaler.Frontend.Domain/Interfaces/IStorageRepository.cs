using Wholesaler.Core.Dto.ResponseModels;
using Wholesaler.Frontend.Domain.ValueObjects;

namespace Wholesaler.Frontend.Domain.Interfaces;

public interface IStorageRepository
{
    Task<ExecutionResultGeneric<StorageDto>> AddAsync(string name);
    Task<ExecutionResultGeneric<List<StorageDto>>> GetAllStoragesAsync();
    Task<ExecutionResultGeneric<StorageDto>> DeliverAsync(Guid id, int quantity, Guid personId);
}
