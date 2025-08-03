using Wholesaler.Frontend.Domain.Interfaces;
using Wholesaler.Frontend.Presentation.States;
using Wholesaler.Frontend.Presentation.Views.Components;
using Wholesaler.Frontend.Presentation.Views.Generic;

namespace Wholesaler.Frontend.Presentation.Views.EmployeeViews;

internal class StartWorkdayView : View
{
    private readonly IUserService _service;
    private readonly IWorkDayRepository _workDayRepository;
    private readonly StartWorkdayState _state;

    public StartWorkdayView(IUserService service, IWorkDayRepository workdayRepository, ApplicationState state)
        : base(state)
    {
        _workDayRepository = workdayRepository;
        _service = service;
        _state = state.GetEmployeeViews().GetStartWorkday();
        _state.Initialize();
    }

    protected override async Task RenderViewAsync()
    {
        var id = State.GetLoggedInUser().Id;
        var startWorking = await _service.StartWorkingAsync(id);

        if (!startWorking.IsSuccess)
        {
            var errorPage = new ErrorPageComponent(startWorking.Message);
            errorPage.Render();
        }

        _state.StartWork(startWorking.Payload);            
        var newWorkday = await _workDayRepository.GetWorkdayAsync(startWorking.Payload.Id);

        if (!newWorkday.IsSuccess)
        {
            var errorPage = new ErrorPageComponent(newWorkday.Message);
            errorPage.Render();
        }

        Console.WriteLine($"You started your work at: {_state.GetWorkday().Start}");
        Console.ReadLine();
    }
}
