using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using TwitterBook.Data;
using TwitterBook.Installers;
using TwitterBook.Options;


var builder = WebApplication.CreateBuilder(args);

InstallerExtensions.InstallServicesInAssembly(builder.Services,builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseHsts();
}
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

SwaggerOptions swaggerOptions = new SwaggerOptions();
builder.Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

app.UseSwagger(option =>  option.RouteTemplate = swaggerOptions.JsonRoute);
app.UseSwaggerUI(swaggerUiOptions => {
    swaggerUiOptions.DocumentTitle = "TwitterBook";
    swaggerUiOptions.SwaggerEndpoint(swaggerOptions.UIEndpoint,swaggerOptions.Description);
    swaggerUiOptions.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseStaticFiles();

 app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

public partial class Program { }