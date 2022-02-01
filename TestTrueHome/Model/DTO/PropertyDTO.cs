using System.Text.Json.Serialization;

namespace TestTrueHome.Model.DTO
{
    public class PropertyDTO
    {
        public int id { get; set; }
        public string title { get; set; }
        public string address { get; set; }
        [JsonIgnore]
        public string description { get; set; }
        [JsonIgnore]
        public DateTime created_at { get; set; }
        [JsonIgnore]
        public DateTime updated_at { get; set; }
        [JsonIgnore]
        public DateTime disabled_at { get; set; }
        [JsonIgnore]
        public string status { get; set; }
    }
}
