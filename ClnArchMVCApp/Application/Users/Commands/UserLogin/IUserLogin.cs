﻿using Domain.Users;

namespace Application.Users.Commands.UserLogin
{
    public interface IUserLogin
    {
        CreateUserModel Execute(string username, string password);
    }
}
