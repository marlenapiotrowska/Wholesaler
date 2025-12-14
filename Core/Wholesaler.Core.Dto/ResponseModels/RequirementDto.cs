namespace Wholesaler.Core.Dto.ResponseModels;

public class RequirementDto
{
    public Guid Id { get; set; }
    public int Quantity { get; set; }
    public Guid ClientId { get; set; }
    public string ClientName { get; set; }
    public Guid StorageId { get; set; }
    public string StorageName { get; set; }
    public string Status { get; set; }
    public DateTime? DeliveryDate { get; set; }
}
