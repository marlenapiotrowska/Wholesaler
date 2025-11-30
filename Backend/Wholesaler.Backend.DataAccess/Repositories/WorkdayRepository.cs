using Microsoft.EntityFrameworkCore;
using Wholesaler.Backend.DataAccess.Factories;
using Wholesaler.Backend.Domain.Entities;
using Wholesaler.Backend.Domain.Exceptions;
using Wholesaler.Backend.Domain.Repositories;
using WorkdayDb = Wholesaler.Backend.DataAccess.Models.Workday;

namespace Wholesaler.Backend.DataAccess.Repositories;

public class WorkdayRepository : IWorkdayRepository
{
    private readonly WholesalerContext _context;
    private readonly IWorkdayFactory _workdayFactory;

    public WorkdayRepository(WholesalerContext context, IWorkdayFactory workdayFactory)
    {
        _context = context;
        _workdayFactory = workdayFactory;
    }

    public Workday Get(Guid id)
    {
        var workdayDb = _context.Workdays
            .Include(w => w.Person)
            .Where(w => w.Id == id)
            .FirstOrDefault()
            ?? throw new EntityNotFoundException($"There is no workday with {id}");

        return _workdayFactory.Create(workdayDb);
    }

    public List<Workday> GetByPerson(Guid personId)
    {
        var workdayDbList = _context.Workdays
                        .Include(w => w.Person)
                        .Where(w => w.PersonId == personId)
                        .ToList();

        var listOfWorkdays = workdayDbList.Select(workdayDb =>
        {
            var person = new Person(
            workdayDb.Person.Id,
            workdayDb.Person.Login,
            workdayDb.Person.Password,
            workdayDb.Person.Role,
            workdayDb.Person.Name,
            workdayDb.Person.Surname);

            return new Workday(workdayDb.Id, workdayDb.Start, workdayDb.Stop, person);
        });

        return [.. listOfWorkdays];
    }

    public Workday? GetActiveByPersonOrDefault(Guid personId)
    {
        var workdayDb = _context.Workdays
                        .Include(w => w.Person)
                        .Where(w => w.PersonId == personId)
                        .Where(w => w.Stop == null)
                        .FirstOrDefault();

        return workdayDb == null
            ? default
            : _workdayFactory.Create(workdayDb);
    }

    public Workday Add(Workday workday)
    {
        var workdayDb = new WorkdayDb
        {
            Id = workday.Id,
            PersonId = workday.Person.Id,
            Start = workday.Start,
            Stop = workday.Stop
        };

        _context.Workdays.Add(workdayDb);
        _context.SaveChanges();

        return workday;
    }

    public Workday UpdateWorkday(Workday workday)
    {
        var workdayDb = _context.Workdays
            .Where(w => w.Id == workday.Id)
            .FirstOrDefault()
            ?? throw new InvalidProcedureException($"There is no workday with id: {workday.Id}");

        workdayDb.Stop = workday.Stop;

        _context.SaveChanges();

        return workday;
    }
}
