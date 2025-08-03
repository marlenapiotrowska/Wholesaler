using Wholesaler.Frontend.Domain.Interfaces;
using Wholesaler.Frontend.Presentation.States;
using Wholesaler.Frontend.Presentation.Views.Components;
using Wholesaler.Frontend.Presentation.Views.Generic;

namespace Wholesaler.Frontend.Presentation.Views.EmployeeViews;

internal class StopWorkTaskView : View
{
    private readonly IUserService _service;
    private readonly StopWorkTaskState _state;
    private readonly IWorkTaskRepository _workTaskRepository;

    public StopWorkTaskView(IUserService service, ApplicationState state, IWorkTaskRepository workTaskRepository) 
        : base(state)
    {
        _service = service;
        _state = state.GetEmployeeViews().GetStopWorkTask();
        _state.Initialize();
        _workTaskRepository = workTaskRepository;
    }

    protected override async Task RenderViewAsync()
    {
        var id = State.GetLoggedInUser().Id;
        var getTasks = await _workTaskRepository.GetAssignedTaskToAnEmployeeAsync(id);

        if (!getTasks.IsSuccess)
        {
            var errorPage = new ErrorPageComponent(getTasks.Message);
            errorPage.Render();
        }

        var selectWorkTask = new SelectWorkTaskComponent(getTasks.Payload);
        var workTask = selectWorkTask.Render();
        var stopWorkTask = await _service.StopWorkTaskAsync(workTask.Id);

        if (!stopWorkTask.IsSuccess)
        {
            var errorPage = new ErrorPageComponent(stopWorkTask.Message);
            errorPage.Render();
        }

        _state.StopWorkTask(stopWorkTask.Payload);

        Console.WriteLine($"You stopped worktask with id: {stopWorkTask.Payload.Id} at {DateTime.Now}");
        Console.ReadLine();
    }
}
