using AutoMapper;
using RestaurantManagementAPI.Entities;
using RestaurantManagementAPI.Models;

namespace RestaurantManagementAPI.Profiles
{
    public class RestaurantMappingProfile : Profile
    {
        public RestaurantMappingProfile()
        {
            CreateMap<Restaurant, RestaurantDto>()
                .ForMember(dto => dto.City, r => r.MapFrom(c => c.Address.City))
                .ForMember(dto => dto.Street, r => r.MapFrom(c => c.Address.Street))
                .ForMember(dto => dto.PostalCode, r => r.MapFrom(c => c.Address.PostalCode));

            CreateMap<Dish, DishDto>();

            CreateMap<CreateRestaurantDto, Restaurant>()
                .ForMember(r => r.Address, dto => dto.MapFrom(x => new Address()
                { City = x.City, Street = x.Street, PostalCode = x.PostalCode }));

            CreateMap<CreateDishDto, Dish>();
        }
    }
}
