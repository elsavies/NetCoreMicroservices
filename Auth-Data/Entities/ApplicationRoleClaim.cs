using Microsoft.AspNetCore.Identity;

namespace Auth_Data.Entities
{
    public class ApplicationRoleClaim : IdentityRoleClaim<long>
    {
        #region relations
        public virtual ApplicationRole Role { get; set; }
        #endregion
    }
}
