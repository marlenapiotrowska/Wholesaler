using Wholesaler.Backend.Api.Factories.Interfaces;
using Wholesaler.Backend.Domain.Entities;
using Wholesaler.Core.Dto.ResponseModels;

namespace Wholesaler.Backend.Api.Factories;

public class RequirementDtoFactory : IRequirementDtoFactory
{
    public RequirementDto Create(Requirement requirement)
    {
        return new()
        {
            Id = requirement.Id,
            Quantity = requirement.Quantity,
            ClientId = requirement.ClientId,
            ClientName = requirement.ClientName,
            StorageId = requirement.StorageId,
            StorageName = requirement.StorageName,
            Status = requirement.Status.ToString(),
            DeliveryDate = requirement.DeliveryDate
        };
    }
}
