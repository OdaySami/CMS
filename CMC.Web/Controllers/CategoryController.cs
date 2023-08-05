using CMC.Core.Constants;
using CMC.Core.Dtos;
using CMC.Infrastructure.Services.Categories;
using CMC.Infrastructure.Services.Users;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CMC.Web.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryServices;

        public CategoryController(ICategoryService categoryServices , IUserServices userServices) : base(userServices)
        {
            _categoryServices = categoryServices;
        }


        public IActionResult Index()
        {
            
            return View();
        }


        public async Task<JsonResult> GetCategoryData(Pagination pagination , Query query)
        {
            var result =await _categoryServices.GetAll(pagination, query);
            return Json(result);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryDto dto) 
        {
            if (ModelState.IsValid)
            {
               await _categoryServices.Create(dto);
               return Ok(Results.AddSuccessResult());
            }
            return View(dto);
        }


        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var user = await _categoryServices.Get(id);
            return View(user);
        }


        [HttpPost]
        public async Task<IActionResult> Update(UpdateCategoryDto dto)  
        {
            if (ModelState.IsValid)
            {
                await _categoryServices.Update(dto);
                return Ok(Results.EditSuccessResult());
            }
            return View(dto);
        }



        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryServices.Delete(id);
            return Ok(Results.DeleteSuccessResult());
        }

    }
}
