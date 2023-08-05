using CMC.Core.Constants;
using CMC.Core.Dtos;
using CMC.Infrastructure.Services.Users;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CMC.Web.Controllers
{
    public class UserController : BaseController
    {
     

        public UserController(IUserServices userServices) : base(userServices)
        {
           
        }

        [HttpGet]
        public IActionResult Index()
        {
            
            return View();
        }

        public async Task<IActionResult> ExportToExcel()
        {
            return File(await _userServices.ExportToExcel() , "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "report.xlsx");
        }



        [HttpGet]
        public async  Task<IActionResult> AddFCM(string fcm)
        {
            await _userServices.setFCMToUser(userId , fcm);
            return Ok("Update FCM User");
        }

        public async Task<JsonResult> GetUserData(Pagination pagination , Query query)
        {
            var result =await _userServices.GetAll(pagination, query);
            return Json(result);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm]CreateUserDto dto)  // FromForm بسبب وجود ملفلت نضع 
        {
            if (ModelState.IsValid)
            {
               await _userServices.Create(dto);
               return Ok(Results.AddSuccessResult());
            }
            return View(dto);
        }


        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var user = await _userServices.Get(id);
            return View(user);
        }


        [HttpPost]
        public async Task<IActionResult> Update([FromForm] UpdateUserDto dto)  // FromForm بسبب وجود ملفلت نضع
        {
            if (ModelState.IsValid)
            {
                await _userServices.Update(dto);
                return Ok(Results.EditSuccessResult());
            }
            return View(dto);
        }



        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            await _userServices.Delete(id);
            return Ok(Results.DeleteSuccessResult());
        }

    }
}
