using Wholesaler.Frontend.Presentation.Views.Generic;

namespace Wholesaler.Frontend.Presentation.Views.Components;

public class ErrorPageComponent : Component
{
    private readonly string _message;

    public ErrorPageComponent(string message)
    {
        _message = message;
    }

    public override void Render()
    {
        Console.WriteLine(_message);
        Console.WriteLine("Press enter to continue.");
        Console.ReadKey();
    }
}
