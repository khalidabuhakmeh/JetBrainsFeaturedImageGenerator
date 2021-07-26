using System;
using System.Linq;
using System.Reflection;
using System.Resources;
using SixLabors.Fonts;

namespace JetbrainsFeaturedImageGenerator.Models
{
    public static class Fonts
    {
        private static readonly Assembly Assembly
            = typeof(Products).Assembly;

        public static Font GetGothamFont(int size = 125)
        {
            var collection = new FontCollection();
            var name = Assembly.GetManifestResourceNames()
                .FirstOrDefault(n => n.EndsWith($"Gotham-Bold.ttf"));

            if (name is null)
                throw new MissingManifestResourceException($"Gotham Bold font not found in resources");

            using var stream = Assembly.GetManifestResourceStream(name);

            if (stream is null)
                throw new MissingManifestResourceException($"Gotham Bold font not found in resources");

            collection.Install(stream);

            if (!collection.TryFind("Gotham Bold", out var family))
                throw new Exception("Was unable to create Gotham font");

            var font = family.CreateFont(size, FontStyle.Bold);
            return font;

        }
    }
}