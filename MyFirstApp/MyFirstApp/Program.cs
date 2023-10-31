using Microsoft.Extensions.Primitives;
using System.IO;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async (HttpContext context) => 
{
    System.IO.StreamReader reader = new System.IO.StreamReader(context.Request.Body);
    string body = await reader.ReadToEndAsync();

    Dictionary<string, StringValues> queryDict =
        Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(body);

    if (queryDict.ContainsKey("firstNumber"))
    {
        string firstNumber = queryDict["firstNumber"][0];
        string secondNumber = queryDict["secondNumber"][0];
        string operation = queryDict["operation"][0];

        int number1 = Int32.Parse(firstNumber);
        int number2 = Int32.Parse(secondNumber);
        switch (operation)
        {
            case "add":
                await context.Response.WriteAsync($"<p>{number1 + number2}</p>");
                break;
            case "sub":
                await context.Response.WriteAsync($"<p>{number1 - number2}</p>");
                break;
            case "mult":
                await context.Response.WriteAsync($"<p>{number1 * number2}</p>");
                break;
            case "div":
                await context.Response.WriteAsync($"<p>{number1 / number2}</p>");
                break;
            case "rem":
                await context.Response.WriteAsync($"<p>{number1 % number2}</p>");
                break;
        }
    }
});

app.Run();
