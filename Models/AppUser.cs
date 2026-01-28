namespace CorpVault.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public Company Company { get; set; }
    }
}