
using Api.Helper.ContentWrapper.Core.ResponseModel;
using KNN.NULLPrinter.Core.Dto.AppUser;
using KNN.NULLPrinter.Core.Dto.Authenticate;
using KNN.NULLPrinter.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KNN.NULLPrinter.Services.AppUser
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
