using Api.ResultWrapper.AspNetCore.ResponseModel;
using Api.ResultWrapper.AspNetCore.WrapperModel;
using Application.Core.Dto.Order;
using Application.Core.Models;
using AspNetCore.MongoDb.Repository;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services.Orders
{
    public class OrderService : IOrderService
    { 
        private readonly IMongoRepository<Order, ObjectId> _repository; 
          
        public OrderService(IMongoRepository<Order, ObjectId> repository)
        {
            _repository = repository;
        }  
         
        public async Task<IEnumerable<OrderDto>> GetAll(string searchText)
        {
            try
            {
                var result = _repository.GetAll();

                if (!string.IsNullOrEmpty(searchText))
                {
                    result = result.Where(x => x.code.ToLower().Contains(searchText.ToLower()));
                }

                return result.Select(x => new OrderDto
                {
                    Id = x.Id.ToString(),
                    code = x.code,
                    createdAt = x.createdAt,
                    updatedAt = x.updatedAt,
                    status = x.status,
                    items = x.items.Select(q => new OrderDetailDto {
                        Id = q.Id.ToString(),
                        code = q.code,
                        description = q.description,
                        location = q.location,
                        quantity = q.quantity,
                        status = q.status
                    }).ToList()
                }).OrderByDescending(x => x.status).ThenBy(x => x.code); 
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<int> GetPendingCount()
        {
            try
            {
                var result = await _repository.GetAllListAsync(); 
                return result.Count(x => x.status);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<PagedResultDto<OrderDto>> GetAllPaged(string searchText, int skip = 0, int maxResultCount = 10)
        {
            try
            {
                var result = await _repository.GetAllListAsync();
                var res = result.Select(x => new OrderDto
                {
                    Id = x.Id.ToString(),
                    code = x.code,
                    createdAt = x.createdAt,
                    updatedAt = x.updatedAt,
                    status = x.status,
                    items = x.items.Select(q => new OrderDetailDto
                    {
                        Id = q.Id.ToString(),
                        code = q.code,
                        description = q.description,
                        location = q.location,
                        quantity = q.quantity,
                        status = q.status
                    }).ToList()
                });

                if (!string.IsNullOrEmpty(searchText))
                {
                    res = res.Where(x => x.code.ToLower().Contains(searchText));
                }  
                return new PagedResultDto<OrderDto>(res.Count(), res.Skip(skip).Take(maxResultCount).ToList());
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        public async Task<OrderDto> Get(string id)
        {
            var result = await _repository.GetAsync(new ObjectId(id));
            if (result == null)
            {
                throw new ApiException("Your request is not valid", 500);
            }

            return new OrderDto
            {
                Id = result.Id.ToString(),
                code = result.code,
                createdAt = result.createdAt,
                updatedAt = result.updatedAt,
                status = result.status,
                items = result.items.Select(q => new OrderDetailDto
                {
                    Id = q.Id.ToString(),
                    code = q.code,
                    description = q.description,
                    location = q.location,
                    quantity = q.quantity,
                    status = q.status
                }).OrderByDescending(x => x.status).ThenBy(x => x.code).ToList()
            };
        }

        public async Task<OrderDto> Save(OrderDto input, string userId)
        { 
            var saveModel = new Order
            {  
                code = input.code,
                createdAt = input.createdAt,
                updatedAt = input.updatedAt,
                status = input.status,
                items = input.items.Select(q => new OrderDetail
                { 
                    code = q.code,
                    description = q.description,
                    location = q.location,
                    quantity = q.quantity,
                    status = q.status
                }).ToList()
            }; 
             
            await _repository.InsertAsync(saveModel);
            return input;
        } 

        public async Task<OrderDto> Update(OrderDto input, string userId)
        { 
            var saveModel = new Order
            {
                code = input.code,
                createdAt = input.createdAt,
                updatedAt = input.updatedAt,
                status = input.status,
                items = input.items.Select(q => new OrderDetail
                {
                    code = q.code,
                    description = q.description,
                    location = q.location,
                    quantity = q.quantity,
                    status = q.status
                }).ToList()
            };
             
            await _repository.UpdateAsync(saveModel);
            return input;
        }

        public async Task<bool> PickOrderItem(string itemId)
        {
            var order = _repository.GetAll().FirstOrDefault(x => x.items.Any(q => q.Id == new ObjectId(itemId)));

            if (order == null)
            {
                throw new Exception("Request is not valid");
            }

            //delete item if not in new list 
            order.updatedAt = DateTime.Now;
            for (int i = 0; i < order.items.Count(); i++)
            {
                if (order.items[i].Id == new ObjectId(itemId))
                {
                    order.items[i].status = false;
                    break;
                }
            }

            //isAllItemPicked
            var isAllItemPicked = order.items.Count() == order.items.Count(x => !x.status);
            if (isAllItemPicked) order.status = false; 
            await _repository.UpdateAsync(order);
            return true;
        }

        public async Task<bool> PickOrder(string orderId)
        {
            var order = await _repository.GetAsync(new ObjectId(orderId));
            if (order == null)
            {
                throw new Exception("Request is not valid");
            }

            //delete item if not in new list 
            order.updatedAt = DateTime.Now;
            order.status = false;
            for (int i = 0; i < order.items.Count(); i++)
            {
                order.items[i].status = false;
            }
            await _repository.UpdateAsync(order);
            return true;
        }

        public async Task<List<OrderDto>> SaveOrUpdate(List<OrderDto> inputs)
        {
            foreach (var input in inputs)
            { 
                var saveModel = new Order
                {
                    code = input.code,
                    createdAt = input.createdAt,
                    updatedAt = input.updatedAt,
                    status = input.status,
                    items = input.items.Select(q => new OrderDetail
                    {
                        Id = ObjectId.GenerateNewId(),
                        code = q.code,
                        description = q.description,
                        location = q.location,
                        quantity = q.quantity,
                        status = q.status
                    }).ToList()
                };
                 
                if(_repository.GetAll().Any(x => x.code == saveModel.code))
                {
                    var order = _repository.GetAll().FirstOrDefault(x => x.code == saveModel.code);

                    if (order == null)
                    {
                        throw new Exception("Request is not valid");
                    }

                    //delete item if not in new list
                    var newIds = saveModel.items.Select(x => x.code).ToList();
                    var removedItems = order.items.Select(x => x.code).Where(x => !newIds.Contains(x));
                    foreach (var removedItemCode in removedItems)
                    {
                        var itemIndex = Array.IndexOf(order.items.ToArray(), order.items.FirstOrDefault(x => x.code == removedItemCode));
                        if (itemIndex > -1)
                        {
                            order.items.RemoveAt(itemIndex);
                        }
                    }

                    order.updatedAt = DateTime.Now;

                    //update or create for each item
                    foreach (var orderItem in saveModel.items)
                    {
                        if (order.items.Any(x => x.code == orderItem.code))
                        {
                            //update to existing field
                            var itemIndex = Array.IndexOf(order.items.ToArray(), order.items.FirstOrDefault(x => x.code == orderItem.code));
                            if (itemIndex > -1)
                            {
                                order.items[itemIndex].quantity = orderItem.quantity;
                                order.items[itemIndex].location = orderItem.location;
                                order.items[itemIndex].description = orderItem.description;
                            }
                        }
                        else
                        {
                            //add new item in the list and update to the database
                            order.items.Add(orderItem);
                        }
                    }

                    await _repository.UpdateAsync(order);
                }
                else
                {
                    await _repository.InsertAsync(saveModel);
                }

            } 
            
            return inputs;
        }

        public async Task<bool> Delete(string id)
        {
            await _repository.DeleteAsync(new ObjectId(id));
            return true;
        } 
    }
}
