using Wholesaler.Frontend.Domain.Interfaces;
using Wholesaler.Frontend.Presentation.States;
using Wholesaler.Frontend.Presentation.Views.Components;
using Wholesaler.Frontend.Presentation.Views.Generic;

namespace Wholesaler.Frontend.Presentation.Views.EmployeeViews;

internal class StartWorkTaskView : View
{
    private readonly IUserService _service;
    private readonly StartWorkTaskState _state;
    private readonly IWorkTaskRepository _workTaskRepository;

    public StartWorkTaskView(IUserService service, ApplicationState state, IWorkTaskRepository workTaskRepository) 
        : base(state)
    {
        _service = service;
        _state = state.GetEmployeeViews().GetStartWorkTask();
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
        var startWorkTask = await _service.StartWorkTaskAsync(workTask.Id);

        if (!startWorkTask.IsSuccess)
        {
            var errorPage = new ErrorPageComponent(startWorkTask.Message);
            errorPage.Render();
        }

        _state.StartWorkTask(startWorkTask.Payload);

        Console.WriteLine($"You started worktask with id: {startWorkTask.Payload.Id} at {DateTime.Now}");
        Console.ReadLine();
    }
}
