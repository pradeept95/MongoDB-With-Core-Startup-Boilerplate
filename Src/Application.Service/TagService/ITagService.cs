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
