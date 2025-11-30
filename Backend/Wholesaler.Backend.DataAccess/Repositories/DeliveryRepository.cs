using Wholesaler.Backend.DataAccess.Factories;
using Wholesaler.Backend.Domain.Entities;
using Wholesaler.Backend.Domain.Repositories;
using DeliveryDb = Wholesaler.Backend.DataAccess.Models.Delivery;

namespace Wholesaler.Backend.DataAccess.Repositories;

public class DeliveryRepository : IDeliveryRepository
{
    private readonly WholesalerContext _context;
    private readonly IDeliveryFactory _deliveryFactory;

    public DeliveryRepository(WholesalerContext context, IDeliveryFactory deliveryFactory)
    {
        _context = context;
        _deliveryFactory = deliveryFactory;
    }

    public Delivery Add(Delivery delivery)
    {
        var deliveryDb = new DeliveryDb()
        {
            Id = delivery.Id,
            Quantity = delivery.Quantity,
            DeliveryDate = delivery.DeliveryDate,
            PersonId = delivery.PersonId
        };

        _context.Add(deliveryDb);
        _context.SaveChanges();

        return delivery;
    }

    public List<Delivery> GetForTimespan(DateTimeOffset dateFrom, DateTimeOffset dateTo)
    {
        var deliveriesDb = _context.Delivery
            .Where(d => d.DeliveryDate >= dateFrom && d.DeliveryDate <= dateTo)
            .ToList();

        return deliveriesDb
            .ConvertAll(_deliveryFactory.Create);
    }
}
