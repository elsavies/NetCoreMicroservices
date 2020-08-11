using Microsoft.AspNetCore.Identity;
using System.Runtime.Serialization;

namespace Auth_Data.Entities
{
    [DataContract]
    public class ApplicationUserRole : IdentityUserRole<long>
    {       
        #region
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
        #endregion
    }
}
