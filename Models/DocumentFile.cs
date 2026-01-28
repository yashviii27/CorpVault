using System.ComponentModel.DataAnnotations.Schema;

namespace CorpVault.Models
{
    [Table("Documents")] // 👈 IMPORTANT
    public class DocumentFile
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Title { get; set; } = "";
        public string FilePath { get; set; } = "";
        public DateTime UploadedAt { get; set; } = DateTime.Now;
    }
}
