using AutoMapper;
using Catalogue.API.Domain;
using Catalogue.API.DTO.Request;
using Catalogue.API.DTO.Response;

namespace Catalogue.API.Web.AutoMapperProfiles
{
    public class ShoppingItemProfiles : Profile 
    {
        public ShoppingItemProfiles()
        {
            CreateMap<Product, CreateProductModel>()
                .ReverseMap();

            CreateMap<Product, UpdateProductModel>()
                .ReverseMap();

            CreateMap<Product, GetProductModelAdmin>()
                .ReverseMap();

            CreateMap<Product, GetProductModel>()
                .ReverseMap();

            CreateMap<PublishedUserModel, LoggedUserModel>();

            CreateMap<Product, PublishProductToCartModel>();

            CreateMap<Product, PublishUpdatedProductModel>();
        }
    }
}
