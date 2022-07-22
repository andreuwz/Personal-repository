using Application.Interfaces.Persistence;
using Application.Users.Commands.UserAdd.UserFactory;
using Application.Users.Commands.UserUpdate;
using Domain.Users;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IFurnitureRepository furnitureRepository;
        private readonly IUserFactory userFactory;
        private readonly IUserUpdate userUpdate;

        public UserController(IUserRepository userRepository, IFurnitureRepository furnitureRepository, IUserFactory userFactory, IUserUpdate userUpdate)
        {
            this.userRepository = userRepository;
            this.furnitureRepository = furnitureRepository;
            this.userFactory = userFactory;
            this.userUpdate = userUpdate;
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
                var updateUser = userRepository.Get(user.Id);
                updateUser.Username = user.Username;
                updateUser.Firstname = user.Firstname;
                updateUser.Password = user.Password;

                userUpdate.Execute(updateUser);
                return View("Edited", updateUser);
            }
            catch
            {
                return View("Edit");
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
