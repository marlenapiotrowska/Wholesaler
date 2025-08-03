using Wholesaler.Frontend.Presentation.States;
using Wholesaler.Frontend.Presentation.Views.EmployeeViews;
using Wholesaler.Frontend.Presentation.Views.Generic;

namespace Wholesaler.Frontend.Presentation.Views.UsersViews;

internal class WorkTaskMenuView : View
{
    private readonly StartWorkTaskView _startWorkTask;
    private readonly StopWorkTaskView _stopWorkTask;
    private readonly FinishWorkTaskView _finishWorkTask;
    private readonly ReviewAssignedTasksView _reviewAssignedTasks;

    public WorkTaskMenuView(
        ApplicationState state,
        StartWorkTaskView startWorkTask,
        StopWorkTaskView stopWorkTask,
        FinishWorkTaskView finishWorkTask,
        ReviewAssignedTasksView reviewAssignedTasks)
        : base(state)
    {
        _startWorkTask = startWorkTask;
        _stopWorkTask = stopWorkTask;
        _finishWorkTask = finishWorkTask;
        _reviewAssignedTasks = reviewAssignedTasks;
    }

    protected override async Task RenderViewAsync()
    {
        var wasExitKeyPressed = false;

        while (wasExitKeyPressed == false)
        {
            Console.WriteLine("---Worktask Menu---");
            Console.WriteLine(
                "\n[1] To start worktask" +
                "\n[2] To stop worktask" +
                "\n[3] To finish worktask" +
                "\n[4] To get information about assigned tasks" +
                "\n[ESC] To quit");

            var pressedKey = Console.ReadKey();

            switch (pressedKey.Key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    await _startWorkTask.RenderAsync();
                    continue;

                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    await _stopWorkTask.RenderAsync();
                    continue;

                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    await _finishWorkTask.RenderAsync();
                    continue;

                case ConsoleKey.D4:
                case ConsoleKey.NumPad4:
                    await _reviewAssignedTasks.RenderAsync();
                    continue;

                case ConsoleKey.Escape:
                    wasExitKeyPressed = true;
                    break;

                default: continue;
            }
        }
    }
}
