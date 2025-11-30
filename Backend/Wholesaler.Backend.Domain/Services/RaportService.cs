using Wholesaler.Backend.Domain.Entities;
using Wholesaler.Backend.Domain.Interfaces;
using Wholesaler.Backend.Domain.Repositories;

namespace Wholesaler.Backend.Domain.Services;

public class RaportService : IRaportService
{
    private readonly IDeliveryRepository _deliveryRepository;
    private readonly float _multiplier = 0.093f;

    public RaportService(IDeliveryRepository deliveryRepository)
    {
        _deliveryRepository = deliveryRepository;
    }

    public float GetCosts(DateTimeOffset dateFrom, DateTimeOffset dateTo)
    {
        var deliveries = _deliveryRepository.GetForTimespan(dateFrom, dateTo);
        return GetCosts(deliveries);
    }

    private float GetCosts(List<Delivery> deliveries)
    {
        var quantity = 0;

        foreach (var delivery in deliveries)
            quantity += delivery.Quantity;

        return _multiplier * quantity;
    }
}
