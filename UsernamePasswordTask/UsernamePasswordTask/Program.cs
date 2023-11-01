using Microsoft.Extensions.Primitives;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

/*app.UseWhen(
    context => (context.Request.Query.ContainsKey("username") && 
    context.Request.Query.ContainsKey("password")), app =>
    {
        app.Use(async (context, next) =>
        {
            
            var qs = context.Request.QueryString;
            string body = qs.ToString();
            Dictionary<string, StringValues> queryDict =
                Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(body);

            string username = queryDict["username"][0];
            string password = queryDict["password"][0];


            await context.Response.WriteAsync($"<p>Username: {username}</p>\n");
            await context.Response.WriteAsync($"<p>Password: {password} </p>\n");

            await next();
        });
    }
    );*/
app.UseWhen(
    context => (context.Request.Query.ContainsKey("username") &&
                context.Request.Query.ContainsKey("password")), app =>
                {
                    app.Use(async (context, next) =>
                    {
                        var qs = context.Request.QueryString;
                        string body = qs.ToString();
                        Dictionary<string, StringValues> quertyDict =
                        Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(body);
                        string username = quertyDict["username"];
                        string password = quertyDict["password"];

                        await context.Response.WriteAsync($"<p>Username: {username}</p>\n");
                        await context.Response.WriteAsync($"<p>Password: {password}</p>\n");

                        await next();
                    });
                });

app.Run(async context =>
{
    await context.Response.WriteAsync("Middleware ended");
});
app.Run();
