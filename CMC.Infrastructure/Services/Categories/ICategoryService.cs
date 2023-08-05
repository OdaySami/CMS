using CMC.Core.Dtos;
using CMC.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMC.Infrastructure.Services.Categories
{
    public interface ICategoryService
    {

        Task<ResponseDto> GetAll(Pagination pagination, Query query);
        Task<int> Create(CreateCategoryDto dto);
        Task<int> Update(UpdateCategoryDto dto);
        Task<int> Delete(int id);
        Task<UpdateCategoryDto> Get(int id);
        Task<List<CategoryViewModel>> GetCategoryList();


    }
}
