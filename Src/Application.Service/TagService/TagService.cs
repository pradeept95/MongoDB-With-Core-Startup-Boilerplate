using Application.Core.Models;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using MongoDB.Bson;
using System.Linq;
using AspNetCore.MongoDb.Repository;
using Api.ResultWrapper.AspNetCore.WrapperModel;

namespace Application.Services.AppUser
{
    public class TagService : ITagService
    { 
        private readonly IMongoRepository<TagLocationMapping, ObjectId> _repository;
        private readonly IOptions<Settings> _settings; 

        public TagService(IOptions<Settings> settings, IMongoRepository<TagLocationMapping, ObjectId> repository)
        {
            _repository = repository;
            _settings = settings;  
        } 

        public async Task<bool> FlashTagByTagAddress(string tagAddress)
        {
            try
            {
                var host = _settings.Value.DropPositionHostUrl;
                var endpoint = host + "/api/tag/flashGreenTag/" + tagAddress;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(host);
                    client.DefaultRequestHeaders.Accept.Clear();
                    var response = await client.GetAsync(endpoint);
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.Write(response);
                        throw new ApiException(response.ReasonPhrase);
                    }
                    var content = response.Content.ReadAsStringAsync();
                }
                return (true);
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.Message);
            }
        }

        public async Task<bool> FlashTagByLocation(string locationName)
        {
            try
            {
                //get the tag address by location name
                var location = _repository.GetAll().FirstOrDefault(x => x.location == locationName);
                if(location == null)
                {
                    throw new ApiException("No Tag found in the current location :"+ locationName);
                }

                var host = _settings.Value.DropPositionHostUrl;
                var endpoint = host + "/api/tag/flashGreenTag/" + location.tagAddress;
                Console.Write(endpoint);
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(host);
                    client.DefaultRequestHeaders.Accept.Clear();
                    var response = await client.GetAsync(endpoint);
                    if(!response.IsSuccessStatusCode)
                    {
                        Console.Write(response);
                        throw new ApiException(response.ReasonPhrase);
                    }
                    var content = response.Content.ReadAsStringAsync();
                }
                return (true);
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.Message);
            }
        }

        public async Task<bool> displayBarcode(string tagAddress, string barcodeValue)
        {
            try
            { 
                string final = string.Empty;
                foreach (char c in barcodeValue)
                {
                    final += int.Parse(c.ToString()) + 30 + " ";
                }

                var host = _settings.Value.DropPositionHostUrl;
                var endpoint = host + "/api/tag/flashBarcode/" + tagAddress + "/" + final;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(host);
                    client.DefaultRequestHeaders.Accept.Clear();
                    var response = await client.GetAsync(endpoint);
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.Write(response);
                        throw new ApiException(response.ReasonPhrase);
                    }
                    var content = response.Content.ReadAsStringAsync();
                }
                return (true);
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.Message);
            }
        }
         
        public async Task<bool> stopFlashByTagAddress(string tagAddress)
        {
            try
            {
                var host = _settings.Value.DropPositionHostUrl;
                var endpoint = host + "/api/tag/stopFlash/" + tagAddress;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(host);
                    client.DefaultRequestHeaders.Accept.Clear();
                    var response = await client.GetAsync(endpoint);
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.Write(response);
                        throw new ApiException(response.ReasonPhrase);
                    }
                    var content = response.Content.ReadAsStringAsync();
                }
                return (true);
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.Message);
            }
        }

        public async Task<bool> stopFlashByLocation(string locationName)
        {
            try
            {
                //get the tag address by location name 
                var location = _repository.GetAll().FirstOrDefault(x => x.location == locationName);
                if (location == null)
                {
                    throw new ApiException("No Tag found in the current location :" + locationName);
                }

                var host = _settings.Value.DropPositionHostUrl;
                var endpoint = host + "/api/tag/stopFlash/" + location.tagAddress;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(host);
                    client.DefaultRequestHeaders.Accept.Clear();
                    var response = await client.GetAsync(endpoint);
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.Write(response);
                        throw new ApiException(response.ReasonPhrase);
                    }
                    var content = response.Content.ReadAsStringAsync();
                }
                return (true);
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.Message);
            }
        }

        public async Task<bool> clearScreen(string tagAddress)
        {
            try
            {
                var host = _settings.Value.DropPositionHostUrl;
                var endpoint = host + "/api/tag/clear/" + tagAddress;
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(host);
                    client.DefaultRequestHeaders.Accept.Clear();
                    var response = await client.GetAsync(endpoint);
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.Write(response);
                        throw new ApiException(response.ReasonPhrase);
                    }
                    var content = response.Content.ReadAsStringAsync();
                }
                return (true);
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.Message);
            }
        } 
    }
}
