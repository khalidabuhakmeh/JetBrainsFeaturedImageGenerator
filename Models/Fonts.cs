using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SixLabors.Fonts;

namespace JetBrains.FeaturedImageGenerator.Models
{
    public static class Fonts
    {
        private static readonly Assembly Assembly
            = typeof(Products).Assembly;

        public const string GothamBold = "Gotham Bold";
        public const string ArialUnicodeMs = "Arial Unicode MS";

        public static IReadOnlyList<(string,string)> All { get; } = new List<(string,string)>() {
            ("Gotham", GothamBold),
            ("Arial Unicode", ArialUnicodeMs)
        }.AsReadOnly();

        private static Lazy<FontCollection> FontCollection = new(() =>
        {
            var collection = new FontCollection();
            var fonts = Assembly.GetManifestResourceNames()
                .Where(n => n.EndsWith($".ttf"))
                .ToList();

            foreach (var font in fonts)
            {
                using var stream = Assembly.GetManifestResourceStream(font);
                if (stream == null) continue;
                collection.Install(stream);
            }

            return collection;
        });

        /// <summary>
        /// Get a Font given the name and size
        /// </summary>
        /// <param name="name">Default font is Gotham Bold</param>
        /// <param name="size">Default size is 125px</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static Font GetFontOrDefault(string name = GothamBold, int size = 125)
        {
            name ??= GothamBold;

            var fontCollection = FontCollection.Value;
            if (!fontCollection.TryFind(name, out var family))
                throw new ArgumentException($"Was unable to create {name} font", nameof(name));

            var font = family.CreateFont(size, FontStyle.Bold);
            return font;
        }
    }
}