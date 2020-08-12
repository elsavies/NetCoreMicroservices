using Microsoft.AspNetCore.Identity;
using System.Runtime.Serialization;

namespace Auth_Data.Entities
{
    [DataContract]
    public class ApplicationRoleClaim : IdentityRoleClaim<long>
    {
        #region relations
        public virtual ApplicationRole Role { get; set; }
        #endregion
    }
}
