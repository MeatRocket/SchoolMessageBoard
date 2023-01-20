using Microsoft.EntityFrameworkCore;
using MessageBoardClassLibrary.MessageBoardContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using NuGet.Configuration;
using AdminPortal.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using AdminPortal.Services;
using AdminPortal.Hubs;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SignalR.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddServerSideBlazor();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new DbLogging());
});


builder.Services.AddDbContext<BoardContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddScoped<DbLogging>();
builder.Services.AddTransient<CommentServices>();


builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

builder.Services.AddSignalR();
builder.Services.AddScoped<HubConnectionBuilder>();
//builder.Services.AddRazorPages();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<BoardContext>();

builder.Services.AddSession();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();


app.UseResponseCompression();

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
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapBlazorHub();
app.MapHub<CommentHub>("/CommentHub");
app.UsePathBase("/subsite");

app.Run();
