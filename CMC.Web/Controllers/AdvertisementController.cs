using CMC.Core.Constants;
using CMC.Core.Dtos;
using CMC.Infrastructure.Services.Advertisements;
using CMC.Infrastructure.Services.Categories;
using CMC.Infrastructure.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace CMC.Web.Controllers
{
    public class AdvertisementController : BaseController
    {
        private readonly IAdvertisementService _advertisementServices;

        public AdvertisementController(IAdvertisementService advertisementServices , IUserServices userServices) : base (userServices)
        {
            _advertisementServices = advertisementServices;
        }

       
        public IActionResult Index()
        {
            
            return View();
        }


        public async Task<JsonResult> GetAdvertisementData(Pagination pagination , Query query)
        {
            var result =await _advertisementServices.GetAll(pagination, query);
            return Json(result);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["owners"] = new SelectList(await _advertisementServices.GetAdvertisementOwner(),"Id","FullName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAdvertisementDto dto) 
        {
            if (!string.IsNullOrWhiteSpace(dto.OwnerId))
            {
                ModelState.Remove("Owner.FullName");
                ModelState.Remove("Owner.Email");
                ModelState.Remove("Owner.PhoneNumber");
            }
          

            if (ModelState.IsValid)
            {
               await _advertisementServices.Create(dto);
               return Ok(Results.AddSuccessResult());
            }
           
            return View(dto);
        }


        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var user = await _advertisementServices.Get(id);
            return View(user);
        }


        [HttpPost]
        public async Task<IActionResult> Update(UpdateAdvertisementDto dto)
        {
            if (ModelState.IsValid)
            {
                await _advertisementServices.Update(dto);
                return Ok(Results.EditSuccessResult());
            }
            return View(dto);
        }



        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _advertisementServices.Delete(id);
            return Ok(Results.DeleteSuccessResult());
        }

    }
}
