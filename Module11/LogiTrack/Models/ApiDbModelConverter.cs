
using LogiTrack.Models.Api;
using LogiTrack.Models.DB;

namespace LogiTrack.Models;

public static class ApiDbModelConverter
{
    public static InventoryItemResponse ToApiResponse(InventoryItem dbItem)
    {
        return new InventoryItemResponse
        {
            Id = dbItem.ItemId,
            Name = dbItem.Name,
            Quantity = dbItem.Quantity,
            Location = dbItem.Location
        };
    }

    public static OrderResponse ToApiResponse(Order dbOrder)
    {
        var response = new OrderResponse
        {
            Id = dbOrder.OrderId,
            Customer = ToApiResponse(dbOrder.Customer),
            DatePlaced = dbOrder.DatePlaced,
        };

        response.Items = new List<InventoryItemResponse>();
        foreach (var oi in dbOrder.OrderInventoryItems)
        {
            response.Items.Add(ToApiResponse(oi.InventoryItem));
        }

        return response;
    }

    public static CustomerResponse ToApiResponse(Customer customer)
    {
        return new CustomerResponse
        {
            Id = customer.Id,
            Name = customer.Name
        };
    }
}