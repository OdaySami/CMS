using CMC.Core.Enums;
using CMC.Infrastructure.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CMC.Web.Controllers
{

    [Authorize]
    public class BaseController : Controller
    {
        
        protected readonly IUserServices _userServices;
        protected string userType;
        protected string userId;

        public BaseController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if(User.Identity.IsAuthenticated)
            {                
                    var userName = User.Identity.Name;
                    var user = _userServices.GetUserByUsername(userName);
                    userId = user.Id;
                    userType = user.UserType;
                    ViewBag.fullName = user.FullName;
                    ViewBag.UserType = user.UserType.ToString();
                    ViewBag.image = user.ImageUrl;
            }
        }

    }
}
