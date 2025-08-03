using Wholesaler.Frontend.Domain.Interfaces;
using Wholesaler.Frontend.Presentation.States;
using Wholesaler.Frontend.Presentation.Views.Components;
using Wholesaler.Frontend.Presentation.Views.Generic;

namespace Wholesaler.Frontend.Presentation.Views.ManagerViews;

internal class AddRequirementView : View
{
    private readonly IRequirementRepository _requirementRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IStorageRepository _storageRepository;
    private readonly AddRequirementState _state;

    public AddRequirementView(
        IRequirementRepository requirementRepository,
        IClientRepository clientRepository,
        IStorageRepository storageRepository,
        ApplicationState state)
        : base(state)
    {
        _requirementRepository = requirementRepository;
        _clientRepository = clientRepository;
        _storageRepository = storageRepository;
        _state = State.GetManagerViews().GetAddRequirement();
        _state.Initialize();
    }

    protected override async Task RenderViewAsync()
    {
        var addRequirementSuccesfully = false;

        while (!addRequirementSuccesfully)
        {
            Console.WriteLine("Quantity: ");
            var quantityInput = Console.ReadLine();
            var getClients = await _clientRepository.GetAllClientsAsync();

            if (!getClients.IsSuccess)
            {
                var errorPage = new ErrorPageComponent(getClients.Message);
                errorPage.Render();
            }

            var selectClient = new SelectClientComponent(getClients.Payload);
            var client = selectClient.Render();

            var getStorages = await _storageRepository.GetAllStoragesAsync();

            if (!getStorages.IsSuccess)
            {
                var errorPage = new ErrorPageComponent(getStorages.Message);
                errorPage.Render();
            }

            var selectStorage = new SelectStorageComponent(getStorages.Payload);
            var storage = selectStorage.Render();

            if (int.TryParse(quantityInput, out var quantity))
            {
                var requirement = await _requirementRepository.AddAsync(quantity, client.Id, storage.Id);
                if (requirement.IsSuccess)
                {
                    _state.GetValues(requirement.Payload.Quantity, requirement.Payload.ClientId);
                    Console.WriteLine("----------------------------");
                    Console.WriteLine($"You add requirement to a client: {requirement.Payload.ClientId} " +
                        $" in a storage: {requirement.Payload.StorageId}" +
                        $" quantity: {requirement.Payload.Quantity}");
                    Console.ReadLine();
                    break;
                }
            }
        }
    }
}
