using Wholesaler.Frontend.Domain.Interfaces;
using Wholesaler.Frontend.Presentation.States;
using Wholesaler.Frontend.Presentation.Views.Components;
using Wholesaler.Frontend.Presentation.Views.Generic;
using Wholesaler.Frontend.Presentation.Views.ManagerViews.Components;

namespace Wholesaler.Frontend.Presentation.Views.ManagerViews;

internal class ChangeOwnerOfTaskView : View
{
    private readonly ChangeOwnerOfTaskState _state;
    private readonly IUserRepository _userRepository;
    private readonly IWorkTaskRepository _workTaskRepository;
    private readonly IUserService _service;

    public ChangeOwnerOfTaskView(ApplicationState state, IUserRepository userRepository, IWorkTaskRepository workTaskRepository, IUserService service) 
        : base(state)
    {
        _userRepository = userRepository;
        _workTaskRepository = workTaskRepository;
        _service = service;
        _state = State.GetManagerViews().GetChangeOwner();
        _state.Initialize();
    }

    protected override async Task RenderViewAsync()
    {
        var role = State.GetLoggedInUser().Role;

        if (role != "Manager")
            throw new InvalidOperationException($"You can not assign task with role {role}. Valid role is Manager.");

        var listOfWorkTasks = await _workTaskRepository.GetAssignedTaskAsync();

        if (listOfWorkTasks.IsSuccess)
            _state.GetWorkTasks(listOfWorkTasks.Payload);

        var workTaskComponent = new SelectWorkTaskComponent(listOfWorkTasks.Payload);
        var selectedWorkTaskId = workTaskComponent.Render().Id;

        var listOfEmployees = await _userRepository.GetEmployeesAsync();

        if (listOfEmployees.IsSuccess)
            _state.GetEmployees(listOfEmployees.Payload);

        var userComponent = new SelectUserComponent(listOfEmployees.Payload);
        var selectedUserId = userComponent.Render().Id;

        var changeOwnerOfTask = await _service.ChangeOwnerAsync(selectedWorkTaskId, selectedUserId);

        if (!changeOwnerOfTask.IsSuccess)
        {
            var errorPage = new ErrorPageComponent(changeOwnerOfTask.Message);
            errorPage.Render();
        }

        _state.ChangeOwnerOfTask(changeOwnerOfTask.Payload);
        Console.WriteLine("----------------------------");
        Console.WriteLine($"You assigned task: {changeOwnerOfTask.Payload.Id} to person: {changeOwnerOfTask.Payload.UserId}");
        Console.ReadLine();
    }
}
