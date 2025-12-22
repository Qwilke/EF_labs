using AutoMapper;
using InventoryModels.DTO;

namespace InventoryModels
{
    public class InventoryMapper : Profile
    {
        public InventoryMapper()
        {
            CreateMaps();
        }

        private void CreateMaps()
        {
            CreateMap<Item, ItemDto>();
            CreateMap<Category, CategoryDto>();
        }
    }
}
