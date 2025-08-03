using Wholesaler.Frontend.Domain.Interfaces;
using Wholesaler.Frontend.Presentation.States;
using Wholesaler.Frontend.Presentation.Views.Components;
using Wholesaler.Frontend.Presentation.Views.Generic;
using Wholesaler.Frontend.Presentation.Views.ManagerViews.Components;

namespace Wholesaler.Frontend.Presentation.Views.ManagerViews;

internal class RequirementProgressView : View
{
    private readonly IRequirementRepository _requirementRepository;
    private readonly RequirementProgressState _state;

    public RequirementProgressView(IRequirementRepository requirementRepository, ApplicationState state)
        : base(state)
    {
        _requirementRepository = requirementRepository;
        _state = state.GetManagerViews().GetRequirementProgress();
        _state.Initialize();
    }

    protected async override Task RenderViewAsync()
    {
        var chooseStatus = new SelectStatusComponent();
        var status = chooseStatus.Render();
        _state.GetStatus(status);

        var reqirementsWithStatus = await _requirementRepository.GetRequirementsByStatusAsync(status);
        if (!reqirementsWithStatus.IsSuccess)
        {
            var errorPage = new ErrorPageComponent(reqirementsWithStatus.Message);
            errorPage.Render();
        }

        _state.GetRequirements(reqirementsWithStatus.Payload);
        var displayRequirements = new DisplayRequirementsComponent(reqirementsWithStatus.Payload);
        displayRequirements.Render();
    }
}
