
using Api.Helper.ContentWrapper.Core.ResponseModel;
using Application.Core.Dto.AppUser;
using Application.Core.Dto.Authenticate;
using Application.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.AppUser
{
    public interface ITagService
    {
        Task<bool> FlashTagByTagAddress(string tagAddress);
        Task<bool> FlashTagByLocation(string locationName);

        Task<bool> displayBarcode(string tagAddress, string barcodeValue);

        Task<bool> stopFlashByTagAddress(string tagAddress);
        Task<bool> stopFlashByLocation(string locationName);
        Task<bool> clearScreen(string tagAddress);
    }
}
