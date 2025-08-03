using Wholesaler.Frontend.Domain.Interfaces;
using Wholesaler.Frontend.Presentation.States;
using Wholesaler.Frontend.Presentation.Views.Components;
using Wholesaler.Frontend.Presentation.Views.Generic;

namespace Wholesaler.Frontend.Presentation.Views.EmployeeViews;

internal class MushroomsDeliverView : View
{
    private readonly IStorageRepository _storageRepository;
    private readonly MushroomsDeliverState _state;

    public MushroomsDeliverView(IStorageRepository storageRepository, ApplicationState state) 
        : base(state)
    {
        _storageRepository = storageRepository;
        _state = State.GetEmployeeViews().GetMushroomsDelivery();
        _state.Initialize();
    }

    protected async override Task RenderViewAsync()
    {
        var role = State.GetLoggedInUser().Role;
        var personId = State.GetLoggedInUser().Id;
        if (role != "Employee")
            throw new InvalidOperationException($"You can not deliver mushrooms with role {role}. Valid role is Employee.");

        var deliverySuccess = false;

        while (!deliverySuccess)
        {
            var getStorages = await _storageRepository.GetAllStoragesAsync();

            if (!getStorages.IsSuccess)
            {
                var errorPage = new ErrorPageComponent(getStorages.Message);
                errorPage.Render();
            }

            var selectStorage = new SelectStorageComponent(getStorages.Payload);
            var storage = selectStorage.Render();

            Console.WriteLine("Enter quantity of mushrooms you want to deliver: ");

            if (int.TryParse(Console.ReadLine(), out var quantity))
            {
                var delivery = await _storageRepository.DeliverAsync(storage.Id, quantity, personId);
                if (delivery.IsSuccess)
                {
                    _state.GetValues(delivery.Payload.Id, quantity);
                    Console.WriteLine("----------------------------");
                    Console.WriteLine($"You delivered {quantity} mushrooms to a storage: {delivery.Payload.Id}");
                    Console.ReadLine();
                    break;
                }
            }
        }
    }
}
