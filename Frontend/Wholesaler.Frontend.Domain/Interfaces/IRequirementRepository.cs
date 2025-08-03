using Wholesaler.Core.Dto.ResponseModels;
using Wholesaler.Frontend.Domain.ValueObjects;

namespace Wholesaler.Frontend.Domain.Interfaces;

public interface IRequirementRepository
{
    Task<ExecutionResultGeneric<RequirementDto>> AddAsync(int quantity, Guid clientId, Guid storageId);
    Task<ExecutionResultGeneric<RequirementDto>> EditQuantityAsync(Guid id, int quantity);
    Task<ExecutionResultGeneric<List<RequirementDto>>> GetAllRequirementsAsync();
    Task<ExecutionResultGeneric<List<RequirementDto>>> GetRequirementsAsync(Guid storageId);
    Task<ExecutionResultGeneric<RequirementDto>> CompleteRequirementAsync(Guid id);
    Task<ExecutionResultGeneric<List<RequirementDto>>> GetRequirementsByStatusAsync(string status);
}
