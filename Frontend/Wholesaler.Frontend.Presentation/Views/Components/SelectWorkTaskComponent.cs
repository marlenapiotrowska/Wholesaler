using Wholesaler.Core.Dto.ResponseModels;
using Wholesaler.Frontend.Presentation.Views.Generic;

namespace Wholesaler.Frontend.Presentation.Views.Components;

public class SelectWorkTaskComponent : Component<WorkTaskDto>
{
    private readonly List<WorkTaskDto> _workTasks;

    public SelectWorkTaskComponent(List<WorkTaskDto> workTasks)
    {
        _workTasks = workTasks;
    }

    public override WorkTaskDto Render()
    {
        var wasCorrectValueProvided = false;
        WorkTaskDto? workTask = null;

        while (!wasCorrectValueProvided)
        {
            Console.Clear();
            Console.WriteLine("----------------------------");
            Console.WriteLine("Tasks:");

            foreach (var task in _workTasks)
                Console.WriteLine($"{_workTasks.IndexOf(task) + 1} {task.Id}");

            Console.WriteLine("----------------------------");
            Console.WriteLine("Enter an index of a task you want to choose: ");
            if (!int.TryParse(Console.ReadLine(), out var workTaskNumber))
            {
                Console.WriteLine("You entered an invalid value.");
                continue;
            }

            var index = workTaskNumber - 1;
            workTask = _workTasks
                .Find(x => _workTasks.IndexOf(x) == index);

            if (workTask == null)
            {
                Console.WriteLine("You entered an invalid value.");
                continue;
            }

            wasCorrectValueProvided = true;
        }

        return workTask;
    }
}
