using MiddlewareExample.CustomMiddleware;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();



//middlware 1
app.UseHelloCustomMiddleware();

//middleware 2
app.Run(async (HttpContext context) => {
    await context.Response.WriteAsync("No response\n");
});


app.Run();