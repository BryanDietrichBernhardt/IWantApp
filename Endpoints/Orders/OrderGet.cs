namespace IWantApp.Endpoints.Orders;

public class OrderGet
{
    public static string Template => "/orders/{id:guid}";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [Authorize]
    public static async Task<IResult> Action(Guid id, HttpContext http, ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        var clientClaim = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier);
        var employeeClaim = http.User.Claims.FirstOrDefault(c => c.Type == "EmployeeCode");

        var order = await context.Orders.Include(o => o.Products).FirstOrDefaultAsync(o => o.Id == id);

        if (order?.ClientId != clientClaim.Value && employeeClaim == null)
            return Results.Forbid();

        if (order == null) return Results.NotFound();

        var client = await userManager.FindByIdAsync(order.ClientId);

        var productsResponse = order.Products.Select(p => new OrderProduct(p.Id, p.Name, p.Price));
        var orderResponse = new OrderResponse(order.Id, client.Email, productsResponse, order.DeliveryAddress, order.Total);

        return Results.Ok(orderResponse);
    }
}
