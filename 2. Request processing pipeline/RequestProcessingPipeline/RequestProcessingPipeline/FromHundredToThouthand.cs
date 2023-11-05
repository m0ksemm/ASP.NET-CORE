using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace RequestProcessingPipeline
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class FromHundredToThouthand
    {
        private readonly RequestDelegate _next;

        public FromHundredToThouthand(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string? token = context.Request.Query["number"];
            try
            {
                int number = Convert.ToInt32(token);
                number = Math.Abs(number);
                if (number < 101)
                {
                    await _next.Invoke(context);
                }
                else if (number == 1000)
                {
                    await context.Response.WriteAsync("Your number is one thouthand");
                }
                else
                {
                    number /= 100;
                    string[] Numbers = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
                    context.Session.SetString("number", Numbers[number - 1] + " hundred");

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
    public static class FromHundredToThouthandExtensions
    {
        public static IApplicationBuilder UseFromHundredToThouthand(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FromHundredToThouthand>();
        }
    }
}
