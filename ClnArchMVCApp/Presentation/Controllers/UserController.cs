using Application.Furnitures.Queries.GetAllFurnituresList;
using Application.Furnitures.Queries.GetAllFurnituresListAdmin;
using Application.Popup;
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
        private readonly IUserDelete userDelete;
        private readonly IGetAllFurnituresListAdminQuery getAllFurnituresAdminQuery;
        private readonly ICreatePopup createPopup;

        public UserController(IUserFactory userFactory
            , IUserUpdate userUpdate, IGetAllUsers getAllUsers, IGetUser getUser, IUserDelete userDelete,
            IGetAllFurnituresListAdminQuery getAllFurnituresAdminQuery,
            IUserLogin userLogin, ICreatePopup createPopup)
        {
            this.userFactory = userFactory;
            this.userUpdate = userUpdate;
            this.getAllUsers = getAllUsers;
            this.getUser = getUser;
            this.userDelete = userDelete;
            this.getAllFurnituresAdminQuery = getAllFurnituresAdminQuery;
            this.userLogin = userLogin;
            this.createPopup = createPopup;
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

                    var popupModel = createPopup.Create();
                    popupModel.Root = Url.Action("AdminInitialMenu", "User", null, "https");
                    popupModel.Message = $"You logged in as ADMIN.";

                    return View("ModalPopUp", popupModel);
                }

                else if (foundUser != null && !foundUser.IsAdmin)
                {
                    var popupModel = createPopup.Create();
                    popupModel.Root = Url.Action("Index", "Furniture", null, "https");
                    popupModel.Message = $"You logged in as a regular user.";

                    return View("ModalPopUp", popupModel);

                }
                else
                {
                    var popupModel = createPopup.Create();
                    popupModel.Root = Url.Action("Login", "User", null, "https");
                    popupModel.Message = $"Login fail. Unrecognized credentials.";

                    return View("ModalPopUp", popupModel);
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

                var popupModel = createPopup.Create();
                popupModel.Root = Url.Action("UserAdminMenu", "User", null, "https");
                popupModel.Message = $"User {user.Username} was created successfully!";

                return View("ModalPopUp", popupModel);
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
                var model = getUser.Execute(id);
                return View(model);
            }
            catch
            {
                var popupModel = createPopup.Create();
                popupModel.Root = Url.Action("UserAdminMenu", "User", null, "https");
                popupModel.Message = $"The user was not found!";

                return View("ModalPopUp", popupModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {


            try
            {
                var updatedUser = userUpdate.Execute(user);

                var popupModel = createPopup.Create();
                popupModel.Root = Url.Action("UserAdminMenu", "User", null, "https");
                popupModel.Message = $"The user {updatedUser.Username} was edited successfully";

                return View("ModalPopUp", popupModel);
            }
            catch
            {
                return View("Edit");
            }
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                var deleteuser = getUser.Execute(id);
                return View(deleteuser);
            }
            catch
            {
                var popupModel = createPopup.Create();
                popupModel.Root = Url.Action("UserAdminMenu", "User", null, "https");
                popupModel.Message = $"The user was not found!";

                return View("ModalPopUp", popupModel);
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, int a)
        {
            try
            {
                var deleteUser = getUser.Execute(id);
                userDelete.Execute(id);

                var popupModel = createPopup.Create();
                popupModel.Root = Url.Action("UserAdminMenu", "User", null, "https");
                popupModel.Message = $"The user {deleteUser.Username} was deleted successfully";

                return View("ModalPopUp", popupModel);
            }
            catch
            {
                var popupModel = createPopup.Create();
                popupModel.Root = Url.Action("UserAdminMenu", "User", null, "https");
                popupModel.Message = $"Deletion was not successful";

                return View("ModalPopUp", popupModel);
            }
        }

        [HttpGet]
        public ActionResult UserItems()
        {
            return View();
        }

    }
}
