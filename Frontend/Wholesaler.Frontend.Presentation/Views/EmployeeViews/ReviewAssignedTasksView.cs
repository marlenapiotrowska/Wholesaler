using Wholesaler.Frontend.Domain.Interfaces;
using Wholesaler.Frontend.Presentation.States;
using Wholesaler.Frontend.Presentation.Views.Components;
using Wholesaler.Frontend.Presentation.Views.Generic;

namespace Wholesaler.Frontend.Presentation.Views.EmployeeViews;

internal class ReviewAssignedTasksView : View
{
    private readonly IWorkTaskRepository _workTaskRepository;
    private readonly GetAssignedTasksState _state;

    public ReviewAssignedTasksView(ApplicationState state, IWorkTaskRepository workTaskRepository) 
        : base(state)
    {
        _workTaskRepository = workTaskRepository;
        _state = state.GetEmployeeViews().GetAssigned();
        _state.Initialize();
    }

    protected async override Task RenderViewAsync()
    {
        var id = State.GetLoggedInUser().Id;
        var getTasks = await _workTaskRepository.GetAssignedTaskToAnEmployeeAsync(id);

        if (!getTasks.IsSuccess)
        {
            var errorPage = new ErrorPageComponent(getTasks.Message);
            errorPage.Render();
        }

        _state.GetTasks(getTasks.Payload);

        var tasksWritedOnConsole = new DisplayWorkTasksComponent(getTasks.Payload);
        tasksWritedOnConsole.Render();
    }
}
