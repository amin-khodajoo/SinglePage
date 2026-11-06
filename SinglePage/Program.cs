using Microsoft.EntityFrameworkCore;
using SinglePage.Models;
using SinglePage.Models.Services.Contracts;
using SinglePage.Models.Services.Repositories;
using SinglePage.ApplicationServices;
using SinglePage.ApplicationServices.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ProjectDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IPersonApplicationService, PersonApplicationService>();
builder.Services.AddControllers();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(errorApp => errorApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await Results.Problem().ExecuteAsync(context);
    }));
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapControllers();
app.MapGet("/", () => Results.Redirect("/index.html"));
app.Run();
