using Microsoft.Extensions.Primitives;
using System.IO;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async (HttpContext context) => 
{
    var qs = context.Request.QueryString;
    string body = qs.ToString();

    if (body.Length > 2) 
    {
        body = body.Substring(1);
        Dictionary<string, StringValues> queryDict =
        Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(body);
        if (!queryDict.ContainsKey("firstNumber"))
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync($"<p>Invalid input for 'firstNumber'</p>");
        }
        else if (!queryDict.ContainsKey("operation") && !queryDict.ContainsKey("secondNumber"))
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync($"<p>Invalid input for 'operation'</p>");
            await context.Response.WriteAsync($"<p>Invalid input for 'secondNumber'</p>");
        }
        else if (!queryDict.ContainsKey("secondNumber"))
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync($"<p>Invalid input for 'secondNumber'</p>");
        }
        else if (!queryDict.ContainsKey("operation"))
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync($"<p>Invalid input for 'operation'</p>");
        }
        else
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
                default:
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync($"<p>Invalid input for 'operation'</p>");
                    break;
            }
        }
    } 
    
    
});

app.Run();
