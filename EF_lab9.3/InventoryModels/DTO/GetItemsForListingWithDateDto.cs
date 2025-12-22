namespace InventoryModels.DTO
{
    public class GetItemsForListingWithDateDto : GetItemsForListingDto
    {
        public DateTime CreatedDate { get; set; }
    }
}
