var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

var app = builder.Build();

//app.Use(async (context, next) =>
// {
//     // Allow requests from any origin
//     context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

//     // Handle preflight requests
//     if (context.Request.Method == HttpMethods.Options)
//     {
//         context.Response.Headers.Add("Access-Control-Allow-Methods", "GET");
//         context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
//         context.Response.StatusCode = StatusCodes.Status204NoContent;
//         return;
//     }
//     await next.Invoke();
// });

 // Configure the HTTP request pipeline.
 if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
