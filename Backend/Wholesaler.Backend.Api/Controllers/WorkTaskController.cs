using Microsoft.AspNetCore.Mvc;
using Wholesaler.Backend.Api.Factories.Interfaces;
using Wholesaler.Backend.Domain.Interfaces;
using Wholesaler.Backend.Domain.Repositories;
using Wholesaler.Backend.Domain.Requests.WorkTasks;
using Wholesaler.Core.Dto.RequestModels;
using Wholesaler.Core.Dto.ResponseModels;

namespace Wholesaler.Backend.Api.Controllers;

[ApiController]
[Route("worktasks")]
public class WorkTaskController : ControllerBase
{
    private readonly IWorkTaskRepository _workTaskRepository;
    private readonly IWorkTaskService _workTaskService;
    private readonly IWorkTaskDtoFactory _workTaskFactory;

    public WorkTaskController(IWorkTaskRepository workTaskRepository, IWorkTaskService workTaskService, IWorkTaskDtoFactory workTaskFactory)
    {
        _workTaskRepository = workTaskRepository;
        _workTaskService = workTaskService;
        _workTaskFactory = workTaskFactory;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> AddAsync([FromBody] AddTaskRequestModel addTask)
    {
        var request = new CreateWorkTaskRequest(addTask.Row);
        var workTask = _workTaskService.Add(request);

        return workTask.Id;
    }

    [HttpPost]
    [Route("{id}/actions/assign")]
    public async Task<ActionResult<WorkTaskDto>> AssignAsync(Guid id, [FromBody] AssignTaskRequestModel assignTask)
    {
        var workTask = _workTaskService.Assign(id, assignTask.UserId);

        return _workTaskFactory.Create(workTask);
    }

    [HttpPatch]
    [Route("{workTaskId}/actions/changeOwner")]
    public async Task<ActionResult<WorkTaskDto>> ChangeOwnerOfWorkTaskAsync(Guid workTaskId, [FromBody] ChangeOwnerRequestModel changeOwner)
    {
        var workTask = _workTaskService.ChangeOwner(workTaskId, changeOwner.NewOwnerId);

        return _workTaskFactory.Create(workTask);
    }

    [HttpPost]
    [Route("{workTaskId}/actions/start")]
    public async Task<ActionResult<WorkTaskDto>> StartWorkTaskAsync(Guid workTaskId)
    {
        var workTask = _workTaskService.Start(workTaskId);

        return _workTaskFactory.Create(workTask);
    }

    [HttpPost]
    [Route("{workTaskId}/actions/stop")]
    public async Task<ActionResult<WorkTaskDto>> StopWorkTaskAsync(Guid workTaskId)
    {
        var workTask = _workTaskService.Stop(workTaskId);

        return _workTaskFactory.Create(workTask);
    }

    [HttpPost]
    [Route("{workTaskId}/actions/finish")]
    public async Task<ActionResult<WorkTaskDto>> FinishWorkTaskAsync(Guid workTaskId)
    {
        var workTask = _workTaskService.Finish(workTaskId);

        return _workTaskFactory.Create(workTask);
    }

    [HttpGet]
    [Route("unassigned")]
    public async Task<ActionResult<List<WorkTaskDto>>> GetNotAssignWorktasksAsync()
    {
        var workday = _workTaskRepository.GetNotAssign();

        return workday
            .ConvertAll(_workTaskFactory.Create);
    }

    [HttpGet]
    [Route("assigned")]
    public async Task<ActionResult<List<WorkTaskDto>>> GetAssignedWorktasksAsync()
    {
        var workday = _workTaskRepository.GetAssigned();

        return workday
            .ConvertAll(_workTaskFactory.Create);
    }

    [HttpGet]
    [Route("assignedToAnEmployee")]
    public async Task<ActionResult<List<WorkTaskDto>>> GetAssignedToAnEmployeeAsync(Guid userId)
    {
        var workTasks = _workTaskRepository.GetAssigned(userId);

        return workTasks
            .ConvertAll(_workTaskFactory.Create);
    }

    [HttpGet]
    [Route("started")]
    public async Task<ActionResult<List<WorkTaskDto>>> GetStartedWorkTasksAsync()
    {
        var workTasks = _workTaskRepository.GetStarted();

        return workTasks
            .ConvertAll(_workTaskFactory.Create);
    }

    [HttpGet]
    [Route("finished")]
    public async Task<ActionResult<List<WorkTaskDto>>> GetFinishedWorkTasksAsync()
    {
        var workTasks = _workTaskRepository.GetFinished();

        return workTasks
            .ConvertAll(_workTaskFactory.Create);
    }
}
