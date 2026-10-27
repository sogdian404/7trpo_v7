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

        public async Task<List<Hotel>> GetHotels(string? city = null, decimal? minRating = null)
        {
            var query = new List<string>();
            if (!string.IsNullOrWhiteSpace(city))
                query.Add($"city={city}");
            if (minRating.HasValue)
                query.Add($"minRating={minRating.Value}");

            var url = "api/hotels" + (query.Count > 0 ? "?" + string.Join("&", query) : "");
            return await _client.GetFromJsonAsync<List<Hotel>>(url)??new List<Hotel>();
        }
        public async Task<Hotel> GetHotel(int id) => await _client.GetFromJsonAsync<Hotel>($"api/hotels/{id}");

        public async Task<bool> CreateHotel(Hotel hotel)=> (await _client.PostAsJsonAsync("api/hotels", hotel)).IsSuccessStatusCode;
        
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
