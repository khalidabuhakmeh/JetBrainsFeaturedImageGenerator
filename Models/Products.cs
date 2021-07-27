using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JetBrains.FeaturedImageGenerator.Models
{
    public static class Products
    {
        // read images from folder as products
        private static readonly Lazy<List<string>> Names = new(() => {
            return typeof(Products).Assembly
                .GetManifestResourceNames()
                .Where(p => p.EndsWith(".jpg"))
                .Select(Path.GetFileNameWithoutExtension)
                .Select(n => n.Substring(n.LastIndexOf('.') + 1))
                .ToList();
        });

        public static readonly List<string> All = Names.Value;

        public static bool Contains(string productName)
        {
            return All.Contains(productName, StringComparer.OrdinalIgnoreCase);
        }
    }
}