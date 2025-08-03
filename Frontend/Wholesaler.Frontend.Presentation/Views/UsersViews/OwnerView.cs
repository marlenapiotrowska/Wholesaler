using Wholesaler.Frontend.Presentation.States;
using Wholesaler.Frontend.Presentation.Views.Generic;
using Wholesaler.Frontend.Presentation.Views.OwnerViews;

namespace Wholesaler.Frontend.Presentation.Views.UsersViews;

internal class OwnerView : View
{
    private readonly CheckCostsView _checkCosts;

    public OwnerView(
        CheckCostsView checkcosts,
        ApplicationState state)
        : base(state)
    {
        _checkCosts = checkcosts;
    }

    protected override async Task RenderViewAsync()
    {
        var wasExitKeyPressed = false;

        while (!wasExitKeyPressed)
        {
            Console.Write("---Welcome in Wholesaler---");
            Console.WriteLine(
                "\n[1] To check costs" +
                "\n[2] To check incomes" +
                "\n[3] To see balance sheet" +
                "\n[ESC] To quit");

            var pressedKey = Console.ReadKey();

            switch (pressedKey.Key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    await _checkCosts.RenderAsync();
                    continue;

                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    Console.Clear();
                    continue;

                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    Console.Clear();
                    continue;

                case ConsoleKey.Escape:
                    wasExitKeyPressed = true;
                    break;

                default: continue;
            }
        }
    }
}
