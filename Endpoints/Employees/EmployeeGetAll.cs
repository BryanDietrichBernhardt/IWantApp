using Microsoft.AspNetCore.Identity;

namespace IWantApp.Endpoints.Employees;

public class EmployeeGetAll
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(int page, int rows, UserManager<IdentityUser> userManager)
    {
        var users = userManager.Users.Skip((page - 1) * rows).Take(rows).ToList();
        var employees = new List<EmployeeResponse>();

        foreach (var user in users)
        {
            var claims = userManager.GetClaimsAsync(user).Result;
            var claimName = claims.FirstOrDefault(c => c.Type == "Name")?.Value;
            var userName = claimName ?? string.Empty;
            employees.Add(new EmployeeResponse(user.Email, userName));
        }

        return Results.Ok(employees);
    }
}
