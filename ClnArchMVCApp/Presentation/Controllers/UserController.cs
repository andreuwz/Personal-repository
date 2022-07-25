using Application.Furnitures.Queries.GetAllFurnituresList;
using Application.Furnitures.Queries.GetAllFurnituresListAdmin;
using Application.Interfaces.Persistence;
using Application.Users.Commands.UserAdd.UserFactory;
using Application.Users.Commands.UserDelete;
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
        private readonly IUserFactory userFactory;
        private readonly IUserUpdate userUpdate;
        private readonly IGetUser getUser;
        private readonly IGetAllUsers getAllUsers;
        private readonly IGetAllFurnituresListQuery getAllFurnituresQuery;
        private readonly IUserDelete userDelete;
        private readonly IGetAllFurnituresListAdminQuery getAllFurnituresAdminQuery;

        public UserController(IUserRepository userRepository, IUserFactory userFactory
            , IUserUpdate userUpdate, IGetAllUsers getAllUsers, IGetUser getUser, IGetAllFurnituresListQuery getAllFurnituresQuery, IUserDelete userDelete, IGetAllFurnituresListAdminQuery getAllFurnituresAdminQuery)
        {
            this.userRepository = userRepository;
            this.userFactory = userFactory;
            this.userUpdate = userUpdate;
            this.getAllUsers = getAllUsers;
            this.getUser = getUser;
            this.getAllFurnituresQuery = getAllFurnituresQuery;
            this.userDelete = userDelete;
            this.getAllFurnituresAdminQuery = getAllFurnituresAdminQuery;
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
            var model = getAllUsers.Execute();
            return View("UserAdminMenu", model);
        }

        [HttpGet]
        public ActionResult FurnitureAdminMenu()
        {
            var model = getAllFurnituresAdminQuery.Execute();
            return View("FurnitureAdminMenu", model);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var model = getUser.Execute(id);
            return View(model);
        }

        [HttpGet]
        public ActionResult AdminInitialMenu()
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
                var updateUser = getUser.Execute(resultId);

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
        public ActionResult Delete(int id)
        {
            var deleteuser = getUser.Execute(id);
            return View(deleteuser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, int a)
        {
            try
            {
                var deleteUser = getUser.Execute(id);
                userDelete.Execute(id);

                return View("Deleted", deleteUser);
            }
            catch
            {
                return View("ErrorPage");
            }
        }

        [HttpGet]
        public ActionResult UserItems()
        {
            return View();
        }

    }
}
