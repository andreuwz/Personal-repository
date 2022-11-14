using Catalogue.API.DTO.Request;

namespace Catalogue.API.Web.AsyncMessageBusServices.PublishedMessages
{
    public interface IUpdatedUserInfo
    {
        Task UpdateProductsCreatedByUpdatedUser(PublishedUpdatedUserModel updatedUser);
    }
}