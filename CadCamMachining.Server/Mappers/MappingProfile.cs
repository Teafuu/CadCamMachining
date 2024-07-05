using AutoMapper;
using CadCamMachining.Server.Models;
using CadCamMachining.Shared.Models;

namespace CadCamMachining.Server.Mappers
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            CreateMap<Order, OrderDto>(); // Map Order to OrderDto
            CreateMap<OrderDto, Order>(); // Map OrderDto to Order
        
            CreateMap<Article, ArticleDto>(); // Map Article to ArticleDto
            CreateMap<ArticleDto, Article>(); // Map ArticleDto to Article

            CreateMap<ArticleStatus, ArticleStatusDto>(); // Map ArticleStatus to ArticleStatusDto
            CreateMap<ArticleStatusDto, ArticleStatus>(); // Map ArticleStatusDto to ArticleStatus

            CreateMap<Contact, ContactDto>(); // Map Contact to ContactDto
            CreateMap<ContactDto, Contact>(); // Map ContactDto to Contact

            CreateMap<Customer, CustomerDto>(); // Map Customer to CustomerDto
            CreateMap<CustomerDto, Customer>(); // Map CustomerDto to Customer

            CreateMap<Material, MaterialDto>(); // Map Material to MaterialDto
            CreateMap<MaterialDto, Material>(); // Map MaterialDto to Material

            CreateMap<OrderStatus, OrderStatusDto>(); // Map OrderStatus to OrderStatusDto
            CreateMap<OrderStatusDto, OrderStatus>(); // Map OrderStatusDto to OrderStatus

            CreateMap<Part, PartDto>(); // Map Part to PartDto
            CreateMap<PartDto, Part>(); // Map PartDto to Part
        }
    }
}
