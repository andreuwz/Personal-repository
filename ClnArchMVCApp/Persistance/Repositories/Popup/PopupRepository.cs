using Application.Interfaces.Persistence;
using Application.Popup;

namespace Persistance.Repositories.Popup
{
    public class PopupRepository : Repository<PopupModel>, IPopupRepository
    {
        public PopupRepository(DatabaseContext context) : base(context)
    {
    }
    
    }
}
