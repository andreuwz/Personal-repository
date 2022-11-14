using AutoMapper;
using Identity.API.Application.Users.Commands.AssignRoleUser;
using Identity.API.Application.Users.Commands.CreateUser;
using Identity.API.Application.Users.Commands.DeleteUser;
using Identity.API.Application.Users.Commands.LoginUser;
using Identity.API.Application.Users.Commands.RegisterUser;
using Identity.API.Application.Users.Commands.UnassignRoleUser;
using Identity.API.Application.Users.Commands.UpdateUser;
using Identity.API.Application.Users.Commands.UpdateUserAdmin;
using Identity.API.Application.Users.Queries.GetAllUsers;
using Identity.API.Application.Users.Queries.GetPrincipalUser;
using Identity.API.Application.Users.Queries.GetUser;
using Identity.API.Common.Constants;
using Identity.API.DTO.Request;
using Identity.API.DTO.Response;
using Identity.API.Web.IdentityServer.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IGetAllUsers getAllUsers;
        private readonly IMapper mapper;
        private readonly IGetUser getUser;
        private readonly ICreateUser createUser;
        private readonly IUpdateUserAdmin updateUser;
        private readonly IDeleteUser deleteUser;
        private readonly ILoginUser loginUser;
        private readonly IAssignRoleUser assignUser;
        private readonly IUnassignRoleUser unassignUser;
        private readonly IRegisterUser registerUser;
        private readonly IUpdateLoggedUser updateLoggedUser;
        private readonly IGetPrincipalUser getPrincipalUser;
        private readonly IAccessTokenIssuer tokenIssuer;

        public UserController(IGetAllUsers getAllUsers, IMapper mapper, IGetUser getUser, ICreateUser createUser
            , IUpdateUserAdmin updateUser, IDeleteUser deleteUser, ILoginUser loginUser
            , IAssignRoleUser assignUser, IUnassignRoleUser unassignUser, IRegisterUser registerUser, 
            IUpdateLoggedUser updateLoggedUser, IGetPrincipalUser getPrincipalUser,
            IAccessTokenIssuer tokenIssuer)
        {
            this.getAllUsers = getAllUsers;
            this.mapper = mapper;
            this.getUser = getUser;
            this.createUser = createUser;
            this.updateUser = updateUser;
            this.deleteUser = deleteUser;
            this.loginUser = loginUser;
            this.assignUser = assignUser;
            this.unassignUser = unassignUser;
            this.registerUser = registerUser;
            this.updateLoggedUser = updateLoggedUser;
            this.getPrincipalUser = getPrincipalUser;
            this.tokenIssuer = tokenIssuer;
        }

        [Authorize(Roles = "Administrator, MasterAdmin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetUserModel>>> GetAllUsersAsync()
        {
            var user = HttpContext.User;
            return Ok(await getAllUsers.GetAllUsersAsync());
        }

        [Authorize(Roles = "Administrator, MasterAdmin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserModel>> GetUserByIdAsync(Guid id)
        {
            var foundUser = await getUser.GetUserByIdAsync(id.ToString());

            return Ok(mapper.Map<GetUserModel>(foundUser));
        }

        [Authorize(Roles = "Administrator, MasterAdmin")]
        [HttpPost]
        public async Task<IActionResult> CreateUserAsync(CreateUserModel userModel)
        {
            if (ModelState.IsValid)
            {
                await createUser.CreateUserAsync(userModel);
                return Created(nameof(HttpPostAttribute),AppConstants.SerializeSingleMessage(AppConstants.successfulUserCreate));
            }
            var allErrors = ModelState.Values.SelectMany(p => p.Errors.Select(prop => prop.ErrorMessage));
            return BadRequest(AppConstants.SerializeMultipleMessages(allErrors));
        }

        [Authorize(Roles = "Administrator, MasterAdmin")]
        [Authorize(Policy = "AdminSelfActionOnly")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EdiUserAsync(Guid id, AdminEditUserModel userModel)
        {
            if (ModelState.IsValid)
            {
                await updateUser.UpdateUserAsync(id, userModel);
                return Ok(AppConstants.SerializeSingleMessage(AppConstants.SerializeSingleMessage(AppConstants.successfulAdminUserEdit)));
            }
            var allErrors = ModelState.Values.SelectMany(p => p.Errors.Select(prop => prop.ErrorMessage));
            return BadRequest(AppConstants.SerializeMultipleMessages(allErrors));
        }

        [Authorize(Roles = "Administrator, MasterAdmin")]
        [Authorize(Policy = "AdminSelfActionOnly")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync(Guid id)
        {
            await deleteUser.DeleteUserAsync(id);
            return Ok(AppConstants.SerializeSingleMessage(AppConstants.successfulUserDelete));
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
                if (ModelState.IsValid)
                {
                    var userRoles = await loginUser.LoginAsync(loginModel.Username, loginModel.Password);
                    var token = await tokenIssuer.IssueIdentityToken(loginModel.Username, loginModel.Password);
                    
                    var loginOutputInfo = new LoginOutputModel()
                        {
                            Roles = userRoles,
                            AccessToken = token.AccessToken,
                            RefreshToken = token.RefreshToken
                        };
                    return Ok(loginOutputInfo);
                }
                var allErrors = ModelState.Values.SelectMany(p => p.Errors.Select(prop => prop.ErrorMessage));
                return BadRequest(AppConstants.SerializeMultipleMessages(allErrors));
        }

        [Authorize(Roles = "MasterAdmin")]
        [HttpPost("AssignAdminRole/ToUser/{id}")]
        public async Task<IActionResult> AssignRoleToUser(Guid id)
        {
            await assignUser.AssignRoleToUserAsync(id);
            return Ok(AppConstants.SerializeSingleMessage(AppConstants.userIsAssigned));
        }

        [Authorize(Roles = "MasterAdmin")]
        [HttpPost("UnassignAdminRole/FromUser/{id}")]
        public async Task<IActionResult> UnassignRoleFromUser(Guid id)
        {
            await unassignUser.UnassignUserFromRoleAsync(id);
            return Ok(AppConstants.SerializeSingleMessage(AppConstants.userIsUnassigned));
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterNewUser(RegisterUserModel userModel)
        {
            if (ModelState.IsValid)
            {
                var loggedUserRoles = await registerUser.RegisterNewUserAsync(userModel);
                var token = await tokenIssuer.IssueIdentityToken(userModel.UserName, userModel.Password);

                var loginOutputInfo = new LoginOutputModel()
                {
                    Roles = loggedUserRoles,
                    AccessToken = token.AccessToken,
                    RefreshToken = token.RefreshToken
                };

                return Created(nameof(HttpPostAttribute), loginOutputInfo);
            }
            var allErrors = ModelState.Values.SelectMany(p => p.Errors.Select(prop => prop.ErrorMessage));
            return BadRequest(AppConstants.SerializeMultipleMessages(allErrors));
        }

        [HttpPut("LoggedUser")]
        [Authorize]
        public async Task<IActionResult> EditLoggedUser(EditUserModel userModel)
        {
            if (ModelState.IsValid)
            {
                var loggedUser = await getPrincipalUser.GetCurrentUserAsync(User);
                await updateLoggedUser.UpdateUserAsync(loggedUser.Id, userModel);

                return Ok(AppConstants.SerializeSingleMessage(AppConstants.successfulUserEdit));
            }
            var allErrors = ModelState.Values.SelectMany(p => p.Errors.Select(prop => prop.ErrorMessage));
            return BadRequest(AppConstants.SerializeMultipleMessages(allErrors));
        }

        [HttpGet("LoggedUser")]
        [Authorize]
        public async Task<ActionResult<GetUserModel>> GetLoggedUser()
        {
            var loggedUser = await getPrincipalUser.GetCurrentUserAsync(User);
            var outputUser = mapper.Map<GetUserModel>(loggedUser);

            return Ok(outputUser); 
        }
    }
}
