using Wholesaler.Frontend.Domain.Interfaces;
using Wholesaler.Frontend.Presentation.States;
using Wholesaler.Frontend.Presentation.Views.Components;
using Wholesaler.Frontend.Presentation.Views.Generic;

namespace Wholesaler.Frontend.Presentation.Views.ManagerViews;

internal class StartedTasksView : View
{
    private readonly IWorkTaskRepository _workTasksRepository;
    private readonly StartedWorkTasksState _state;

    public StartedTasksView(IWorkTaskRepository workTasksRepository, ApplicationState state)
        : base(state)
    {
        _workTasksRepository = workTasksRepository;
        _state = state.GetManagerViews().GetStartedWorkTasks();
        _state.Initialize();
    }

    protected override async Task RenderViewAsync()
    {
        var getStartedTasks = await _workTasksRepository.GetStartedWorkTasksAsync();

        if (!getStartedTasks.IsSuccess)
        {
            var errorPage = new ErrorPageComponent(getStartedTasks.Message);
            errorPage.Render();
        }

        _state.GetWorkTasks(getStartedTasks.Payload);

        var tasksWritedOnConsole = new DisplayWorkTasksComponent(getStartedTasks.Payload);
        tasksWritedOnConsole.Render();
    }
}
