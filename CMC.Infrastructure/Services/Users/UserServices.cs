using AutoMapper;
using CMC.Core.Constants;
using CMC.Core.Dtos;
using CMC.Core.Enums;
using CMC.Core.Exceptions;
using CMC.Core.ViewModels;
using CMC.Data;
using CMC.Data.Models;
using CMC.Infrastructure.Helpers;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMC.Infrastructure.Services.Users
{
    public class UserServices : IUserServices
    {
        private readonly CMCDbContext _db;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IFileService _fileService;
        private readonly IEmailService _emailService;

        public UserServices(IEmailService emailService , CMCDbContext db, IMapper mapper, UserManager<User> userManager, IFileService fileService)
        {
            _db = db;
            _mapper = mapper;
            _userManager = userManager;
            _fileService = fileService;
            _emailService = emailService;
        }



        public async Task<byte[]> ExportToExcel()
        {
            var user = await _db.Users.Where(x => !x.IsDelete).ToListAsync();
            return ExcelHelpers.ToExcel(new Dictionary<string, ExcelColumn>
            {
                {"FullName", new ExcelColumn("FullName", 0)},
                {"Email", new ExcelColumn("Email", 1)},
                {"Phone", new ExcelColumn("Phone", 2)}
            }, new List<ExcelRow>(user.Select(e => new ExcelRow
            {
                Values = new Dictionary<string, string>
                {
                    {"FullName", e.FullName},
                    {"Email", e.Email},
                    {"Phone", e.PhoneNumber}
                }
            })));
        }


        public async Task<string> setFCMToUser(string userId , string fcmTocken)
        {
            var user = _db.Users.SingleOrDefault(x => x.Id == userId && !x.IsDelete);
            if (user == null)
            {
                throw new EntityNotFoundException();
            }
            user.FCMToken = fcmTocken;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
            return user.Id;
        }

        public  UserViewModel GetUserByUsername(string username) 
        {
            var user =  _db.Users.SingleOrDefault(x => x.UserName == username && !x.IsDelete);
            if (user == null)
            {
                throw new EntityNotFoundException();
            }
            return _mapper.Map<UserViewModel>(user);
        }



        public async Task<List<UserViewModel>> GetAuthorList()
        {
            var users = await _db.Users.Where(x => !x.IsDelete && x.UserType == UserType.ArticleAuthor).ToListAsync();
            return _mapper.Map<List<UserViewModel>>(users);
        }


        public async Task<ResponseDto> GetAll(Pagination pagination , CMC.Core.Dtos.Query query)
        {
            var queryString = _db.Users.Where(x => !x.IsDelete && (x.FullName.Contains(query.GeneralSearch) || x.PhoneNumber.Contains(query.GeneralSearch)|| string.IsNullOrWhiteSpace(query.GeneralSearch) || x.Email.Contains(query.GeneralSearch))).AsQueryable();

            var dataCount = queryString.Count();
            var skipValue = pagination.GetSkipValue();
            var dataList = await queryString.Skip(skipValue).Take(pagination.PerPage).ToListAsync();
            var users = _mapper.Map<List<UserViewModel>>(dataList);
            var pages = pagination.GetPages(dataCount);
            var result = new ResponseDto
            {
                data = users,
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

        public async Task<string> Create(CreateUserDto dto)
        {
            var emaileOrPhoneIsExit = _db.Users.Any(x => !x.IsDelete && (x.Email == dto.Email || x.PhoneNumber == dto.PhoneNumber));
            if(emaileOrPhoneIsExit)
            {
                throw new DuplicateEmailOrPhoneException();
            }
            var user = _mapper.Map<User>(dto);
            user.UserName = dto.Email;

            if(dto.Image != null)
            {
                user.ImageUrl = await _fileService.SaveFile(dto.Image, FolderNames.ImageFolder);
            }

            var password = GenratePassword();

            var result = await _userManager.CreateAsync(user, password);
            if(!result.Succeeded) 
            {
                throw new OperationFailedException();
            }

            await _emailService.Send(user.Email, "New Account !", $"Username is : {user.Email} and Passwored {password}");

            return user.Id;
        }



        public async Task<string> Update(UpdateUserDto dto)
        {
            var emaileOrPhoneIsExit = _db.Users.Any(x => !x.IsDelete && (x.Email == dto.Email || x.PhoneNumber == dto.PhoneNumber) && x.Id != dto.Id);
            if (emaileOrPhoneIsExit)
            {
                throw new DuplicateEmailOrPhoneException();
            }

            var user = await _db.Users.FindAsync(dto.Id);
            var updateUser =  _mapper.Map<UpdateUserDto, User>(dto , user);
            if (dto.Image != null)
            {
                user.ImageUrl = await _fileService.SaveFile(dto.Image, FolderNames.ImageFolder);
            }
            _db.Users.Update(updateUser);
            await _db.SaveChangesAsync();

            return user.Id;

        }


        public async Task<UpdateUserDto> Get(string id)
        {
            var user = await _db.Users.SingleOrDefaultAsync(x => x.Id == id && !x.IsDelete);
            if (user == null)
            {
                throw new EntityNotFoundException();
            }
            return _mapper.Map<UpdateUserDto>(user);
        }



        public async Task<string> Delete(string id)
        {
            var user =await _db.Users.SingleOrDefaultAsync(x => x.Id == id && !x.IsDelete);
            if(user == null)
            {
                throw new EntityNotFoundException();
            }
            user.IsDelete = true;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
            return user.Id;
        }


        private string GenratePassword()
        {
            return Guid.NewGuid().ToString().Substring(1,8);
        }

        
    }
}
