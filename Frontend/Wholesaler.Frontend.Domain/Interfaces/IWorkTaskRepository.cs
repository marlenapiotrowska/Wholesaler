using Wholesaler.Core.Dto.ResponseModels;
using Wholesaler.Frontend.Domain.ValueObjects;

namespace Wholesaler.Frontend.Domain.Interfaces;

public interface IWorkTaskRepository
{
    Task<ExecutionResultGeneric<List<WorkTaskDto>>> GetNotAssignWorkTasksAsync();

    Task<ExecutionResultGeneric<List<WorkTaskDto>>> GetAssignedTaskAsync();

    Task<ExecutionResultGeneric<List<WorkTaskDto>>> GetAssignedTaskToAnEmployeeAsync(Guid userId);

    Task<ExecutionResultGeneric<List<WorkTaskDto>>> GetStartedWorkTasksAsync();

    Task<ExecutionResultGeneric<List<WorkTaskDto>>> GetFinishedWorkTasksAsync();
}
