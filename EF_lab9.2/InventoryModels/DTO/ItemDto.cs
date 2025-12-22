namespace InventoryModels.DTO
{
    public class ItemDto
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return $"{Name,25} | {Description}";
        }
    }
}
