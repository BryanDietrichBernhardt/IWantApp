using Dapper;
using IWantApp.Infra.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

namespace IWantApp.Endpoints.Employees;

public class EmployeeGetAll
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    // pode acessar apenas se tiver EmployeeCode
    [Authorize(Policy = "Employee001Policy")]
    public static IResult Action(int? page, int? rows, QueryAllUsersWithClaimName query)
    {

        if (!page.HasValue || page == 0)
        {
            return Results.BadRequest("Page is required!");
        }
        if (!rows.HasValue || rows == 0)
        {
            return Results.BadRequest("Rows is required!");
        }

        return Results.Ok(query.Execute(page.Value, rows.Value));
    }
}
