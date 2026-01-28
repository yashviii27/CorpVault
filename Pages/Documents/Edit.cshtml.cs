using CorpVault.Data;
using CorpVault.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CorpVault.Pages.Documents;

[Authorize]
public class EditModel : PageModel
{
    private readonly CorpVaultDbContext _db;

    public EditModel(CorpVaultDbContext db)
    {
        _db = db;
    }

    [BindProperty]
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

    public IActionResult OnPost()
    {
        int companyId = int.Parse(User.FindFirst("CompanyId")!.Value);

        var dbDoc = _db.DocumentFiles
            .FirstOrDefault(d => d.Id == Document.Id && d.CompanyId == companyId);

        if (dbDoc == null)
            return RedirectToPage("Index");

        dbDoc.Title = Document.Title;
        _db.SaveChanges();

        return RedirectToPage("Index");
    }
}
