using Application.Core.Security.PasswordHasher;
using Application.Core.Dto.AppUser;
using Application.Core.Dto.Authenticate;
using Application.Core.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.MongoDb.Repository;
using Api.ResultWrapper.AspNetCore.WrapperModel;
using Api.ResultWrapper.AspNetCore.ResponseModel;

namespace Application.Services.AppUser
{
    public class AppUsersService : IAppUsersService
    { 
        private readonly IMongoRepository<AppUsers, ObjectId> _repository; 
         

        public AppUsersService(IMongoRepository<AppUsers, ObjectId> repository)
        {
            _repository = repository; 
        }

        public async Task<AppUsers> Authenticate(LoginRequestDto input)
        {
            var hashedPassword = PasswordHasher.Hash(input.Password);
            var user = _repository.GetAll().FirstOrDefault(x => x.UserName == input.UsernameOrEmail && x.Password == hashedPassword);

            // return null if user not found
            if (user == null)
            {
                throw new ApiException("Invalid Username or Password", 500);
            }
            return user;
        }

        public async Task<IEnumerable<AppUsersDto>> GetAll()
        {
            try
            {
                var result = await _repository.GetAllListAsync();

                return result.Select(x => new AppUsersDto
                {
                    Id = x.Id.ToString(),
                    FirstName = x.FirstName,
                    MiddleName = x.MiddleName,
                    LastName = x.LastName,
                    Email = x.Email,
                    CreatedAt = x.CreatedAt,
                    UserName = x.UserName,
                    IsActive = x.IsActive,
                    IsConfirmed = x.IsConfirmed,
                    UpdatedAt = x.UpdatedAt,
                    FullName = string.IsNullOrEmpty(x.MiddleName) ? x.FirstName + " " + x.LastName : x.FirstName + " " + x.MiddleName + " " + x.LastName
                }); 
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<PagedResultDto<AppUsersDto>> GetAllPaged(string searchText, int skip = 0, int maxResultCount = 10)
        {
            try
            {
                var result = await _repository.GetAllListAsync();
                var res = result.Select(x => new AppUsersDto
                {
                    Id = x.Id.ToString(),
                    FirstName = x.FirstName,
                    MiddleName = x.MiddleName,
                    LastName = x.LastName,
                    Email = x.Email,
                    CreatedAt = x.CreatedAt,
                    UserName = x.UserName,
                    IsActive = x.IsActive,
                    IsConfirmed = x.IsConfirmed,
                    UpdatedAt = x.UpdatedAt,
                    FullName = string.IsNullOrEmpty(x.MiddleName) ? x.FirstName + " " + x.LastName : x.FirstName + " " + x.MiddleName + " " + x.LastName
                });

                if (!string.IsNullOrEmpty(searchText))
                {
                    res = res.Where(x => x.FirstName.ToLower().Contains(searchText)
                    || x.MiddleName.ToLower().Contains(searchText)
                    || x.LastName.ToLower().Contains(searchText)
                    || x.Email.ToLower().Contains(searchText)
                    || x.UserName.ToLower().Contains(searchText));
                }  
                return new PagedResultDto<AppUsersDto>(res.Count(), res.Skip(skip).Take(maxResultCount).ToList());
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<AppUsersDto> Get(string id)
        {
            var result = await _repository.GetAsync(new ObjectId(id));
            if (result == null)
            {
                throw new ApiException("Your request is not valid", 500);
            }

            return new AppUsersDto
            {
                Id = result.Id.ToString(),
                FirstName = result.FirstName,
                MiddleName = result.MiddleName,
                LastName = result.LastName,
                Email = result.Email,
                CreatedAt = result.CreatedAt,
                UserName = result.UserName,
                IsActive = result.IsActive,
                Password = result.Password,
                IsConfirmed = result.IsConfirmed,
                UpdatedAt = result.UpdatedAt,
                FullName = string.IsNullOrEmpty(result.MiddleName) ? result.FirstName + " " + result.LastName : result.FirstName + " " + result.MiddleName + " " + result.LastName
            };
        }

        public async Task<AppUsersDto> Save(AppUsersDto input, string userId)
        {
            if (input.Password != input.ConfirmPassword)
            {
                throw new ApiException("Password and Confirm Password does not match.", 500);
            }

            var saveModel = new AppUsers
            { 
                FirstName = input.FirstName,
                MiddleName = input.MiddleName,
                LastName = input.LastName,
                Email = input.Email,
                UserName = input.UserName,
                IsActive = input.IsActive,
                IsConfirmed = true,
                Password = PasswordHasher.Hash(input.Password),
                CreatedAt = DateTime.Now,
                CreatedBy_AppUsers_Id = new ObjectId(userId)
            }; 
            try
            {
                //await _repository.ValidateAppUser(saveModel);
            }
            catch (Exception ex)
            { 
                throw new ApiException(ex.Message, 500);
            }
           
            await _repository.InsertAsync(saveModel);
            return input;
        }

        public async Task<AppUsersDto> Update(AppUsersDto input, string userId)
        {

            var appUser = await _repository.GetAsync(new ObjectId(input.Id));

            if (appUser == null)
            {
                throw new ApiException("Invalid Request", 500);
            }

            appUser.FirstName = input.FirstName;
            appUser.MiddleName = input.MiddleName;
            appUser.LastName = input.LastName;
            appUser.Email = input.Email;
            appUser.UserName = input.UserName;
            appUser.IsActive = input.IsActive;
            appUser.UpdatedAt = DateTime.Now;
            appUser.UpdatedBy_AppUsers_Id = new ObjectId(userId);

            try
            {
                //await _repository.ValidateAppUser(saveModel);
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.Message, 500);
            }
            await _repository.UpdateAsync(appUser);
            return input;
        }

        public async Task<bool> ChangePassword(ChangePasswordDto input, string userId)
        {
            if (input.Password != input.ConfirmPassword)
            {
                throw new ApiException("Password and Confirm Password does not match.", 500);
            }

            var saveModel = await _repository.GetAsync(new ObjectId(input.Id));
            if(saveModel == null)
            {
                throw new ApiException("Invalid Request.", 500);
            }

            saveModel.Password = PasswordHasher.Hash(input.Password);
            saveModel.UpdatedBy_AppUsers_Id = new ObjectId(userId);
            await _repository.UpdateAsync(saveModel);
            return true;
        }

        public async Task<bool> Delete(string id)
        {
            await _repository.DeleteAsync(new ObjectId(id));
            return true;
            //if (!))
            //{
            //    throw new ApiException("Unable to Delete Row");
            //}
            //return true;
        }
    }
}
