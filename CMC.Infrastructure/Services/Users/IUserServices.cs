using CMC.Core.Dtos;
using CMC.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMC.Infrastructure.Services.Users
{
    public interface IUserServices
    {

        Task<ResponseDto> GetAll(Pagination pagination, Query query);
        Task<string> Create(CreateUserDto dto);
        Task<string> Update(UpdateUserDto dto);
        Task<string> Delete(string id);
        Task<UpdateUserDto> Get(string id);
        Task<List<UserViewModel>> GetAuthorList();
        UserViewModel GetUserByUsername(string username);
        Task<string> setFCMToUser(string userId, string fcmTocken);
        Task<byte[]> ExportToExcel();

    }
}
