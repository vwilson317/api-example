using Newtonsoft.Json;

namespace API.DataAccess
{
    public class User
    {
        public string FristName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
