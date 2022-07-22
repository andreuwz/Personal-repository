using Application.Interfaces.Persistence;
using Application.Users.Commands.UserAdd.UserFactory;
using Domain.Users;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IFurnitureRepository furnitureRepository;
        private readonly IUserFactory userFactory;

        public UserController(IUserRepository userRepository, IFurnitureRepository furnitureRepository, IUserFactory userFactory)
        {
            this.userRepository = userRepository;
            this.furnitureRepository = furnitureRepository;
            this.userFactory = userFactory;
        }

        [HttpGet]
        public ActionResult Login()
        {

            return View("Login");

        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            var foundUser = userRepository.Login(user.Username, user.Password);

            if (foundUser != null && !foundUser.IsAdmin)
            {
                return RedirectToAction("Index", "Furniture");
            }
            else if (foundUser != null && foundUser.IsAdmin)
            {
                return View("AdminInitialMenu");
            }

            return View("LoginFail");
        }

        [HttpGet] 
        public ActionResult UserAdminMenu()
        {
            var model = userRepository.GetAll();
            return View("UserAdminMenu", model);
        }

        [HttpGet]
        public ActionResult FurnitureAdminMenu()
        {
            var model = furnitureRepository.GetAll();
            return View("FurnitureAdminMenu", model);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var model = userRepository.Get(id);
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            try
            {
                userFactory.Execute(user.Username, user.Password, user.Firstname, user.IsAdmin, user.CreatedAt);
                return View("Created", user);
            }
            catch
            {
                return View("Create");
            }
        }

        [HttpGet]
        public ActionResult Edit()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
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

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
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
