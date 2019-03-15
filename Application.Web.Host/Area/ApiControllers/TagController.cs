using Api.Helper.ContentWrapper.Core.BaseApiController;
using Application.Core.Models;
using Application.Services.AppUser;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Application.Area.ApiControllers
{
    public class TagController : BaseApiController
    {
        private ITagService _tagService;
        private readonly IOptions<Settings> _settings;

        public TagController(IOptions<Settings> settings, ITagService tagService)
        {
            _tagService = tagService;
            _settings = settings;
        }
         
        [HttpGet("flashByLocation")]
        public async Task<bool> FlashTagByLocation([FromQuery]string locationName)
        {
            return await _tagService.FlashTagByLocation(locationName);
        }

        [HttpGet("flashByTag")]
        public async Task<bool> FlashTagByTagAddress([FromQuery]string tagAddress)
        {
            return await _tagService.FlashTagByTagAddress(tagAddress);
        }

        [HttpGet("stopFlashByLocation")]
        public async Task<bool> StopFlashByLocation([FromQuery]string locationName)
        {
            return await _tagService.stopFlashByLocation(locationName);
        }

        [HttpGet("stopFlashByTag")]
        public async Task<bool> StopFlashByTagAddress([FromQuery]string tagAddress)
        {
            return await _tagService.stopFlashByTagAddress(tagAddress);
        }
    }
}
