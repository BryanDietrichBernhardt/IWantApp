namespace IWantApp.Endpoints.Clients;

public class OrderPost
{
    public static string Template => "/orders";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    public static async Task<IResult> Action(OrderRequest orderRequest, HttpContext http, ApplicationDbContext context)
    {
        var clientId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var clientName = http.User.Claims.First(c => c.Type == "Name").Value;

        var products = context.Products.Where(p => orderRequest.ProductsIds.Contains(p.Id)).ToList();

        var order = new Order(clientId, clientName, products, orderRequest.DeliveryAddress);

        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();

        return Results.Created($"/orders/${order.Id}", order.Id);
    }
}
