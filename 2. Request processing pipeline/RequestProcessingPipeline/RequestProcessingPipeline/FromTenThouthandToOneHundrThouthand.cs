using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace RequestProcessingPipeline
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class FromTenThouthandToOneHundrThouthand
    {
        private readonly RequestDelegate _next;

        public FromTenThouthandToOneHundrThouthand(RequestDelegate next)
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
                if (number < 10001)
                {
                    await _next.Invoke(context);
                }
                else if (number == 100000)
                {
                    await context.Response.WriteAsync("Your number is one hundred thouthands");
                }
                else if (number > 100000) 
                {
                    await context.Response.WriteAsync("Your number is more then 100 000.\n" +
                        "Please, enter the number between 1 and 100 000");
                }
                else
                {
                    number /= 1000;
                    if (number > 9 && number < 20)
                    {
                        string[] Numbers = { "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                        context.Session.SetString("number", Numbers[number - 10] + " thouthands");

                        await _next.Invoke(context);
                    }
                    else if (number > 19 && number < 100) 
                    {
                        string[] N1 = { "twenty", "thirty", "fourty", "fifty", "sixty", "seventy", "eighty", "ninety"};
                        string[] N2 = {"", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"};

                        int n1 = number / 10;
                        int n2 = number % 10;
                        context.Session.SetString("number", N1[n1 - 2] + " " + N2[n2] + " thouthands");

                        await _next.Invoke(context);

                    }


                }
            }
            catch (Exception)
            {
                await context.Response.WriteAsync("Incorrect parameter");
            }

        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class FromTenThouthandToOneHundrThouthandExtensions
    {
        public static IApplicationBuilder UseFromTenThouthandToOneHundrThouthand(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FromTenThouthandToOneHundrThouthand>();
        }
    }
}
