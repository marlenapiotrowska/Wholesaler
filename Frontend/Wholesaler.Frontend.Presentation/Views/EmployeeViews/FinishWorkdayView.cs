using Wholesaler.Frontend.Domain.Interfaces;
using Wholesaler.Frontend.Presentation.States;
using Wholesaler.Frontend.Presentation.Views.Components;
using Wholesaler.Frontend.Presentation.Views.Generic;

namespace Wholesaler.Frontend.Presentation.Views.EmployeeViews;

internal class FinishWorkdayView : View
{
    private readonly IWorkDayRepository _workDayRepository;
    private readonly IUserService _service;
    private readonly FinishWorkdayState _state;

    public FinishWorkdayView(IUserService service, IWorkDayRepository workDayRepository, ApplicationState state)
        : base(state)
    {
        _service = service;
        _workDayRepository = workDayRepository;
        _state = state.GetEmployeeViews().GetFinishWorkday();
        _state.Initialize();
    }

    protected override async Task RenderViewAsync()
    {
        var id = State.GetLoggedInUser().Id;
        var finishWorking = await _service.FinishWorkingAsync(id);

        if (!finishWorking.IsSuccess)
        {
            var errorPage = new ErrorPageComponent(finishWorking.Message);
            errorPage.Render();
        }

        _state.FinishWork(finishWorking.Payload);
        var finishWorkday = await _workDayRepository.GetWorkdayAsync(finishWorking.Payload.Id);

        if (!finishWorkday.IsSuccess)
        {
            var errorPage = new ErrorPageComponent(finishWorkday.Message);
            errorPage.Render();
        }

        Console.WriteLine($"You finished your work at: {_state.GetWorkday().Stop}");
        Console.ReadLine();
    }
}
