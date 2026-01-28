using CorpVault.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace CorpVault.Pages.Auth;

public class LoginModel : PageModel
{
    private readonly CorpVaultDbContext _db;

    public LoginModel(CorpVaultDbContext db)
    {
        _db = db;
    }

    [BindProperty]
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Enter a valid email address")]
    public string Email { get; set; } = "";

    [BindProperty]
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = "";

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(Password, user.PasswordHash))
        {
            ModelState.AddModelError("", "Invalid email or password");
            return Page();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim("CompanyId", user.CompanyId.ToString())
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity)
        );

        return RedirectToPage("/Documents/Index");
    }
}
