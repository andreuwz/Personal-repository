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

        // GET: FurnitureController
        public ActionResult Index()
        {
            return View();
        }

        // GET: FurnitureController/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
            var model = furnitureFactory.Execute(furnitureModel.Name, furnitureModel.Type, furnitureModel.Description);
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = furnitureRepository.Get(id);
            return View(model);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Furniture model)
        {
            try
            {
                var foundModel = furnitureRepository.Get(model.Id);
                updateFurniture.Execute(foundModel);
                return View("Edited",foundModel);
            }
            catch
            {
                return View();
            }
            
        }

        // GET: FurnitureController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FurnitureController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
