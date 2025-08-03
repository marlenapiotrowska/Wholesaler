using Wholesaler.Core.Dto.ResponseModels;
using Wholesaler.Frontend.Presentation.Views.Generic;

namespace Wholesaler.Frontend.Presentation.Views.Components;

public class SelectClientComponent : Component<ClientDto>
{
    private readonly List<ClientDto> _clients;

    public SelectClientComponent(List<ClientDto> clients)
    {
        _clients = clients;
    }

    public override ClientDto Render()
    {
        var wasCorrectValueProvided = false;
        ClientDto? clientDto = null;

        while (wasCorrectValueProvided is false)
        {
            Console.Clear();
            Console.WriteLine("----------------------------");
            Console.WriteLine("Clients:");

            foreach (var client in _clients)
                Console.WriteLine($"{_clients.IndexOf(client) + 1} {client.Id}");

            Console.WriteLine("----------------------------");
            Console.WriteLine("Enter an index of a client you want to choose: ");
            if (!int.TryParse(Console.ReadLine(), out var clientIndex))
            {
                Console.WriteLine("You entered an invalid value.");
                continue;
            }

            var index = clientIndex - 1;
            clientDto = _clients
                .Find(x => _clients.IndexOf(x) == index);

            if (clientDto == null)
            {
                Console.WriteLine("You entered an invalid value.");
                continue;
            }

            wasCorrectValueProvided = true;
        }

        return clientDto;
    }
}
