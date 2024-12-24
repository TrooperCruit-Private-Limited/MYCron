using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYCentralModels;
using MYCentralModels.SQLite;
using System.Data;
using System.Text.Json;
using MYCron_API.DBContext;

namespace MYCron_API
{
    public class ProjectManagerEndPoints()
    {
        public void WebSiteEndPoint(IEndpointRouteBuilder builder)
        {
            builder.MapGet("api/ProjectManager/check", () => Results.Ok());

            builder.MapPost("api/ProjectManager/Project/Create", async ([FromBody] EmployeeOnSite model, MYCronDbContext Db) =>
            {
                if (await Db.EmployeesOnSite.AnyAsync(x => x.EmployeeName == model.EmployeeName))
                {
                    return Results.Ok(new TransactionStatus
                    {
                        Status = 2,
                        Message = "Employee is already on site."
                    });
                }
                Db.EmployeesOnSite.Add(model);
                await Db.SaveChangesAsync();
                return Results.Ok(new TransactionStatus
                {
                    Status = 1,
                    Message = "Employee added successfully."
                });
            });

            builder.MapPost("api/ProjectManager/Project/Edit", async ([FromBody] EmployeeOnSite model, MYCronDbContext Db) =>
            {
                if (await Db.EmployeesOnSite.AnyAsync(x => x.Id == model.Id && x.EmployeeName == model.EmployeeName && x.Designation == model.Designation && x.DisplayPicture == model.DisplayPicture))
                {
                    return Results.Ok(new TransactionStatus
                    {
                        Status = 2,
                        Message = "Employee details are already updated."
                    });
                }
                Db.EmployeesOnSite.Update(model);
                await Db.SaveChangesAsync();
                return Results.Ok(new TransactionStatus
                {
                    Status = 1,
                    Message = "Employee details updated successfully."
                });
            });

            builder.MapPost("api/ProjectManager/Project/Delete", async ([FromBody] EmployeeOnSite model, MYCronDbContext Db) =>
            {
                string newStatus = model.Active == 0 ? "Inactivated" : "Activated";
                if (await Db.EmployeesOnSite.AnyAsync(x => x.Id == model.Id && x.Active == model.Active))
                {
                    return Results.Ok(new TransactionStatus
                    {
                        Status = 2,
                        Message = $"Employee is already {newStatus}."
                    });
                }
                Db.EmployeesOnSite.Update(model);
                await Db.SaveChangesAsync();
                return Results.Ok(new TransactionStatus
                {
                    Status = 1,
                    Message = $"Employee {newStatus} successfully."
                });
            });

            builder.MapGet("api/ProjectManager/Project/List", async (MYCronDbContext Db) =>
            {
                var employeesList = await Db.EmployeesOnSite.ToListAsync();
                return TypedResults.Ok(employeesList);
            });

            builder.MapGet("api/ProjectManager/Project/Read", async (int id, MYCronDbContext Db) =>
            {
                var employeeDetails = await Db.EmployeesOnSite
                                    .Where(Employee => Employee.Id == id)
                                    .Select(Employee => new EmployeeOnSite
                                    {
                                        Id = Employee.Id,
                                        EmployeeName = Employee.EmployeeName,
                                        Designation = Employee.Designation,
                                        DisplayPicture = Employee.DisplayPicture,
                                        Active = Employee.Active
                                    }).FirstOrDefaultAsync();
                return TypedResults.Ok(employeeDetails);
            });
        }
    }
}
