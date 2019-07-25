using Api.Helper.ContentWrapper.Core.ResponseModel;
using Application.Core.Dto.File;
using Application.Core.Models;
using Microsoft.AspNetCore.Hosting;
using MongoDB.Bson;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.MongoDb.Repository;

namespace Application.Service.Setting
{
    public class SettingService : ISettingService
    { 
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMongoRepository<FileDirectorySetting, ObjectId> _repository;

        public SettingService(IHostingEnvironment hostingEnvironment, IMongoRepository<FileDirectorySetting, ObjectId> repository)
        { 
            _hostingEnvironment = hostingEnvironment;
            _repository = repository;
        }

        public async Task<ResultDto<FileDirectorySettingDto>> GetEdit()
        {
            var result = _repository.GetAll();
            if (!result.Any())
            {
                return new ResultDto<FileDirectorySettingDto>(new FileDirectorySettingDto {
                    Id = ObjectId.Empty.ToString()
                });
            }

            return new ResultDto<FileDirectorySettingDto>(new FileDirectorySettingDto
            {
                Id = result.First().Id.ToString(),
                ArchiveDirectoryPath = result.First().ArchiveDirectoryPath,
                DefaultDirectoryPath = result.First().DefaultDirectoryPath,
                DirectoryPath = result.First().DirectoryPath
            }); 
        }

        public async Task<ResultDto<FileDirectorySettingDto>> Update(FileDirectorySettingDto input)
        {
          
            if(new MongoDB.Bson.ObjectId(input.Id) == MongoDB.Bson.ObjectId.Empty)
            {
                var saveModel = new FileDirectorySetting
                { 
                    ArchiveDirectoryPath = input.ArchiveDirectoryPath,
                    DirectoryPath = input.DirectoryPath,
                    DefaultDirectoryPath = input.DefaultDirectoryPath
                };

                await _repository.InsertAsync(saveModel);
            }
            else
            {
                var saveModel = new FileDirectorySetting
                {
                    Id = new MongoDB.Bson.ObjectId(input.Id),
                    ArchiveDirectoryPath = input.ArchiveDirectoryPath,
                    DirectoryPath = input.DirectoryPath,
                    DefaultDirectoryPath = input.DefaultDirectoryPath
                };

                await _repository.UpdateAsync(saveModel);
            } 
            return new ResultDto<FileDirectorySettingDto>(input);
        }
    }
}
