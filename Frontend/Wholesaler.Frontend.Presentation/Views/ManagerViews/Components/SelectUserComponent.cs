using Wholesaler.Core.Dto.ResponseModels;
using Wholesaler.Frontend.Presentation.Views.Generic;

namespace Wholesaler.Frontend.Presentation.Views.ManagerViews.Components;

internal class SelectUserComponent : Component<UserDto>
{
    private readonly List<UserDto> _users;

    public SelectUserComponent(List<UserDto> users)
    {
        _users = users;
    }

    public override UserDto Render()
    {
        var wasCorrectValueProvided = false;
        UserDto? user = null;

        while (wasCorrectValueProvided is false)
        {
            Console.WriteLine("----------------------------");
            Console.WriteLine("Employees ID:");

            foreach (var employee in _users)
                Console.WriteLine($"{_users.IndexOf(employee) + 1}: {employee.Id}");
            Console.WriteLine("----------------------------");
            Console.WriteLine("Enter an id of an employee you want to choose: ");
            if (!int.TryParse(Console.ReadLine(), out var userNumber))
            {
                Console.WriteLine("You entered an invalid value.");
                continue;
            }

            var index = userNumber - 1;
            user = _users
                .Find(x => _users.IndexOf(x) == index);

            if (user == null)
            {
                Console.WriteLine("You entered an invalid value.");
                continue;
            }

            wasCorrectValueProvided = true;
        }

        return user;
    }
}
