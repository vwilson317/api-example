using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class FormattedUserDto
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        [EmailAddress]
        public string EmailAddress { get; set; }
        public string UserId { get; set; }
    }
}
