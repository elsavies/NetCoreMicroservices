using Microsoft.AspNetCore.Identity;

namespace Auth_Data.Entities
{
    public class ApplicationUserToken : IdentityUserToken<long>
    {

        #region relations
        public virtual ApplicationUser User { get; set; }
        #endregion
    }
}
