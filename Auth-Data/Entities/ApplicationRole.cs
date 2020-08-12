using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Auth_Data.Entities
{
    [DataContract]
    public class ApplicationRole : IdentityRole<long>
    {
        #region relations
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; }

        #endregion
    }
}
