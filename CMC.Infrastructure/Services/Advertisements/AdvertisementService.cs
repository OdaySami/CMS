using AutoMapper;
using CMC.Core.Dtos;
using CMC.Core.Enums;
using CMC.Core.Exceptions;
using CMC.Core.ViewModels;
using CMC.Data;
using CMC.Data.Models;
using CMC.Infrastructure.Services.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMC.Infrastructure.Services.Advertisements
{
    public class AdvertisementService : IAdvertisementService
    {
        private readonly CMCDbContext _db;
        private readonly IMapper _mapper;
        private readonly IUserServices _userServices;
        private readonly IFileService _fileService;

        public AdvertisementService(CMCDbContext db, IMapper mapper, IUserServices userServices, IFileService fileService)
        {
            _db = db;
            _mapper = mapper;
            _userServices = userServices;
            _fileService = fileService;
        }



        public async Task<List<UserViewModel>> GetAdvertisementOwner()
        {
            var users = await _db.Users.Where(x => !x.IsDelete && x.UserType == UserType.AdvertisementOwner).ToListAsync();
           return   _mapper.Map<List<UserViewModel>>(users);
        }


        public async Task<ResponseDto> GetAll(Pagination pagination, Query query)
        {
            var queryString = _db.Advertisements.Include(x => x.Owner).Where(x => !x.IsDelete).AsQueryable();

            var dataCount = queryString.Count();
            var skipValue = pagination.GetSkipValue();
            var dataList = await queryString.Skip(skipValue).Take(pagination.PerPage).ToListAsync();
            var advertisements = _mapper.Map<List<AdvertisementViewModel>>(dataList);
            var pages = pagination.GetPages(dataCount);
            var result = new ResponseDto
            {
                data = advertisements,
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







        public async Task<int> Delete(int id)
        {
            var advertisements = await _db.Advertisements.SingleOrDefaultAsync(x => x.Id == id && !x.IsDelete);
            if (advertisements == null)
            {
                throw new EntityNotFoundException();
            }
            advertisements.IsDelete = true;
            _db.Advertisements.Update(advertisements);
            await _db.SaveChangesAsync();
            return advertisements.Id;
        }


        public async Task<UpdateAdvertisementDto> Get(int id)
        {
            var advertisements = await _db.Advertisements.SingleOrDefaultAsync(x => x.Id == id && !x.IsDelete);
            if (advertisements == null)
            {
                throw new EntityNotFoundException();
            }
         return _mapper.Map<UpdateAdvertisementDto>(advertisements);
        }


        public async Task<int> Create(CreateAdvertisementDto dto)
        {

            if(dto.StartDate >= dto.EndDate && dto.StartDate < dto.EndDate)
            {
                throw new InvalidDataException();
            }


            var advertisements = _mapper.Map<Advertisement>(dto);
            if(dto.Image != null)
            {
                advertisements.ImageUrl = await _fileService.SaveFile(dto.Image, "Images");
            }

            if (!string.IsNullOrWhiteSpace(dto.OwnerId))
            {
                advertisements.OwnerId = dto.OwnerId;
            }

            await _db.Advertisements.AddAsync(advertisements);
            await _db.SaveChangesAsync();

            if(advertisements.OwnerId == null)
            {
                var userId = await _userServices.Create(dto.Owner);
                advertisements.OwnerId = userId;
                _db.Advertisements.Update(advertisements);
                await _db.SaveChangesAsync();
            }
           
            return advertisements.Id;
        }


        public async Task<int> Update(UpdateAdvertisementDto dto)
        {

            if (dto.StartDate >= dto.EndDate && dto.StartDate < dto.EndDate)
            {
                throw new InvalidDataException();
            }
            var advertisements = await _db.Advertisements.SingleOrDefaultAsync(x => x.Id == dto.Id && !x.IsDelete);
            if (advertisements == null)
            {
                throw new EntityNotFoundException();
            }
            var updatadvertisements = _mapper.Map(dto, advertisements);
            if (dto.Image != null)
            {
                updatadvertisements.ImageUrl = await _fileService.SaveFile(dto.Image, "Images");
            }
             _db.Advertisements.Update(updatadvertisements);
            await _db.SaveChangesAsync();
            return advertisements.Id;
        }
    }
}
