using System.ComponentModel.DataAnnotations;

namespace InventoryModels
{
    public class Genre : FullAuditModel
    {
        [Required]
        [StringLength(InventoryModelConstants.MAX_NAME_LENGTH)]
        public string Name { get; set; }

        public virtual List<ItemGenre> GenreItems { get; set; } = new List<ItemGenre>();
    }
}
