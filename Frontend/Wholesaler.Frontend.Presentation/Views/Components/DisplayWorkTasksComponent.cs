using Wholesaler.Core.Dto.ResponseModels;
using Wholesaler.Frontend.Presentation.Views.Generic;

namespace Wholesaler.Frontend.Presentation.Views.Components;

internal class DisplayWorkTasksComponent : Component
{
    private readonly List<WorkTaskDto> _listOfWorkTasks;

    public DisplayWorkTasksComponent(List<WorkTaskDto> listOfWorktasks)
    {
        _listOfWorkTasks = listOfWorktasks;
    }

    public override void Render()
    {
        var groupedTasks = _listOfWorkTasks
            .GroupBy(w => w.UserId)
            .ToList();

        Console.WriteLine("Tasks");

        foreach (var user in groupedTasks)
        {
            Console.WriteLine($"\nUser Id: {user.Key}");
            foreach (var task in user)
            {
                Console.WriteLine(
                $"\nId: {task.Id}" +
                $"\nUser id: {task.UserId}" +
                $"\nRow: {task.Row}" +
                $"\nIs started: {task.IsStarted}" +
                $"\nIsFinished: {task.IsFinished}");
            }
        }

        Console.ReadLine();
    }
}
