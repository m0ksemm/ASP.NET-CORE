using Microsoft.AspNetCore.Routing.Constraints;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.Map("files/{filename}",  
        async context =>
        {
            string? fikeName = Convert.ToString(context.Request.RouteValues["filename"]);
            string? extension = Convert.ToString(context.Request.RouteValues["extension"]);
            await context.Response.WriteAsync($"In files - {fikeName} - {extension}");
        });
    endpoints.Map("employee/profile/{EmployeeName}", async
        context =>
    {
        string? employeeName = Convert.ToString(context.Request.RouteValues["employeename"]);
        await context.Response.WriteAsync($"In Employee profile - {employeeName}");
    });
});

app.Run(async context =>
{
    await context.Response.WriteAsync($"Request recieved at {context.Request.Path}\n");
});

app.Run();
