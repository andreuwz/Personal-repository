namespace Application.Furnitures.Queries.GetAllFurnituresListAdmin
{
    public interface IGetAllFurnituresListAdminQuery
    {
        IEnumerable<FurnitureModel> Execute();
    }
}
