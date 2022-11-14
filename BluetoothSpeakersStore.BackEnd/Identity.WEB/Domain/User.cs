using Microsoft.AspNetCore.Identity;

namespace Identity.API.Domain
{
    public class User : IdentityUser
    {
        public double Balance { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
