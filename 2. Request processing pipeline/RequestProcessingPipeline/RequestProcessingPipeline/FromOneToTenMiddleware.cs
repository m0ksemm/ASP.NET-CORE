namespace RequestProcessingPipeline
{
    public class FromOneToTenMiddleware
    {
        private readonly RequestDelegate _next;

        public FromOneToTenMiddleware(RequestDelegate next)
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
                if(number == 10)
                {
                    await context.Response.WriteAsync("Your number is ten");
                }
                else
                {
                    string? result = string.Empty;
                    if (number > 10)
                        result = context.Session.GetString("number");
                    number %= 10;
                    string[] Numbers = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"};
                    if(number>0)
                    {
                        await context.Response.WriteAsync("Your number is " + result + " " + Numbers[number-1]);
                    }
                    else
                        await context.Response.WriteAsync("Your number is " + result);
                }            
            }
            catch(Exception)
            {
                await context.Response.WriteAsync("Incorrect parameter");
            }
        }
    }
}
