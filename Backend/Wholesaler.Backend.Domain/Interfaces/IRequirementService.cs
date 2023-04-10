﻿using Wholesaler.Backend.Domain.Entities;
using Wholesaler.Backend.Domain.Requests.Requirements;

namespace Wholesaler.Backend.Domain.Interfaces
{
    public interface IRequirementService
    {
        Requirement Add(CreateRequirementRequest request);
    }
}
