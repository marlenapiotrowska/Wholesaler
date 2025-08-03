using Wholesaler.Frontend.Domain.Interfaces;
using Wholesaler.Frontend.Presentation.States;
using Wholesaler.Frontend.Presentation.Views.Components;
using Wholesaler.Frontend.Presentation.Views.Generic;
using Wholesaler.Frontend.Presentation.Views.ManagerViews.Components;

namespace Wholesaler.Frontend.Presentation.Views.ManagerViews;

internal class EditRequirementView : View
{
    private readonly IRequirementRepository _repository;
    private readonly EditRequirementState _state;

    public EditRequirementView(IRequirementRepository repository, ApplicationState state) 
        : base(state)
    {
        _repository = repository;
        _state = State.GetManagerViews().GetEditRequirementState();
        _state.Initialize();
    }

    protected async override Task RenderViewAsync()
    {
        var role = State.GetLoggedInUser().Role;
        if (role != "Manager")
            throw new InvalidOperationException($"You can not edit requirement with role {role}. Valid role is Manager.");

        var editRequirementSuccesfully = false;

        while (!editRequirementSuccesfully)
        {
            var getRequirements = await _repository.GetAllRequirementsAsync();
            if (!getRequirements.IsSuccess)
            {
                var errorPage = new ErrorPageComponent(getRequirements.Message);
                errorPage.Render();
            }

            var selectRequirement = new SelectRequirementComponent(getRequirements.Payload);
            var requirement = selectRequirement.Render();

            Console.WriteLine("New quantity: ");
            var quantityInput = Console.ReadLine();

            if (int.TryParse(quantityInput, out var quantity))
            {
                var editedRequirement = await _repository.EditQuantityAsync(requirement.Id, quantity);
                if (editedRequirement.IsSuccess)
                {
                    _state.GetValues(requirement.Id, quantity);
                    Console.WriteLine("----------------------------");
                    Console.WriteLine($"You edited requirement: {requirement.Id} quantity: {quantity}"); 
                    Console.ReadLine();
                    break;
                }
            }
        }
    }
}
