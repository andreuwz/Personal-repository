using Application.Furnitures;
using Application.Furnitures.Commands.AddFurniture.FurnitureFactory;
using Application.Furnitures.Commands.BuyFurniture;
using Application.Furnitures.Commands.RemoveFurniture;
using Application.Furnitures.Commands.UpdateFurniture;
using Application.Furnitures.Queries.GetAllFurnituresList;
using Application.Furnitures.Queries.GetSingleFurniture;
using Application.Popup;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class FurnitureController : Controller
    {
        private readonly IFurnitureFactory furnitureFactory;
        private readonly IRemoveFurniture removeFurniture;
        private readonly IUpdateFurniture updateFurniture;
        private readonly IGetAllFurnituresListQuery getAllFurnitures;
        private readonly IGetSingleFurnitureQuery getFurniture;
        private readonly IBuyFurniture buyFurniture;
        private readonly ICreatePopup createPopup;


        public FurnitureController(IFurnitureFactory furnitureFactory, IRemoveFurniture removeFurniture
            , IUpdateFurniture updateFurniture, IGetAllFurnituresListQuery getAllFurnitures
            , IGetSingleFurnitureQuery getFurniture, IBuyFurniture buyFurniture, ICreatePopup createPopup)
        {
            this.furnitureFactory = furnitureFactory;
            this.removeFurniture = removeFurniture;
            this.updateFurniture = updateFurniture;
            this.getAllFurnitures = getAllFurnitures;
            this.getFurniture = getFurniture;
            this.buyFurniture = buyFurniture;
            this.createPopup = createPopup;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = getAllFurnitures.Execute();
            return View("Index", model);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var model = getFurniture.Execute(id);
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FurnitureModel furnitureModel)
        {
            
            if (ModelState.IsValid)
            {
                var newFurniture = furnitureFactory.Execute(furnitureModel.Name, furnitureModel.Type, furnitureModel.Description, furnitureModel.Quantity);

                var popupModel = createPopup.Create();
                popupModel.Root = Url.Action("FurnitureAdminMenu", "User", null, "https");
                popupModel.Message = $"The {newFurniture.Name} was created successfully!";

                return View("ModalPopUp", popupModel);
            } 
            else
            {
                return View("Create");
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (ModelState.IsValid)
            {
                var model = getFurniture.Execute(id);
                return View(model);
            }
            else
            {
                var popupModel = createPopup.Create();
                popupModel.Root = Url.Action("FurnitureAdminMenu", "User", null, "https");
                popupModel.Message = $"The furniture was not found!";

                return View("ModalPopUp", popupModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FurnitureModel model)
        {
            if (ModelState.IsValid)
            {
                updateFurniture.Execute(model);

                var popupModel = createPopup.Create();
                popupModel.Root = Url.Action("FurnitureAdminMenu", "User", null, "https");
                popupModel.Message = $"The {model.Name} furniture item was edited successfully!";

                return View("ModalPopUp", popupModel);
            }
            else
            {
                return View("Edit");
            }

        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                var model = getFurniture.Execute(id);
                return View(model);
            }
            else
            {
                var popupModel = createPopup.Create();
                popupModel.Root = Url.Action("FurnitureAdminMenu", "User", null, "https");
                popupModel.Message = $"The furniture was not found!";

                return View("ModalPopUp", popupModel);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, int b)
        {
            removeFurniture.Execute(id);

            var popupModel = createPopup.Create();
            popupModel.Root = Url.Action("FurnitureAdminMenu", "User", null, "https");
            popupModel.Message = $"The furniture was successfully deleted!";

            return View("ModalPopUp", popupModel);
        }

        [HttpGet]
        public ActionResult AdminFurnitureDetails(int id)
        {
            var model = getFurniture.Execute(id);
            return View("AdminDetails", model);
        }

        [HttpGet]
        public ActionResult BuyFurniture(int id)
        {
            var model = getFurniture.Execute(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BuyFurniture(int id, int quantity)
        {
            var buyCheck = buyFurniture.Execute(id, quantity);

            if (buyCheck != null)
            {
                var popupModel = createPopup.Create();
                popupModel.Root = Url.Action("Index", "Furniture", null, "https");
                popupModel.Message = $"You successfully bought {quantity} pieces of this item!";

                return View("ModalPopUp", popupModel);
            }
            else
            {
                var popupModel = createPopup.Create();
                popupModel.Root = Url.Action($"BuyFurniture", "Furniture", id, "https");
                popupModel.Message = $"Cannot buy quantites which are higher than item availability in the store!";

                return View("ModalPopUp", popupModel);
            }
        }

    }
}
