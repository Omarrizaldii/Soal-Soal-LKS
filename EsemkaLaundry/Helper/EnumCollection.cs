using System.Text.Json.Serialization;

namespace EsemkaLaundry.Helper
{
    public class EnumCollection
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum UserGender
        {
            Female = 0,
            Male = 1
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum UserRole
        {
            Admin = 0,
            User = 1
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum ServiceCategory
        {
            Kiloan = 0,
            Satuan = 1,
            PerlengkapanBayi = 2,
            Helm = 3,
            Sepatu = 4,
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum ServiceUnit
        {
            KG = 0,
            Piece = 1
        }
    }
}