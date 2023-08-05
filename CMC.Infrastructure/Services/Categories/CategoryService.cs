using AutoMapper;
using CMC.Core.Constants;
using CMC.Core.Dtos;
using CMC.Core.Enums;
using CMC.Core.Exceptions;
using CMC.Core.ViewModels;
using CMC.Data;
using CMC.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace CMC.Infrastructure.Services.Categories
{
    public class CategoryService : ICategoryService
    {

        private readonly CMCDbContext _db;
        private readonly IMapper _mapper;


        public CategoryService(CMCDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }


        public async Task<List<CategoryViewModel>> GetCategoryList()
        {
            var categorys = await _db.Categories.Where(x => !x.IsDelete).ToListAsync();
            return _mapper.Map<List<CategoryViewModel>>(categorys);
        }

        public async Task<ResponseDto> GetAll(Pagination pagination, Query query)
        {
            var queryString = _db.Categories.Where(x => !x.IsDelete && (x.Name.Contains(query.GeneralSearch) || string.IsNullOrWhiteSpace(query.GeneralSearch))).AsQueryable();

            var dataCount = queryString.Count();
            var skipValue = pagination.GetSkipValue();
            var dataList = await queryString.Skip(skipValue).Take(pagination.PerPage).ToListAsync();
            var categories = _mapper.Map<List<CategoryViewModel>>(dataList);
            var pages = pagination.GetPages(dataCount);
            var result = new ResponseDto
            {
                data = categories,
                meta = new Meta
                {
                    page = pagination.Page,
                    perpage = pagination.PerPage,
                    pages = pages,
                    total = dataCount,
                }
            };
            return result;
        }


        public async Task<int> Create(CreateCategoryDto dto)
        {
            var category = _mapper.Map<Category>(dto);
            await _db.Categories.AddAsync(category);
            await _db.SaveChangesAsync();
            return category.Id;
        }


        public async Task<int> Update(UpdateCategoryDto dto)
        {
            var category = await _db.Categories.SingleOrDefaultAsync(x => !x.IsDelete && x.Id == dto.Id);
            if(category == null)
            {
                throw new EntityNotFoundException();
            }
            var updateCategory = _mapper.Map<UpdateCategoryDto, Category>(dto, category);
            _db.Categories.Update(updateCategory);
            await _db.SaveChangesAsync();
            return updateCategory.Id;
        }


        public async Task<int> Delete(int id)
        {
            var category = await _db.Categories.SingleOrDefaultAsync(x => x.Id == id && !x.IsDelete);
            if (category == null)
            {
                throw new EntityNotFoundException();
            }
            category.IsDelete = true;
            _db.Categories.Update(category);
            await _db.SaveChangesAsync();
            return category.Id;
        }



        public async Task<UpdateCategoryDto> Get(int id)
        {
            var category = await _db.Categories.SingleOrDefaultAsync(x => x.Id == id && !x.IsDelete);
            if (category == null)
            {
                throw new EntityNotFoundException();
            }
            return _mapper.Map<UpdateCategoryDto>(category);
        }



    }
}
