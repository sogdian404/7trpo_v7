namespace _6_webApi.Models
{
    public class HotelPatch
    {

        public string? Name { get; set; }

        public string? City { get; set; }

        public decimal? Rating { get; set; }

        public decimal? PricePerNight { get; set; }

        public bool? IsAvailable { get; set; }
    }
}
