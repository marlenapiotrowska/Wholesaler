using Wholesaler.Core.Dto.ResponseModels;
using Wholesaler.Frontend.Presentation.Views.Generic;

namespace Wholesaler.Frontend.Presentation.Views.Components;

internal class SelectStorageComponent : Component<StorageDto>
{
    private readonly List<StorageDto> _storages;

    public SelectStorageComponent(List<StorageDto> storages)
    {
        _storages = storages;
    }

    public override StorageDto Render()
    {
        var wasCorrectValueProvided = false;
        StorageDto? storageDto = null;

        while (wasCorrectValueProvided is false)
        {
            Console.Clear();
            Console.WriteLine("----------------------------");
            Console.WriteLine("Storages:");

            foreach (var storage in _storages)
                Console.WriteLine($"{_storages.IndexOf(storage) + 1} {storage.Id}");

            Console.WriteLine("----------------------------");
            Console.WriteLine("Enter an index of a storage you want to choose: ");
            if (!int.TryParse(Console.ReadLine(), out var storageIndex))
            {
                Console.WriteLine("You entered an invalid value.");
                continue;
            }

            var index = storageIndex - 1;
            storageDto = _storages
                .Find(x => _storages.IndexOf(x) == index);

            if (storageDto == null)
            {
                Console.WriteLine("You entered an invalid value.");
                continue;
            }

            wasCorrectValueProvided = true;
        }

        return storageDto;
    }
}
