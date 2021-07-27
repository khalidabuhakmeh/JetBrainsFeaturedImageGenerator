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
    public class Index : PageModel
    {
        [BindProperty]
        public string Product { get; set; }
        [BindProperty]
        public string Text { get; set; }
        [BindProperty]
        public string Search { get; set; }

        public List<SelectListItem> Products =
            JetBrainsFeaturedImageGenerator.Models.Products.All
                .Select(p => new SelectListItem(p, p))
                .ToList();

        public Image Image { get; set; }

        public async Task<IActionResult> OnPost([FromServices]HttpClient unsplash)
        {
            Image sidebarImage = null;

            // try to find a random side image
            if (!string.IsNullOrWhiteSpace(Search))
            {
                sidebarImage = await unsplash.GetSidebarImage(Search);
            }

            Image = await Images.Render(Product, Text, sidebarImage);

            return Partial("_Image", this);
        }
    }
}