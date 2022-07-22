using Application.Furnitures;
using Application.Furnitures.Commands.AddFurniture.FurnitureFactory;
using Application.Furnitures.Commands.RemoveFurniture;
using Application.Furnitures.Commands.UpdateFurniture;
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

        public FurnitureController(IFurnitureFactory furnitureFactory, IRemoveFurniture removeFurniture
            , IUpdateFurniture updateFurniture, IFurnitureRepository furnitureRepository)
        {
            this.furnitureFactory = furnitureFactory;
            this.removeFurniture = removeFurniture;
            this.updateFurniture = updateFurniture;
            this.furnitureRepository = furnitureRepository;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = furnitureRepository.GetAll();
            return View(model);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var model = furnitureRepository.Get(id);
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
                furnitureFactory.Execute(furnitureModel.Name, furnitureModel.Type, furnitureModel.Description);
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
            var model = furnitureRepository.Get(id);

            if (model == null)
            {
                return View("ErrorPage");
            }

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Furniture model)
        {
            try
            {
                var foundModel = furnitureRepository.Get(model.Id);

                foundModel.Description = model.Description;
                foundModel.Name = model.Name;
                foundModel.Type = model.Type;

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
    }
}
