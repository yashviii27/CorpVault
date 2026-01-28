using CorpVault.Data;
using CorpVault.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CorpVault.Pages.Documents;

[Authorize]
public class DeleteModel : PageModel
{
    private readonly CorpVaultDbContext _db;

    public DeleteModel(CorpVaultDbContext db)
    {
        _db = db;
    }

    public DocumentFile Document { get; set; } = null!;

    public IActionResult OnGet(int id)
    {
        int companyId = int.Parse(User.FindFirst("CompanyId")!.Value);

        Document = _db.DocumentFiles
            .FirstOrDefault(d => d.Id == id && d.CompanyId == companyId);

        if (Document == null)
            return RedirectToPage("Index");

        return Page();
    }

    public IActionResult OnPost(int id)
    {
        int companyId = int.Parse(User.FindFirst("CompanyId")!.Value);

        var doc = _db.DocumentFiles
            .FirstOrDefault(d => d.Id == id && d.CompanyId == companyId);

        if (doc != null)
        {
            _db.DocumentFiles.Remove(doc);
            _db.SaveChanges();
        }

        return RedirectToPage("Index");
    }
}
