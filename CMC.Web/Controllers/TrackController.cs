using CMC.Core.Constants;
using CMC.Core.Dtos;
using CMC.Core.Enums;
using CMC.Infrastructure.Services.Advertisements;
using CMC.Infrastructure.Services.Categories;
using CMC.Infrastructure.Services.Tracks;
using CMC.Infrastructure.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace CMC.Web.Controllers
{
    public class TrackController : BaseController
    {
        private readonly ITrackService _trackServices;
        private readonly ICategoryService _categoryService;

        public TrackController(ITrackService trackServices , ICategoryService categoryService , IUserServices userServices) : base(userServices)
        {
            _trackServices = trackServices;
            _categoryService = categoryService;
        }


        public IActionResult Index()
        {
            
            return View();
        }


        public async Task<JsonResult> GetTrackData(Pagination pagination , Query query)
        {
            var result =await _trackServices.GetAll(pagination, query);
            return Json(result);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["categoris"] = new SelectList(await _categoryService.GetCategoryList(),"Id","Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTrackDto dto) 
        {

            if (ModelState.IsValid)
            {
               await _trackServices.Create(dto);
               return Ok(Results.AddSuccessResult());
            }
            ViewData["categoris"] = new SelectList(await _categoryService.GetCategoryList(), "Id", "Name");
            return View(dto);
        }


        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var track = await _trackServices.Get(id);
            ViewData["categoris"] = new SelectList(await _categoryService.GetCategoryList(), "Id", "Name");
            return View(track);
        }


        [HttpPost]
        public async Task<IActionResult> Update(UpdateTrackDto dto)
        {
            if (ModelState.IsValid)
            {
                await _trackServices.Update(dto);
                return Ok(Results.EditSuccessResult());
            }
            ViewData["categoris"] = new SelectList(await _categoryService.GetCategoryList(), "Id", "Name");
            return View(dto);
        }



        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _trackServices.Delete(id);
            return Ok(Results.DeleteSuccessResult());
        }



        [HttpGet]
        public async Task<IActionResult> GetLog(int Id)
        {
            var logs = await _trackServices.GetLog(Id);
            return View(logs);
        }


        [HttpGet]
        public async Task<IActionResult> UpdateStatus(int id, ContentStatus status)
        {
            await _trackServices.UpdateStatus(id, status);
            return Ok(Results.UpdateStatusSuccessResult());
        }

    }
}
