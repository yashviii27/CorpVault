using CorpVault.Models;
using Microsoft.EntityFrameworkCore;

namespace CorpVault.Data
{
    public class CorpVaultDbContext : DbContext
    {
        public CorpVaultDbContext(DbContextOptions<CorpVaultDbContext> options)
            : base(options) { }

        public DbSet<Company> Companies { get; set; }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<DocumentFile> DocumentFiles { get; set; }
    }
}
