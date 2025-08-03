using Wholesaler.Frontend.Presentation.Interfaces;
using Wholesaler.Frontend.Presentation.States;
using Wholesaler.Frontend.Presentation.Views.Generic;
using Wholesaler.Frontend.Presentation.Views.UsersViews;

namespace Wholesaler.Frontend.Presentation.Views;

internal class MenuView : View, IMenuView
{
    private readonly EmployeeView _employeeView;
    private readonly ManagerView _managerView;
    private readonly OwnerView _ownerView;

    public MenuView(ApplicationState state, EmployeeView employeeView, ManagerView managerView, OwnerView ownerView)
        : base(state)
    {
        _employeeView = employeeView;
        _managerView = managerView;
        _ownerView = ownerView;
    }

    protected override async Task RenderViewAsync()
    {
        var role = State.GetLoggedInUser().Role;

        switch (role)
        {
            case "Employee":
                await _employeeView.RenderAsync();
                break;

            case "Manager":
                await _managerView.RenderAsync();
                break;

            case "Owner":
                await _ownerView.RenderAsync();
                break;

            default: throw new InvalidOperationException($"User login: {State.GetLoggedInUser().Login} has invalid role: {role}.");
        }
    }
}