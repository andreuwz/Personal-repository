﻿using Cart.API.DTO.Request;

namespace Cart.API.Web.AsyncMessageBusServices.PublishedMessages
{
    public interface IDeleteUserInfo
    {
        Task DeleteUserAndHisCartAsync(PublishedUpdatedUserModel userModel);
    }
}