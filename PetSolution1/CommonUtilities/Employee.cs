using Newtonsoft.Json;
using System;
using System.Globalization;

namespace PetSolution1.CommonUtilities
{
    public class Employee
    {
        [JsonProperty("id")]
        public string Id { get; set; }
       
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("age")]
        public int Age { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("dob")]
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly DOB { get; set; } = new DateOnly();

        [JsonProperty("phonenumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

    }


    public class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        private const string Format = "yyyy-MM-dd";

        public override DateOnly ReadJson(JsonReader reader,
            Type objectType,
            DateOnly existingValue,
            bool hasExistingValue,
            JsonSerializer serializer) => DateOnly.ParseExact((string)reader.Value, Format, DateTimeFormatInfo.CurrentInfo);

        public override void WriteJson(JsonWriter writer,
            DateOnly value,
            JsonSerializer serializer) => writer.WriteValue(value.ToString(Format, DateTimeFormatInfo.CurrentInfo));
    }
}