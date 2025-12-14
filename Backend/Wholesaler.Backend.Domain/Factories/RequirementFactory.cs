using Wholesaler.Backend.Domain.Entities;
using Wholesaler.Backend.Domain.Exceptions;
using Wholesaler.Backend.Domain.Factories.Interfaces;
using Wholesaler.Backend.Domain.Requests.Requirements;

namespace Wholesaler.Backend.Domain.Factories;

public class RequirementFactory : IRequirementFactory
{
    public Requirement Create(CreateRequirementRequest request)
    {
        if (request.Quantity <= 0)
            throw new InvalidDataProvidedException("You need to provide quantity more than 0.");

        return new(
            request.Quantity,
            request.ClientId,
            request.StorageId);
    }
}
