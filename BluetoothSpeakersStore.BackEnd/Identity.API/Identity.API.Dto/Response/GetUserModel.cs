using System.ComponentModel.DataAnnotations;

namespace Identity.API.DTO.Response
{
    public class GetUserModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        [DataType(DataType.Date)]
        public DateTime ModifiedAt { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
        public List<string> UserRoles { get; set; }
        public double Balance { get; set; }
    }
}
