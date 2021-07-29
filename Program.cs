using System.Net.Http;
using JetBrains.FeaturedImageGenerator.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SixLabors.ImageSharp.Formats.Jpeg;

var builder = WebApplication.CreateBuilder(args);

// unsplash API for sidebar images
builder.Services.AddHttpClient();
builder.Services.AddRazorPages();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "Space";
})
.AddCookie(options =>
{
    // set the path for the sign out
    options.LogoutPath = "/signout";
})
.AddSpace(options =>
{
    builder.Configuration.Bind(options);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseForwardedHeaders();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapRazorPages();

// this compiles, don't believe
// what Rider is telling you
app.MapGet("/api", async (
    /*params*/ string product, string text, string font, string search,
    /*services*/ HttpClient unsplash, HttpContext ctx
) =>
{
    if (!Products.Contains(product) || string.IsNullOrWhiteSpace(text))
    {
        ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
        return;
    }

    var (_, sb) = await unsplash.TryGetSidebarImage(search);

    using var sidebar = sb;
    using var image = await Images.Render(product, text, font, sidebar);

    ctx.Response.ContentType = "image/jpeg";
    await image.SaveAsync(ctx.Response.Body, new JpegEncoder());
}).RequireAuthorization();

app.MapGet("/signout", async ctx =>
{
    await ctx.SignOutAsync(
        CookieAuthenticationDefaults.AuthenticationScheme,
        new AuthenticationProperties
        {
            RedirectUri = "/bye"
        });
});

app.Run();