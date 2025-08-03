using Wholesaler.Frontend.Domain.Interfaces;
using Wholesaler.Frontend.Presentation.States;
using Wholesaler.Frontend.Presentation.Views.Components;
using Wholesaler.Frontend.Presentation.Views.Generic;

namespace Wholesaler.Frontend.Presentation.Views.ManagerViews;

internal class FinishedTasksView : View
{
    private readonly IWorkTaskRepository _workTasksRepository;
    private readonly FinishedWorkTasksState _state;

    public FinishedTasksView(IWorkTaskRepository workTasksRepository, ApplicationState state) 
        : base(state)
    {
        _workTasksRepository = workTasksRepository;
        _state = state.GetManagerViews().GetFinishedWorkTasks();
        _state.Initialize();
    }

    protected override async Task RenderViewAsync()
    {
        var getFinishedTasks = await _workTasksRepository.GetFinishedWorkTasksAsync();

        if (!getFinishedTasks.IsSuccess)
        {
            var errorPage = new ErrorPageComponent(getFinishedTasks.Message);
            errorPage.Render();
        }

        _state.GetWorkTasks(getFinishedTasks.Payload);

        var tasksWritedOnConsole = new DisplayWorkTasksComponent(getFinishedTasks.Payload);
        tasksWritedOnConsole.Render();
    }
}
