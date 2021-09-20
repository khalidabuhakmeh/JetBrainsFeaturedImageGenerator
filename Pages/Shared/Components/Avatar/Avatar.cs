using System.Security.Claims;
using System.Threading.Tasks;
using JetBrains.Space.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace JetBrains.FeaturedImageGenerator.Pages.Shared.Components.Avatar;

public class Avatar : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(int? width, int? height)
    {
        if (User.Identity is not { IsAuthenticated: true })
            return Content("");

        var avatar = UserClaimsPrincipal.FindFirstValue("urn:space:smallAvatar");
        return View(new AvatarModel(UserClaimsPrincipal.Identity?.Name, avatar, width, height));
    }

    public record AvatarModel(string Name, string AvatarId, int? Width, int? Height);
}