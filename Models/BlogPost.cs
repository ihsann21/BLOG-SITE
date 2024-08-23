

using System.ComponentModel.DataAnnotations;

namespace blogSitesi.Models
{
    public class BlogPost
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Content { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }= DateTime.Now;
        

        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        public string? ImagePath { get; set; } // Fotoğrafın dosya yolu
    }

}
