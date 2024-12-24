using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYCentralModels;
using MYCentralModels.SQLite;
using System.Data;
using System.Text.Json;
using MYCron_API.DBContext;

namespace MYCron_API
{
    public class CommonEndPoints()
    {
        public void WebSiteEndPoint(IEndpointRouteBuilder builder)
        {
            builder.MapGet("api/check", () => Results.Ok());

            builder.MapPost("api/SignUp", async ([FromBody] SignUp model, MYCronDbContext Db) =>
            {
                var usersModel = await Db.Users.Where(x => x.EMail == model.EMail || x.Phone == model.Phone).ToListAsync();
                if (usersModel.Count == 1)
                {
                    return Results.Ok(new TransactionStatus { Status = 2, Message = "You are already our valuable user." });
                }
                else if (usersModel.Count == 2)
                {
                    return Results.Ok(new TransactionStatus { Status = 2, Message = "Your eMail and Phone number are already linked with two different user accounts." });
                }
                Users user = new()
                {
                    UserName = model.UserName,
                    EMail = model.EMail,
                    Phone = model.Phone,
                    Password = model.Password,
                    SignedUpOn = DateTime.UtcNow,
                    Active = 1,
                    UserType = 2
                };
                Db.Users.Add(user);
                await Db.SaveChangesAsync();
                return Results.Ok(new TransactionStatus { Status = 1, Message = "Your account created successfully." });
            });

            builder.MapPost("api/SignIn", async ([FromBody] SignIn model, MYCronDbContext Db) =>
            {
                AuthenticatedUser authenticatedUser = new();
                var usersModel = await Db.Users.Where(x => x.EMail == model.EMailorPhone || x.Phone == model.EMailorPhone).ToListAsync();
                if (usersModel.Count == 0)
                {
                    authenticatedUser.Status = new TransactionStatus { Status = 0, Message = "You look new to TrooperCruit." };
                }
                else
                {
                    Users user = usersModel.FirstOrDefault(user => user.Password == model.Password);
                    if (user == null)
                    {
                        authenticatedUser.Status = new TransactionStatus { Status = 2, Message = "Your password is incorrect." };
                    }
                    else
                    {
                        user.LastSignedInOn = DateTime.UtcNow;
                        Db.Users.Update(user);
                        await Db.SaveChangesAsync();
                        authenticatedUser.Status = new TransactionStatus { Status = 1, Message = "Your successfully authenticated." };
                        authenticatedUser.UserSessionData = new UserSession()
                        {
                            Id = user.Id,
                            UserName = user.UserName,
                            EMail = user.EMail,
                            Phone = user.Phone,
                            LastSignedInOn = user.LastSignedInOn,
                            UserType = user.UserType,
                            LastAccessTime = user.LastSignedInOn
                        };
                    }
                }
                return Results.Ok(authenticatedUser);
            });

            builder.MapPost("api/LogVisitorDetails", async ([FromBody] SiteVisits siteVisitsModel, MYCronDbContext Db) =>
            {
                Db.SiteVisits.Add(siteVisitsModel);
                await Db.SaveChangesAsync();
                return Results.Ok("1");
            });

            builder.MapGet("api/GetVisitUsInfoList", async (MYCronDbContext Db) =>
            {
                var visitUsInfoList = await Db.VisitUsInformation.ToListAsync();
                return TypedResults.Ok(visitUsInfoList);
            });

        }
    }
}
