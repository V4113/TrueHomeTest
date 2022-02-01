using System.Text.Json.Serialization;

namespace TestTrueHome.Model.DTO
{
    public class ActivityToList
    {
        public int Id { get; set; }
        [JsonIgnore]
        public int propery_id { get; set; }
        public DateTime schedule { get; set; }
        public string title { get; set; }
        public DateTime created_at { get; set; }
        [JsonIgnore]
        public DateTime updated_at { get; set; }
        public string status { get; set; }
        public string condition { get; set; }

        public PropertyDTO property { get; set; }

        public SurveyDTO survey { get; set; }
    }
}
