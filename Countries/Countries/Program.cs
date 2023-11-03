var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

Dictionary<int, string> countries = new Dictionary<int, string>()
{
 { 1, "United States" },
 { 2, "Canada" },
 { 3, "United Kingdom" },
 { 4, "India" },
 { 5, "Japan" }
};

app.UseRouting();

//creating endpoints
app.UseEndpoints(endpoints =>
{
    //Eg: /countries
    endpoints.MapGet("/countries", async context =>
    {
        foreach (KeyValuePair<int, string> country in countries)
        {
            await context.Response.WriteAsync($"{country.Key}, {country.Value}\n");
        }
    });

    //Eg: /countries/{countryID}
    endpoints.MapGet("/countries/{countryID:int:range(1,100)}", async context =>
    {
        if (context.Request.RouteValues.ContainsKey("countryID") == false)
        {
            await context.Response.WriteAsync("THe CountryID should be between 1 and 100");
        }

        int countryID = Convert.ToInt32(context.Request.RouteValues["countryID"]);

        if (countries.ContainsKey(countryID))
        {
            string countryName = countries[countryID];
            await context.Response.WriteAsync($"{countryName}\n");
        }
        else
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync($"[No country]\n");
        }
    });


    endpoints.MapGet("/countries/{countryID:min(101)}", async context =>
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("The CountryID should be between 1 and 100 - min");
    });

});

app.Run(async context => {
    await context.Response.WriteAsync("No response");
});

app.Run();