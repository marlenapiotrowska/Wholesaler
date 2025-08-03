using Wholesaler.Core.Dto.ResponseModels;
using Wholesaler.Frontend.Presentation.Views.Generic;

namespace Wholesaler.Frontend.Presentation.Views.ManagerViews.Components;

internal class DisplayRequirementsComponent : Component
{
    private readonly List<RequirementDto> _requirements;

    public DisplayRequirementsComponent(List<RequirementDto> requirements)
    {
        _requirements = requirements;
    }

    public override void Render()
    {
        Console.WriteLine("Ongoing requirements:");

        foreach (var requirement in _requirements)
        {
            Console.WriteLine(
                $"\nId: {requirement.Id}," +
                $"\nQuantity: {requirement.Quantity}," +
                $"\nClientId: {requirement.ClientId}," +
                $"\nStorageId: {requirement.StorageId}," +
                $"\nStatus: {requirement.Status}");
        }

        Console.ReadLine();
    }
}
