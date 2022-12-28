namespace IWantApp.Endpoints.Orders;

public record OrderResponse(Guid Id, string ClientEmail, IEnumerable<OrderProduct> Products, string DeliveryAddress, decimal Total);

public record OrderProduct(Guid Id, string Name, decimal price);