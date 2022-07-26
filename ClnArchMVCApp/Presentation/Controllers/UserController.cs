using Application.Furnitures.Queries.GetAllFurnituresList;
using Application.Furnitures.Queries.GetAllFurnituresListAdmin;
using Application.Users;
using Application.Users.Commands.UserAdd.UserFactory;
using Application.Users.Commands.UserDelete;
using Application.Users.Commands.UserLogin;
using Application.Users.Commands.UserUpdate;
using Application.Users.Queries.GetAllUsers;
using Application.Users.Queries.GetUser;
using Domain.Users;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserLogin userLogin;
        private readonly IUserFactory userFactory;
        private readonly IUserUpdate userUpdate;
        private readonly IGetUser getUser;
        private readonly IGetAllUsers getAllUsers;
        private readonly IGetAllFurnituresListQuery getAllFurnituresQuery;
        private readonly IUserDelete userDelete;
        private readonly IGetAllFurnituresListAdminQuery getAllFurnituresAdminQuery;

        public UserController(IUserFactory userFactory
            , IUserUpdate userUpdate, IGetAllUsers getAllUsers, IGetUser getUser, IGetAllFurnituresListQuery getAllFurnituresQuery, IUserDelete userDelete, IGetAllFurnituresListAdminQuery getAllFurnituresAdminQuery, IUserLogin userLogin)
        {
            this.userFactory = userFactory;
            this.userUpdate = userUpdate;
            this.getAllUsers = getAllUsers;
            this.getUser = getUser;
            this.getAllFurnituresQuery = getAllFurnituresQuery;
            this.userDelete = userDelete;
            this.getAllFurnituresAdminQuery = getAllFurnituresAdminQuery;
            this.userLogin = userLogin;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {
            
            try
            {
                var foundUser = userLogin.Execute(loginModel.Username, loginModel.Password);

                if (foundUser != null && foundUser.IsAdmin)
                {

                    return View("AdminInitialMenu");
                }

                else if (foundUser != null && !foundUser.IsAdmin)
                {
                    return RedirectToAction("Index", "Furniture");

                }
                else
                {
                    TempData["msg"] = "Unknown credentials. Authentication fail!";
                    return View("LoginFail");
                }
            }
            catch
            {
                return View("Login");
            }

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
        public ActionResult Edit(int id)
        {
            var model = getUser.Execute(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            try
            {
                var updatedUser = userUpdate.Execute(user);
                return View("Edited", updatedUser);
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

        [HttpGet]
        public ActionResult LoginFail()
        {
            TempData["Message"] = "You are not authorized.";
            return View("Login");
            
        }

    }
}
