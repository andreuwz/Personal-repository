using Domain.Common;

namespace Domain.Furnitures
{
    public class Furniture : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
    }
}
