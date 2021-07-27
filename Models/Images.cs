using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;
using JetBrainsFeaturedImageGenerator.Models;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using Color = SixLabors.ImageSharp.Color;

namespace JetBrains.FeaturedImageGenerator.Models
{
    public static class Images
    {
        private static readonly Assembly Assembly
            = typeof(Images).Assembly;

        public static async Task<(bool, Image)> TryGetSidebarImage(this HttpClient client, string search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return (false, null);

            return (true, await GetSidebarImage(client, search));
        }

        public static async Task<Image> GetSidebarImage(this HttpClient httpClient, string query)
        {
            var stream = await httpClient.GetStreamAsync($"https://source.unsplash.com/random/1280x720?{query}");
            var sidebar = await Image.LoadAsync(stream);

            return sidebar;
        }

        public static async Task<Image> Render(
            string productName,
            string titleText,
            Image sidebar = null)
        {
            var image = await GetProductImage(productName);

            if (sidebar is { })
            {
                sidebar.Mutate(ctx => ctx
                    .EntropyCrop()
                    .Resize(new ResizeOptions
                    {
                        Size = new Size(645, 1280),
                        Mode = ResizeMode.Crop
                    })
                );

                image.Mutate(ctx => ctx.DrawImage(
                    sidebar,
                    new Point(1275, 0),
                    1
                ));
            }

            var font = Fonts.GetGothamFont(size: 125);
            image.Mutate(ctx => ctx
                .DrawText(
                    new()
                    {
                        TextOptions = new()
                        {
                            WrapTextWidth = 1100,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            ApplyKerning = true
                        },
                        GraphicsOptions = new()
                        {
                            Antialias = true
                        }
                    },
                    titleText,
                    font,
                    Color.White,
                    new PointF(100, 250)
                )
                .Resize(1280, 720)
            );

            return image;
        }

        public static async Task<Image> GetProductImage(string productName)
        {
            var name = Assembly
                .GetManifestResourceNames()
                .FirstOrDefault(n => n.EndsWith($"{productName}.jpg", StringComparison.OrdinalIgnoreCase));

            if (name is null)
                throw new MissingManifestResourceException($"No image for {productName}");

            var stream = Assembly.GetManifestResourceStream(name);

            if (stream is null)
                throw new MissingManifestResourceException($"No image for {productName}");

            var image = await Image.LoadAsync(stream);

            return image;
        }
    }
}