using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RequestProcessingPipeline
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class FromThouthndToTenThouthand
    {
        private readonly RequestDelegate _next;

        public FromThouthndToTenThouthand(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        
        {
            string result = string.Empty;
            context.Session.SetString("number", "");



            string? token = context.Request.Query["number"];
            try
            {
                int number = Convert.ToInt32(token);
                number = Math.Abs(number);
                if (number < 1001)
                {
                    await _next.Invoke(context);
                }
                else if (number == 10000)
                {
                    await context.Response.WriteAsync("Your number is ten thouthands");
                }
                else
                {
                    number /= 1000;
                    string[] Numbers = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
                    context.Session.SetString("number", Numbers[number - 1] + " thouthands");

                    await _next.Invoke(context);
                }
            }
            catch (Exception)
            {
                await context.Response.WriteAsync("Incorrect parameter");
            }
        }
    }
        // Extension method used to add the middleware to the HTTP request pipeline.
        public static class FromThouthndToTenThouthandExtensions
        {
            public static IApplicationBuilder UseFromThouthndToTenThouthand(this IApplicationBuilder builder)
            {
                return builder.UseMiddleware<FromThouthndToTenThouthand>();
            }
        }
    
}
