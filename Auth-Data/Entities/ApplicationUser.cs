using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Auth_Data.Entities
{
    [DataContract]
    public class ApplicationUser : IdentityUser<long>
    {

        [DataMember(Name = "name")]
        public string Fullname { get; set; }

        [DataMember(Name = "profile_picture")]
        public string ProfilePicture { get; set; }

        [DataMember(Name = "profile_thumbnail")]
        public string ProfileThumbnail { get; set; }

        [DataMember(Name = "created_date")]
        public System.DateTime CreatedDate { get; set; }

        [DataMember(Name = "updated_date")]
        public System.DateTime UpdateDate { get; set; }

        [DataMember(Name = "active")]
        public bool? Active { get; set; }

        [DataMember(Name = "version")]
        public string Version { get; set; }

        [StringLength(15)]
        public override string PhoneNumber { get; set; }

        [DataMember(Name = "id_country")]       
        public long? Id_Country { get; set; }

        [DataMember(Name = "id_state")]       
        public long? Id_State { get; set; }

        [DataMember(Name = "id_city")]        
        public long? Id_City { get; set; }

        #region relations

        public virtual ICollection<ApplicationUserClaim> Claims { get; set; }
        public virtual ICollection<ApplicationUserLogin> Logins { get; set; }
        public virtual ICollection<ApplicationUserToken> Tokens { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        //public virtual ICollection<Kardex> Kardices { get; set; }        
        
        //public virtual Country Country { get; set; }
        //public virtual State State { get; set; }
        //public virtual City City { get; set; }

        #endregion
    }
}
