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
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Name)); ;
        }
    }
}
