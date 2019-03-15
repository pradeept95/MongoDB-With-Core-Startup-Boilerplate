using Api.Helper.ContentWrapper.Core.BaseApiController;
using Api.Helper.ContentWrapper.Core.ResponseModel; 
using Application.Core.Dto.Order;
using Application.Services.Orders;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Application.Area.ApiControllers
{
    public class OrderController : BaseApiController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
         
        [HttpGet("GetAll")]
        public async Task<ListResultDto<OrderDto>> GetAll([FromQuery]string searchText)
        {
            var cu = this.currentUserId; 

            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            var data =  await _orderService.GetAll(searchText); 
            return new ListResultDto<OrderDto>(data.ToList());
        }

        [HttpGet("GetOpenOrderCount")]
        public async Task<ResultDto<int>> GetOpenOrderCount()
        { 
            var data = await _orderService.GetPendingCount();
            return new ResultDto<int>(data);
        }

        [HttpGet("GetAllPaged")]
        public async Task<PagedResultDto<OrderDto>> GetAll(string searchText, int skip = 0, int maxResultCount = 10)
        { 
           return await _orderService.GetAllPaged(searchText, skip, maxResultCount); 
        }
         
        [HttpGet("Get/{id}")]
        public async Task<ResultDto<OrderDto>> Get(string id)
        {
            var result = await _orderService.Get(id) ?? new OrderDto();
            return new ResultDto<OrderDto>(result);
        }
          
        [HttpPost("Save")]
        public async Task<ResultDto<OrderDto>> Save([FromBody] OrderDto input)
        {

            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value; 

            var result = await _orderService.Save(input, userId);
            return new ResultDto<OrderDto>(result);
        }

        [HttpPut("Update")]
        public async Task<ResultDto<OrderDto>> Update([FromBody] OrderDto input)
        { 
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var result = await _orderService.Update(input, userId);
            return new ResultDto<OrderDto>(result);
        }

        [HttpPost("pickOrderItem")]
        public async Task<ResultDto<bool>> PickOrderItem([FromQuery] string itemId)
        {

            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var result = await _orderService.PickOrderItem(itemId);
            return new ResultDto<bool>(result);
        }

        [HttpPost("pickOrder")]
        public async Task<ResultDto<bool>> PickOrder([FromQuery] string orderId)
        {

            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var result = await _orderService.PickOrder(orderId);
            return new ResultDto<bool>(result);
        }


        [HttpDelete("Delete/{id}")]
        public async Task<ResultDto<bool>> Delete(string id)
        {
            await _orderService.Delete(id);
            return new ResultDto<bool>(true);
        }
    }
}
