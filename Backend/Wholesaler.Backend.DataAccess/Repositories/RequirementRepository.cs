using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Wholesaler.Backend.DataAccess.Factories;
using Wholesaler.Backend.Domain.Entities;
using Wholesaler.Backend.Domain.Exceptions;
using Wholesaler.Backend.Domain.Repositories;
using RequirementDb = Wholesaler.Backend.DataAccess.Models.Requirement;

namespace Wholesaler.Backend.DataAccess.Repositories;

public class RequirementRepository : IRequirementRepository
{
    private readonly IRequirementDbFactory _factory;
    private readonly WholesalerContext _context;

    public RequirementRepository(IRequirementDbFactory factory, WholesalerContext context)
    {
        _factory = factory;
        _context = context;
    }

    public Requirement Add(Requirement requirement)
    {
        var requirementDb = new RequirementDb()
        {
            Id = requirement.Id,
            Quantity = requirement.Quantity,
            ClientId = requirement.ClientId,
            StorageId = requirement.StorageId
        };

        _context.Add(requirementDb);
        _context.SaveChanges();

        return requirement;
    }

    public List<Requirement> GetAll()
    {
        return GetRequirementsBase()
            .ToList()
            .ConvertAll(_factory.Create);
    }

    public List<Requirement> Get(Guid storageId)
    {
        var requirementsDb = _context.Requirements
            .Where(r => r.StorageId == storageId)
            .ToList();

        return requirementsDb == null
            ? new()
            : requirementsDb
            .ConvertAll(_factory.Create);
    }

    public Requirement? GetOrDefault(Guid id)
    {
        var requirementDb = GetRequirementsBase()
            .FirstOrDefault(r => r.Id == id);

        return requirementDb == null
            ? default
            : _factory.Create(requirementDb);
    }

    public Requirement Update(Requirement requirement)
    {
        var requirementDb = _context.Requirements
            .FirstOrDefault(r => r.Id == requirement.Id)
            ?? throw new EntityNotFoundException($"There is no requirement with id {requirement.Id}");

        requirementDb.Quantity = requirement.Quantity;
        requirementDb.ClientId = requirement.ClientId;
        requirementDb.StorageId = requirement.StorageId;
        requirementDb.Status = requirement.Status;
        requirementDb.DeliveryDate = requirement.DeliveryDate;
        _context.SaveChanges();

        return requirement;
    }

    public List<Requirement> GetByStatus(string status)
    {
        var statusName = PrepareStatusName(status);

        if (!Enum.TryParse(statusName, out Status requirementStatus))
            throw new InvalidDataProvidedException("You entered an invalid value of status.");

        var requirementsDb = GetRequirementsBase()
            .Where(r => r.Status == requirementStatus)
            .ToList();

        return requirementsDb
            .ConvertAll(_factory.Create);
    }

    public Task<List<Requirement>> GetByCustomFiltersAsync(Dictionary<string, string> customFilters)
    {
        var query = GetRequirementsBase();

        foreach (var filter in customFilters)
        {
            query = query
                .Where($"{filter.Key} == @0", filter.Value);
        }

        return query
            .Select(requirementDb => _factory.Create(requirementDb))
            .ToListAsync();
    }

    private static string PrepareStatusName(string status)
    {
        return char.ToUpper(status[0])
            + status.Substring(1)
            .ToLower();
    }

    private IQueryable<RequirementDb> GetRequirementsBase()
    {
        return _context.Requirements
            .Include(r => r.Client)
            .Include(r => r.Storage);
    }
}
