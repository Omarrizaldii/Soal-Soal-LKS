using System.Text.Json.Serialization;

namespace EsemkaLaundry.Helper
{
    public class Response
    {
        public int Code { get; set; }
        [JsonIgnore(Condition =JsonIgnoreCondition.WhenWritingNull)]
        public string Message { get; set; }

        [JsonIgnore(Condition =JsonIgnoreCondition.WhenWritingNull)]
        public object Data { get; set; }
    }
}
