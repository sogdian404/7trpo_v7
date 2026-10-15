using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

using _6_webApi.Models;
namespace _7_wpf
{
    public class HotelApi
    {
        private readonly HttpClient _client;
        public HotelApi(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<Hotel>> GetHotels()
        {
            return await _client.GetFromJsonAsync<List<Hotel>>("api/hotels");
        }
        public async Task<Hotel> GetHotel(int id)
        {
            return await _client.GetFromJsonAsync<Hotel>($"api/hotels/{id}");
        }

        public async Task<bool> CreateHotel(Hotel hotel)
        {
            var response = await _client.PostAsJsonAsync("api/hotels", hotel);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Update(int id, Hotel hotel)
        {
            var response = await _client.PutAsJsonAsync($"api/hotels/{id}", hotel);
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> Update(int id, HotelPatch hotel)
        {
            var response = await _client.PatchAsJsonAsync($"api/hotels/{id}", hotel);
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> Delete(int id, Hotel hotel)
        {
            var response = await _client.DeleteAsync($"api/hotels/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
