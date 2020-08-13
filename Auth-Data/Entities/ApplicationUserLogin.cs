using Microsoft.AspNetCore.Identity;
using System.Runtime.Serialization;

namespace Auth_Data.Entities
{
    [DataContract]
    public class ApplicationUserLogin : IdentityUserLogin<long>
    {

        #region relations
        public virtual ApplicationUser User { get; set; }
        #endregion
    }
}
