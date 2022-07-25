using Application.Furnitures;
using Application.Furnitures.Commands.AddFurniture.FurnitureFactory;
using Application.Furnitures.Commands.BuyFurniture;
using Application.Furnitures.Commands.RemoveFurniture;
using Application.Furnitures.Commands.UpdateFurniture;
using Application.Furnitures.Queries.GetAllFurnituresList;
using Application.Furnitures.Queries.GetSingleFurniture;
using Application.Interfaces.Persistence;
using Application.Users.Commands.UserAddItem;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class FurnitureController : Controller
    {
        private readonly IFurnitureFactory furnitureFactory;
        private readonly IRemoveFurniture removeFurniture;
        private readonly IUpdateFurniture updateFurniture;
        private readonly IFurnitureRepository furnitureRepository;
        private readonly IGetAllFurnituresListQuery getAllFurnitures;
        private readonly IGetSingleFurnitureQuery getFurniture;
        private readonly IBuyFurniture buyFurniture;
        private readonly IUserAddItem userAddItem;


        public FurnitureController(IFurnitureFactory furnitureFactory, IRemoveFurniture removeFurniture
            , IUpdateFurniture updateFurniture, IFurnitureRepository furnitureRepository, IGetAllFurnituresListQuery getAllFurnitures
            , IGetSingleFurnitureQuery getFurniture, IBuyFurniture buyFurniture, IUserAddItem userAddItem)
        {
            this.furnitureFactory = furnitureFactory;
            this.removeFurniture = removeFurniture;
            this.updateFurniture = updateFurniture;
            this.furnitureRepository = furnitureRepository;
            this.getAllFurnitures = getAllFurnitures;
            this.getFurniture = getFurniture;
            this.buyFurniture = buyFurniture;
            this.userAddItem = userAddItem;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = getAllFurnitures.Execute();
            return View(model);
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
            try
            {
                var newFurniture = furnitureFactory.Execute(furnitureModel.Name, furnitureModel.Type, furnitureModel.Description, furnitureModel.Quantity);
                return View("Created", newFurniture);
            }
            catch
            {
                return View("Create");
                
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {
                var model = getFurniture.Execute(id);

                if (model == null)
                {
                    return View("ErrorPage");
                }

                return View(model);
            }
            catch
            {
                return View("Edit");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FurnitureModel model)
        {
            try
            {
                updateFurniture.Execute(model);
                return View("Edited", model);
            }
            catch
            {
                return View();
            }

        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var model = getFurniture.Execute(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, int b)
        {
            try
            {
                removeFurniture.Execute(id);
                return View("Deleted");
            }
            catch
            {
                return View("ErrorPage");
            }
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
            try
            {
                var model = buyFurniture.Execute(id, quantity);
                
                if (model == null)
                {
                    return View("InvalidQuantity");
                }
                return View("BoughtItem",model);
            }
            catch
            {
                return View();
            }
        }

        
    }
}
