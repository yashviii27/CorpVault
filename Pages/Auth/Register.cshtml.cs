using CorpVault.Data;
using CorpVault.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CorpVault.Pages.Auth;

public class RegisterModel : PageModel
{
    private readonly CorpVaultDbContext _db;

    public RegisterModel(CorpVaultDbContext db)
    {
        _db = db;
    }

    [BindProperty]
    [Required(ErrorMessage = "Company name is required")]
    public string CompanyName { get; set; } = "";

    [BindProperty]
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Enter a valid email address")]
    public string Email { get; set; } = "";

    [BindProperty]
    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    public string Password { get; set; } = "";

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        // 🔴 Duplicate email check
        bool emailExists = await _db.Users.AnyAsync(u => u.Email == Email);
        if (emailExists)
        {
            ModelState.AddModelError("Email", "Email already exists");
            return Page();
        }

        var company = new Company
        {
            Name = CompanyName,
            RegEmail = Email
        };

        _db.Companies.Add(company);
        await _db.SaveChangesAsync();

        var user = new AppUser
        {
            CompanyId = company.Id,
            Email = Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(Password)
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return RedirectToPage("/Auth/Login");
    }
}
