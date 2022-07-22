using Application.Furnitures.Queries.GetAllFurnituresList;
using Application.Interfaces.Persistence;
using Application.Users.Commands.UserAdd.UserFactory;
using Application.Users.Commands.UserUpdate;
using Application.Users.Queries.GetAllUsers;
using Application.Users.Queries.GetUser;
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
        private readonly IGetUser getUser;
        private readonly IGetAllUsers getAllUsers;
        private readonly IGetAllFurnituresListQuery getAllFurnituresQuery;

        public UserController(IUserRepository userRepository, IFurnitureRepository furnitureRepository, IUserFactory userFactory
            , IUserUpdate userUpdate, IGetAllUsers getAllUsers, IGetUser getUser, IGetAllFurnituresListQuery getAllFurnituresQuery)
        {
            this.userRepository = userRepository;
            this.furnitureRepository = furnitureRepository;
            this.userFactory = userFactory;
            this.userUpdate = userUpdate;
            this.getAllUsers = getAllUsers;
            this.getUser = getUser;
            this.getAllFurnituresQuery = getAllFurnituresQuery;
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
            var model = getAllUsers.GetAll();
            return View("UserAdminMenu", model);
        }

        [HttpGet]
        public ActionResult FurnitureAdminMenu()
        {
            var model = getAllFurnituresQuery.Execute();
            return View("FurnitureAdminMenu", model);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var model = getUser.GetUser(id);
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

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
            var currentId = HttpContext.GetRouteData().Values["Id"].ToString();

            try
            {
                int resultId = int.Parse(currentId);
                var updateUser = getUser.GetUser(resultId);

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

        [HttpGet]
        public ActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var currentId = HttpContext.GetRouteData().Values["Id"].ToString();

            try
            {
                int resultId = int.Parse(currentId);
                var deleteUser = getUser.GetUser(resultId);

                userRepository.Delete(deleteUser);

                return View("Deleted", deleteUser);
            }
            catch
            {
                return View("Delete");
            }
        }
    }
}
