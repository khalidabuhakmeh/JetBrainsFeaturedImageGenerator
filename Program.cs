using System.Net.Http;
using JetbrainsFeaturedImageGenerator.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;

var builder = WebApplication.CreateBuilder(args);

// unsplash API for sidebar images
builder.Services.AddHttpClient();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// this compiles, don't believe
// what Rider is telling you
app.MapGet("/", async (
        string product, string text, string search,
        HttpClient unsplash, HttpContext ctx
    ) =>
{
    product ??= Products.Rider;
    text ??= $"Cool New\nBlog Post\nFor {product}";
    Image sidebarImage = null;

    // try to find a random side image
    if (!string.IsNullOrWhiteSpace(search))
    {
        sidebarImage = await unsplash.GetSidebarImage(search);
    }

    var image = await Images.Render(product, text, sidebarImage);

    ctx.Response.ContentType = "image/jpeg";
    await image.SaveAsync(ctx.Response.Body, new JpegEncoder());
});

app.Run();