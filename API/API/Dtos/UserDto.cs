using System;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class UserDto : IEmailable
    {
        public string FristName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        [EmailAddress]
        public string EmailAddress { get; set; }
    }

    public interface IEmailable
    {
        string EmailAddress { get; set; }
    }
}
