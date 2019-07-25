using Api.ResultWrapper.AspNetCore.ResponseModel;
using Application.Core.Dto.Order;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.Orders
{
    public interface IOrderService
    { 
        Task<IEnumerable<OrderDto>> GetAll(string searchText);
        Task<int> GetPendingCount();
        Task<PagedResultDto<OrderDto>> GetAllPaged(string searchText, int skip = 0, int maxResultCount = 10);
        Task<OrderDto> Get(string id);
        Task<OrderDto> Save(OrderDto input, string userId);
        Task<OrderDto> Update(OrderDto input, string userId);
        Task<bool> PickOrderItem(string itemId);
        Task<bool> PickOrder(string orderId);
        Task<List<OrderDto>> SaveOrUpdate(List<OrderDto> inputs);
        Task<bool> Delete(string id);
    }
}
