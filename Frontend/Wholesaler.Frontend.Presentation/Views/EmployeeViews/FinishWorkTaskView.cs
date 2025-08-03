using Wholesaler.Frontend.Domain.Interfaces;
using Wholesaler.Frontend.Presentation.States;
using Wholesaler.Frontend.Presentation.Views.Components;
using Wholesaler.Frontend.Presentation.Views.Generic;

namespace Wholesaler.Frontend.Presentation.Views.EmployeeViews;

internal class FinishWorkTaskView : View
{
    private readonly IUserService _service;
    private readonly FinishWorkTaskState _state;
    private readonly IWorkTaskRepository _workTaskRepository;

    public FinishWorkTaskView(IUserService service, ApplicationState state, IWorkTaskRepository workTaskRepository) 
        : base(state)
    {
        _service = service;
        _state = state.GetEmployeeViews().GetFinishWorkTask();
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
        var finishWorkTask = await _service.FinishWorkTaskAsync(workTask.Id);

        if (!finishWorkTask.IsSuccess)
        {
            var errorPage = new ErrorPageComponent(finishWorkTask.Message);
            errorPage.Render();
        }

        _state.FinishWorkTask(finishWorkTask.Payload);

        Console.WriteLine($"You finished worktask with id: {finishWorkTask.Payload.Id} at {DateTime.Now}");
        Console.ReadLine();
    }
}
