using Wholesaler.Backend.Domain.Entities;
using RequirementDb = Wholesaler.Backend.DataAccess.Models.Requirement;

namespace Wholesaler.Backend.DataAccess.Factories;

public class RequirementDbFactory : IRequirementDbFactory
{
    public Requirement Create(RequirementDb requirement)
    {
        return new(
            requirement.Id,
            requirement.Quantity,
            requirement.ClientId,
            (requirement.Client?.Name + " " + requirement.Client?.Surname) ?? string.Empty,
            requirement.StorageId,
            requirement.Storage?.Name ?? string.Empty,
            requirement.Status,
            requirement.DeliveryDate);
    }
}
