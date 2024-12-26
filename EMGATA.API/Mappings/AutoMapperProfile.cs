using AutoMapper;
using EMGATA.API.Dtos;
using EMGATA.API.Models;

namespace EMGATA.API.Mappings;

public class AutoMapperProfile : Profile
{
	public AutoMapperProfile()
	{
		CreateMap<Car, CarDto>()
			.ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model));     

		CreateMap<CreateCarDto, Car>();
		CreateMap<UpdateCarDto, Car>();

		// Brand mappings
		CreateMap<Brand, BrandDto>();
		CreateMap<CreateBrandDto, Brand>();
		CreateMap<UpdateBrandDto, Brand>();

		// Model mappings
		CreateMap<Model, ModelDto>()
			.ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand));
		CreateMap<CreateModelDto, Model>();
		CreateMap<UpdateModelDto, Model>();

		// CarImage mappings
		CreateMap<CarImage, CarImageDto>();
		CreateMap<CreateCarImageDto, CarImage>();
	}
}