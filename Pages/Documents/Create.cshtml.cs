using CorpVault.Data;
using CorpVault.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CorpVault.Pages.Documents;

[Authorize]
public class CreateModel : PageModel
{
    private readonly CorpVaultDbContext _db;
    private readonly IWebHostEnvironment _env;

    public CreateModel(CorpVaultDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    [BindProperty]
    public DocumentFile Document { get; set; } = new();

    [BindProperty]
    public IFormFile UploadFile { get; set; } = null!;

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var companyId = int.Parse(User.FindFirst("CompanyId")!.Value);

        var uploadFolder = Path.Combine(_env.WebRootPath, "uploads");
        Directory.CreateDirectory(uploadFolder);

        var fileName = Guid.NewGuid() + Path.GetExtension(UploadFile.FileName);
        var fullPath = Path.Combine(uploadFolder, fileName);

        using var stream = new FileStream(fullPath, FileMode.Create);
        await UploadFile.CopyToAsync(stream);

        Document.FilePath = "/uploads/" + fileName;
        Document.CompanyId = companyId;

        _db.DocumentFiles.Add(Document);
        await _db.SaveChangesAsync();

        return RedirectToPage("Index");
    }
}
