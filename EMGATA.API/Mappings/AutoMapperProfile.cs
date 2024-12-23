using AutoMapper;
using EMGATA.API.Dtos;
using EMGATA.API.Models;

namespace EMGATA.API.Mappings;

public class AutoMapperProfile : Profile
{
	public AutoMapperProfile()
	{
		// Car mappings
		CreateMap<Car, CarDto>()
			.ForMember(dest => dest.ModelName, opt => opt.MapFrom(src => src.Model.Name))
			.ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Model.Brand.Name));
		CreateMap<CreateCarDto, Car>();
		CreateMap<UpdateCarDto, Car>();

		// Brand mappings
		CreateMap<Brand, BrandDto>();
		CreateMap<CreateBrandDto, Brand>();
		CreateMap<UpdateBrandDto, Brand>();

		// Model mappings
		CreateMap<Model, ModelDto>()
			.ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name));
		CreateMap<CreateModelDto, Model>();
		CreateMap<UpdateModelDto, Model>();

		// CarImage mappings
		CreateMap<CarImage, CarImageDto>();
		CreateMap<CreateCarImageDto, CarImage>();
	}
}