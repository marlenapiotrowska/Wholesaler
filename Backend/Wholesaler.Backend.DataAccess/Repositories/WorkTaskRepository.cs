using Microsoft.EntityFrameworkCore;
using Wholesaler.Backend.DataAccess.Factories;
using Wholesaler.Backend.Domain.Entities;
using Wholesaler.Backend.Domain.Exceptions;
using Wholesaler.Backend.Domain.Repositories;
using ActivityDb = Wholesaler.Backend.DataAccess.Models.Activity;
using WorkTaskDb = Wholesaler.Backend.DataAccess.Models.WorkTask;

namespace Wholesaler.Backend.DataAccess.Repositories;

public class WorkTaskRepository : IWorkTaskRepository
{
    private readonly WholesalerContext _context;
    private readonly IWorkTaskFactory _workTaskFactory;

    public WorkTaskRepository(WholesalerContext context, IWorkTaskFactory workTaskFactory)
    {
        _context = context;
        _workTaskFactory = workTaskFactory;
    }

    public Guid Add(WorkTask worktask)
    {
        var workTaskDb = new WorkTaskDb()
        {
            Id = worktask.Id,
            Row = worktask.Row,
            IsStarted = worktask.IsStarted,
            IsFinished = worktask.IsFinished,
            PersonId = worktask.Person?.Id
        };

        _context.WorkTasks.Add(workTaskDb);
        _context.SaveChanges();

        return worktask.Id;
    }

    public WorkTask Get(Guid id)
    {
        var workTaskDb = _context.WorkTasks
            .Include(w => w.Person)
            .Include(w => w.Activities)
            .Where(w => w.Id == id)
            .FirstOrDefault()
            ?? throw new EntityNotFoundException($"There is no not assigned worktask with id: {id}");

        if (workTaskDb.Person is null)
        {
            return new(
            workTaskDb.Id,
            workTaskDb.Row,
            workTaskDb.IsStarted,
            workTaskDb.IsFinished);
        }

        var person = new Person(
           workTaskDb.Person.Id,
           workTaskDb.Person.Login,
           workTaskDb.Person.Password,
           workTaskDb.Person.Role,
           workTaskDb.Person.Name,
           workTaskDb.Person.Surname);

        if (workTaskDb.Activities == null)
        {
            var emptyActivities = new List<Activity>();
            return new(workTaskDb.Id, workTaskDb.Row, emptyActivities, workTaskDb.IsStarted, workTaskDb.IsFinished, person);
        }

        var activities = workTaskDb.Activities.Select(activityDb =>
            new Activity(activityDb.Id, activityDb.Start, activityDb.Stop, activityDb.PersonId));

        return new(
            workTaskDb.Id,
            workTaskDb.Row,
            [.. activities],
            workTaskDb.IsStarted,
            workTaskDb.IsFinished,
            person);
    }

    public WorkTask Update(WorkTask worktask)
    {
        var workTaskDb = _context.WorkTasks
            .Include(w => w.Person)
            .Include(w => w.Activities)
            .Where(w => w.Id == worktask.Id)
            .FirstOrDefault()
            ?? throw new InvalidProcedureException($"There is no worktask with id: {worktask.Id}");

        workTaskDb.Activities
            ??= new List<ActivityDb>();

        var activities = worktask.Activities.Select(activity =>
        {
            var activityDb = workTaskDb.Activities
            .FirstOrDefault(a => a.Id == activity.Id);

            if (activityDb == null)
            {
                var newActivity = new ActivityDb()
                {
                    Id = activity.Id,
                    Start = activity.Start,
                    Stop = activity.Stop,
                    PersonId = activity.PersonId,
                    WorkTaskId = worktask.Id
                };
                _context.Activities.Add(newActivity);

                return newActivity;
            }

            activityDb.Start = activity.Start;
            activityDb.Stop = activity.Stop;
            activityDb.PersonId = activity.PersonId;

            return activityDb;
        });

        workTaskDb.Row = worktask.Row;
        workTaskDb.Activities = [.. activities];
        workTaskDb.IsStarted = worktask.IsStarted;
        workTaskDb.IsFinished = worktask.IsFinished;
        workTaskDb.PersonId = worktask.Person?.Id;
        _context.SaveChanges();

        return worktask;
    }

    public List<WorkTask> GetNotAssign()
    {
        var workTasksDbList = _context.WorkTasks
            .Where(w => w.Person == null)
            .ToList();

        var listOfWorkTasks = _workTaskFactory.Create(workTasksDbList);

        return [.. listOfWorkTasks];
    }

    public List<WorkTask> GetAssigned(Guid userId)
    {
        var workTasksDbList = _context.WorkTasks
            .Include(w => w.Person)
            .Include(w => w.Activities)
            .Where(w => w.PersonId == userId)
            .ToList();

        var listOfWorkTasks = _workTaskFactory.Create(workTasksDbList);

        return [.. listOfWorkTasks];
    }

    public List<WorkTask> GetAssigned()
    {
        var workTasksDbList = _context.WorkTasks
            .Include(w => w.Person)
            .Include(w => w.Activities)
            .Where(w => w.Person != null)
            .ToList();

        var listOfWorkTasks = _workTaskFactory.Create(workTasksDbList);

        return [.. listOfWorkTasks];
    }

    public List<WorkTask> GetStarted()
    {
        var workTasksDbList = _context.WorkTasks
            .Include(w => w.Person)
            .Include(w => w.Activities)
            .Where(w => w.IsStarted)
            .ToList();

        var listOfWorkTasks = _workTaskFactory.Create(workTasksDbList);

        return [.. listOfWorkTasks];
    }

    public List<WorkTask> GetFinished()
    {
        var workTasksDbList = _context.WorkTasks
            .Include(w => w.Person)
            .Include(w => w.Activities)
            .Where(w => w.IsFinished)
            .ToList();

        var listOfWorkTasks = _workTaskFactory.Create(workTasksDbList);

        return [.. listOfWorkTasks];
    }
}
