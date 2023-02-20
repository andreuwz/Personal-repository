using AutoMapper;
using Cart.API.Domain;
using Cart.API.DTO.Request;
using Cart.API.DTO.Response;

namespace Cart.API.Web.AutoMapperProfiles
{
    public class ShoppingCartProfiles : Profile
    {
        public ShoppingCartProfiles()
        {
            CreateMap<PublishedProductModel, Product>()
                .ReverseMap();
            CreateMap<CreateCartModel, ShoppingCart>();
            CreateMap<Product, GetProductModel>();
            CreateMap<Product, GetCartProductsModel>();
            CreateMap<Product, PublishProductForPurchaseModel>();
            CreateMap<Product, PublishProductForPurchaseModel>()
                .PreserveReferences().ForMember(dest => dest.Id, src => src.MapFrom(prop => prop.ProductId));
            CreateMap<ShoppingCart, PublishListProductsForPurchaseModel>();
            CreateMap<ShoppingCart, GetUserCartModel>();
        }
    }
}
