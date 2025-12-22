using System.ComponentModel.DataAnnotations;

namespace Activity0601_QuickCrud_Web.Models
{
    public class Category
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [StringLength(50)]
        [Required]
        public string Name { get; set; }
        public virtual List<Item> Items { get; set; } = new List<Item>();
    }
}
