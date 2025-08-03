using Wholesaler.Frontend.Domain.Interfaces;
using Wholesaler.Frontend.Presentation.States;
using Wholesaler.Frontend.Presentation.Views.Components;
using Wholesaler.Frontend.Presentation.Views.Generic;
using Wholesaler.Frontend.Presentation.Views.ManagerViews.Components;

namespace Wholesaler.Frontend.Presentation.Views.ManagerViews;

internal class MushroomsDepartView : View
{
    private readonly IRequirementRepository _requirementRepository;
    private readonly MushroomsDepartState _state;

    public MushroomsDepartView(IRequirementRepository requirementRepository, ApplicationState state)
        : base(state)
    {
        _requirementRepository = requirementRepository;
        _state = State.GetManagerViews().GetMushroomsDeparture();
        _state.Initialize();
    }

    protected async override Task RenderViewAsync()
    {
        var role = State.GetLoggedInUser().Role;
        if (role != "Manager")
            throw new InvalidOperationException($"You can not deliver mushrooms with role {role}. Valid role is Manager.");

        var departureSuccess = false;

        while (departureSuccess is false)
        {
            var getRequirement = await _requirementRepository.GetAllRequirementsAsync();

            if (!getRequirement.IsSuccess)
            {
                var errorPage = new ErrorPageComponent(getRequirement.Message);
                errorPage.Render();
            }

            var selectRequirement = new SelectRequirementComponent(getRequirement.Payload);
            var requirement = selectRequirement.Render();

            var departure = await _requirementRepository.CompleteRequirementAsync(requirement.Id);
            if (departure.IsSuccess)
            {
                _state.GetValues(departure.Payload.Id, requirement.Quantity, requirement.StorageId);
                Console.WriteLine("----------------------------");
                Console.WriteLine($"{departure.Payload.Quantity} mushrooms depart from storage: {departure.Payload.StorageId}");
                Console.ReadLine();
                departureSuccess = true;
            }
        }
    }
}
