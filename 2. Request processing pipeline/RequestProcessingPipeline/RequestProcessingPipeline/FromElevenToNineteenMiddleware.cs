using Microsoft.AspNetCore.Http;

namespace RequestProcessingPipeline
{
    public class FromElevenToNineteenMiddleware
    {
        private readonly RequestDelegate _next;

        public FromElevenToNineteenMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string? token = context.Request.Query["number"];
            try
            {
                int number = Convert.ToInt32(token);
                number = Math.Abs(number);
                if (number % 100 < 11 || number % 100 > 19)
                {
                    await _next.Invoke(context);
                }
                else
                {
                    if (number > 100)
                        number %= 100;
                    //if (number > 1000)
                    //    number %= 1000;
                    string? result = string.Empty;//
                    result = context.Session.GetString("number");//
                    string[] Numbers = { "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                    await context.Response.WriteAsync("Your number is " + result + " " + Numbers[number - 11]);
                }
            }
            catch (Exception)
            {
                await context.Response.WriteAsync("Incorrect parameter");
            }
        }
    }
}
