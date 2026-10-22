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

        public async Task<List<Hotel>> GetHotels(string? city = null, int? minRating = null)
        {
            var query = new List<string>();
            if (!string.IsNullOrWhiteSpace(city))
                query.Add($"city={city}");
            if (minRating.HasValue)
                query.Add($"minRating={minRating.Value}");

            var url = "_api/hotels" + (query.Count > 0 ? "?" + string.Join("&", query) : "");
            return await _client.GetFromJsonAsync<List<Hotel>>(url)??new List<Hotel>();
        }
        public async Task<Hotel> GetHotel(int id) => await _client.GetFromJsonAsync<Hotel>($"_api/hotels/{id}");

        public async Task<bool> CreateHotel(Hotel hotel)=> (await _client.PostAsJsonAsync("_api/hotels", hotel)).IsSuccessStatusCode;
        
        public async Task<bool> Update(int id, Hotel hotel)
        {
            var response = await _client.PutAsJsonAsync($"_api/hotels/{id}", hotel);
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> Update(int id, HotelPatch hotel)
        {
            var response = await _client.PatchAsJsonAsync($"_api/hotels/{id}", hotel);
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> Delete(int id, Hotel hotel)
        {
            var response = await _client.DeleteAsync($"_api/hotels/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
