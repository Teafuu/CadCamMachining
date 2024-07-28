using System.CodeDom;
using AutoMapper;
using CadCamMachining.Server.Models;
using CadCamMachining.Server.Models.Layouts;
using CadCamMachining.Server.Models.Properties;
using CadCamMachining.Shared.Models;

namespace CadCamMachining.Server.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            {
                CreateMap<Item, ItemDto>()
                    .ForMember(dest => dest.PropertyValues, opt => opt.MapFrom(src => src.PropertyValues));
                CreateMap<ItemDto, Item>()
                    .ForMember(dest => dest.PropertyValues, opt => opt.MapFrom(src => src.PropertyValues));

                CreateMap<ItemType, ItemTypeDto>()
                    .ForMember(dest => dest.Properties, opt => opt.MapFrom(src => src.Properties))
                    .ForMember(dest => dest.ParentConnections, opt => opt.MapFrom(src => src.ParentConnections))
                    .ForMember(dest => dest.ChildConnections, opt => opt.MapFrom(src => src.ChildConnections));

                CreateMap<ItemTypeDto, ItemType>()  
                    .ForMember(dest => dest.Properties, opt => opt.MapFrom(src => src.Properties))
                    .ForMember(dest => dest.ParentConnections, opt => opt.MapFrom(src => src.ParentConnections))
                    .ForMember(dest => dest.ChildConnections, opt => opt.MapFrom(src => src.ChildConnections));

                // Property Definitions
                CreateMap<ItemProperty, ItemPropertyDto>()
                    .Include<StringProperty, StringPropertyDto>()
                    .Include<EnumProperty, EnumPropertyDto>()
                    .Include<BoolProperty, BoolPropertyDto>()
                    .Include<DateTimeProperty, DateTimePropertyDto>();
                CreateMap<ItemPropertyDto, ItemProperty>()
                    .Include<StringPropertyDto, StringProperty>()
                    .Include<EnumPropertyDto, EnumProperty>()
                    .Include<BoolPropertyDto, BoolProperty>()
                    .Include<DateTimePropertyDto, DateTimeProperty>();

                CreateMap<StringProperty, StringPropertyDto>();
                CreateMap<StringPropertyDto, StringProperty>();

                CreateMap<EnumProperty, EnumPropertyDto>();
                CreateMap<EnumPropertyDto, EnumProperty>();

                CreateMap<BoolProperty, BoolPropertyDto>();
                CreateMap<BoolPropertyDto, BoolProperty>();

                CreateMap<DateTimeProperty, DateTimePropertyDto>();
                CreateMap<DateTimePropertyDto, DateTimeProperty>();

                //Property Values
                CreateMap<ItemPropertyValue, ItemPropertyValueDto>()
                    .Include<StringPropertyValue, StringPropertyValueDto>()
                    .Include<EnumPropertyValue, EnumPropertyValueDto>()
                    .Include<BoolPropertyValue, BoolPropertyValueDto>()
                    .Include<DateTimePropertyValue, DateTimePropertyValueDto>();

                CreateMap<ItemPropertyValueDto, ItemPropertyValue>()
                    .Include<StringPropertyValueDto, StringPropertyValue>()
                    .Include<EnumPropertyValueDto, EnumPropertyValue>()
                    .Include<BoolPropertyValueDto, BoolPropertyValue>()
                    .Include<DateTimePropertyValueDto, DateTimePropertyValue>();

                CreateMap<StringPropertyValue, StringPropertyValueDto>();
                CreateMap<StringPropertyValueDto, StringPropertyValue>();

                CreateMap<EnumPropertyValue, EnumPropertyValueDto>();
                CreateMap<EnumPropertyValueDto, EnumPropertyValue>();

                CreateMap<BoolPropertyValue, BoolPropertyValueDto>();
                CreateMap<BoolPropertyValueDto, BoolPropertyValue>();

                CreateMap<DateTimePropertyValue, DateTimePropertyValueDto>();
                CreateMap<DateTimePropertyValueDto, DateTimePropertyValue>();

                CreateMap<ItemTypeConnection, ItemTypeConnectionDto>();
                CreateMap<ItemTypeConnectionDto, ItemTypeConnection>();

                CreateMap<ItemConnection, ItemConnectionDto>();
                CreateMap<ItemConnectionDto, ItemConnection>();

                CreateMap<Layout, LayoutDto>().ReverseMap();
                CreateMap<Component, ComponentDto>().ReverseMap();
            }
        }
    }
}
