using CMC.Core.Dtos;
using CMC.Core.Enums;
using CMC.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMC.Infrastructure.Services.Posts
{
    public interface IPostService
    {
        Task<ResponseDto> GetAll(Pagination pagination, Query query);
        Task<int> Delete(int id);
        Task<int> Create(CreatePostDto dto);
        Task<UpdatePostDto> Get(int id);
        Task<int> UpdateStatus(int id, ContentStatus status);
        Task<List<ContentChangeLogViewModel>> GetLog(int id);
        Task<int> Update(UpdatePostDto dto);
        Task<int> RemoveAttachment(int id); 
    }
}
