using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.FeaturedImageGenerator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SixLabors.ImageSharp;

namespace JetBrains.FeaturedImageGenerator.Pages
{
    public class Index : PageModel, IDisposable
    {
        [BindProperty]
        public string Product { get; set; }
        [BindProperty]
        public string Text { get; set; }
        [BindProperty]
        public string Search { get; set; }

        public List<SelectListItem> Products =
            Models.Products.All
                .Select(p => new SelectListItem(p, p))
                .ToList();

        public Image Image { get; set; }

        public async Task<IActionResult> OnPost([FromServices]HttpClient unsplash)
        {
            var (_, sb) = await unsplash.TryGetSidebarImage(Search);
            using var sidebar = sb;
            Image = await Images.Render(Product, Text, sidebar);
            return Partial("_Image", this);
        }

        public void Dispose()
        {
            Image?.Dispose();
        }
    }
}