using CorpVault.Data;
using CorpVault.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CorpVault.Pages.Documents;

[Authorize]
public class IndexModel : PageModel
{
    private readonly CorpVaultDbContext _db;

    public IndexModel(CorpVaultDbContext db)
    {
        _db = db;
    }

    public List<DocumentFile> Documents { get; set; } = new();
    public string CompanyName { get; set; } = "";

    public IActionResult OnGet()
    {
        var companyIdClaim = User.FindFirst("CompanyId");
        if (companyIdClaim == null)
            return RedirectToPage("/Auth/Login");

        int companyId = int.Parse(companyIdClaim.Value);

        // ✅ Fetch company name
        CompanyName = _db.Companies
            .Where(c => c.Id == companyId)
            .Select(c => c.Name)
            .FirstOrDefault() ?? "Company";

        Documents = _db.DocumentFiles
            .Where(d => d.CompanyId == companyId)
            .ToList();

        return Page();
    }

    // 🔐 Logout
    public async Task<IActionResult> OnPostLogoutAsync()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme
        );

        return RedirectToPage("/Auth/Login");
    }
}
