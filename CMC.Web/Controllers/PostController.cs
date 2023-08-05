using CMC.Core.Constants;
using CMC.Core.Dtos;
using CMC.Core.Enums;
using CMC.Infrastructure.Services.Advertisements;
using CMC.Infrastructure.Services.Categories;
using CMC.Infrastructure.Services.Posts;
using CMC.Infrastructure.Services.Tracks;
using CMC.Infrastructure.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace CMC.Web.Controllers
{
    public class PostController : BaseController
    {
        private readonly IPostService _postServices;
        private readonly ICategoryService _categoryService;
       

        public PostController(IPostService postServices, ICategoryService categoryService , IUserServices userServices) : base(userServices)
        {
            _postServices = postServices;
            _categoryService = categoryService;
           
        }


        public IActionResult Index()
        {
            
            return View();
        }


        public async Task<JsonResult> GetPostData(Pagination pagination , Query query)
        {
            var result =await _postServices.GetAll(pagination, query);
            return Json(result);
        }

        public async Task<IActionResult> GetLog(int id)
        {
            var logs = await _postServices.GetLog(id);
            return View(logs);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["categoris"] = new SelectList(await _categoryService.GetCategoryList(),"Id","Name");
            ViewData["authores"] = new SelectList(await _userServices.GetAuthorList(), "Id", "FullName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePostDto dto) 
        {

            if (ModelState.IsValid)
            {
               await _postServices.Create(dto);
               return Ok(Results.AddSuccessResult());
            }
            ViewData["categoris"] = new SelectList(await _categoryService.GetCategoryList(), "Id", "Name");
            ViewData["authores"] = new SelectList(await _userServices.GetAuthorList(), "Id", "FullName");
            return View(dto);
        }

        

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {           
            var track = await _postServices.Get(id);
            ViewData["categoris"] = new SelectList(await _categoryService.GetCategoryList(), "Id", "Name");
            ViewData["authores"] = new SelectList(await _userServices.GetAuthorList(), "Id", "FullName");
            return View(track);
        }


        [HttpPost]
        public async Task<IActionResult> Update(UpdatePostDto dto)
        {
            if (ModelState.IsValid)
            {
                await _postServices.Update(dto);
                return Ok(Results.EditSuccessResult());
            }
            ViewData["categoris"] = new SelectList(await _categoryService.GetCategoryList(), "Id", "Name");
            ViewData["authores"] = new SelectList(await _userServices.GetAuthorList(), "Id", "FullName");
            return View(dto);
        }



        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _postServices.Delete(id);
            return Ok(Results.DeleteSuccessResult());
        }

        [HttpGet]
        public async Task<IActionResult> RemoveAttachment(int id)
        {
            await _postServices.RemoveAttachment(id);
            return Ok(Results.DeleteSuccessResult());
        }


        [HttpGet]
        public async Task<IActionResult> UpdateStatus(int id , ContentStatus status)
        {
            await _postServices.UpdateStatus(id , status);
            return Ok(Results.UpdateStatusSuccessResult());
        }

    }
}
