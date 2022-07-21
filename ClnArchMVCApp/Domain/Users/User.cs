﻿using Domain.Common;
using Domain.Furnitures;

namespace Domain.Users
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public string Firstname { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<Furniture> Furnitures { get; set; }
    }
}
