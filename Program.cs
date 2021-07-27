using System.Net.Http;
using JetBrains.FeaturedImageGenerator.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SixLabors.ImageSharp.Formats.Jpeg;

var builder = WebApplication.CreateBuilder(args);

// unsplash API for sidebar images
builder.Services.AddHttpClient();
builder.Services.AddRazorPages();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.MapRazorPages();

// this compiles, don't believe
// what Rider is telling you
app.MapGet("/api", async (
        /*params*/   string product, string text, string search,
        /*services*/ HttpClient unsplash, HttpContext ctx
    ) =>
{
    if (!Products.Contains(product) || string.IsNullOrWhiteSpace(text))
    {
        ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
        return;
    }

    var (_, sidebar) = await unsplash.TryGetSidebarImage(search);
    var image = await Images.Render(product, text, sidebar);

    ctx.Response.ContentType = "image/jpeg";
    await image.SaveAsync(ctx.Response.Body, new JpegEncoder());
});


app.Run();