using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace Challenge3Api.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public string Flavor { get; set; }

        public string SKU { get; set; }

        public int Volume { get; set; }

        public int ExpirationInDays { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PackagingType PackagingType { get; set; }
    }
}
