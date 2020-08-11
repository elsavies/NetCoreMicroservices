using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Auth_API.Models.Request
{

    [DataContract]
    public class SignUpRequestModel
    {
        [DataMember(Name = "email")]
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [DataMember(Name = "username")]
        [StringLength(32, ErrorMessage = "Invalid Username(at least 4 chars and 32 chars maximun, alphanumeric only)", MinimumLength = 4)]
        [Required]
        public string Username { get; set; }

        [DataMember(Name = "fullname")]
        public string Fullname { get; set; }

        [DataMember(Name = "password")]
        [Required]
        public string Password { get; set; }

        [DataMember(Name = "profile_picture")]
        public string ProfilePicture { get; set; }

        [DataMember(Name = "profile_thumbnail")]
        public string ProfileThumbnail { get; set; }

        [DataMember(Name = "key")]
        [Required]
        public string Key { get; set; }
    }
}
