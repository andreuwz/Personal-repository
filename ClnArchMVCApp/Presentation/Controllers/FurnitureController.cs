using Application.Furnitures;
using Application.Furnitures.Commands.AddFurniture.FurnitureFactory;
using Application.Furnitures.Commands.RemoveFurniture;
using Application.Furnitures.Commands.UpdateFurniture;
using Application.Furnitures.Queries.GetAllFurnituresList;
using Application.Furnitures.Queries.GetSingleFurniture;
using Application.Interfaces.Persistence;
using Domain.Furnitures;
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

        public FurnitureController(IFurnitureFactory furnitureFactory, IRemoveFurniture removeFurniture
            , IUpdateFurniture updateFurniture, IFurnitureRepository furnitureRepository, IGetAllFurnituresListQuery getAllFurnitures
            , IGetSingleFurnitureQuery getFurniture)
        {
            this.furnitureFactory = furnitureFactory;
            this.removeFurniture = removeFurniture;
            this.updateFurniture = updateFurniture;
            this.furnitureRepository = furnitureRepository;
            this.getAllFurnitures = getAllFurnitures;
            this.getFurniture = getFurniture;
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
                furnitureFactory.Execute(furnitureModel.Name, furnitureModel.Type, furnitureModel.Description, furnitureModel.Quantity);
                return View("Created");
            }
            catch
            {
                return View("Create");
                
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var currentId = HttpContext.GetRouteData().Values["Id"].ToString();
            
            try
            {
                int resultId = int.Parse(currentId);
                var model = getFurniture.Execute(resultId);

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
        public ActionResult Edit(Furniture model)
        {
            var currentId = HttpContext.GetRouteData().Values["Id"].ToString();

            try
            {
                int resultId = int.Parse(currentId);
                var foundModel = furnitureRepository.Get(resultId);

                foundModel.Description = model.Description;
                foundModel.Name = model.Name;
                foundModel.Type = model.Type;
                foundModel.Quantity = model.Quantity;

                updateFurniture.Execute(foundModel);
                return View("Edited", foundModel);
            }
            catch
            {
                return View();
            }

        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var model = furnitureRepository.Get(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, int b)
        {
            var model = furnitureRepository.Get(id);
            try
            {
                removeFurniture.Execute(model);
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
            return View("AdminDetails",model);
        }
    }
}
