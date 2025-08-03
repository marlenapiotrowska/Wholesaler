using Wholesaler.Frontend.Domain.Interfaces;
using Wholesaler.Frontend.Presentation.States;
using Wholesaler.Frontend.Presentation.Views.Components;
using Wholesaler.Frontend.Presentation.Views.Generic;

namespace Wholesaler.Frontend.Presentation.Views.OwnerViews;

internal class CheckCostsView : View
{
    private readonly IDeliveryRepository _repository;
    private readonly CheckCostsState _state;

    public CheckCostsView(IDeliveryRepository repository, ApplicationState state)
        : base(state)
    {
        _repository = repository;
        _state = state.GetOwnerViews().GetCheckCostsState();
        _state.Initialize();
    }

    protected async override Task RenderViewAsync()
    {
        var dates = GetDates();
        var from = dates[0].ToUnixTimeMilliseconds();
        var to = dates[1].ToUnixTimeMilliseconds();

        var getCosts = await _repository.GetCostsAsync(from, to);

        if (!getCosts.IsSuccess)
        {
            var errorPage = new ErrorPageComponent(getCosts.Message);
            errorPage.Render();
        }

        _state.GetCosts(getCosts.Payload);

        Console.WriteLine($"Costs of mushrooms from {dates[0]} to {dates[1]}: {getCosts.Payload} zł");
        Console.ReadLine();
    }

    private List<DateTimeOffset> GetDates()
    {
        var validValueProvided = false;
        var dates = new List<DateTimeOffset>();

        while (validValueProvided is false)
        {
            Console.WriteLine("Enter the start and the end date." +
            "\nStart date: ");

            if (!DateTimeOffset.TryParse(Console.ReadLine(), out var dateFrom))
            {
                Console.WriteLine("You entered an invalid value.");
                continue;
            }

            Console.WriteLine("End date: ");

            if (!DateTimeOffset.TryParse(Console.ReadLine(), out var dateTo))
            {
                Console.WriteLine("You entered an invalid value.");
                continue;
            }

            dates.Add(dateFrom);
            dates.Add(dateTo);

            validValueProvided = true;
        }

        return dates;
    }
}
