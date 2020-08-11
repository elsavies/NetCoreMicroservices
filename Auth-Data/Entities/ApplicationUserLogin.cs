using Microsoft.AspNetCore.Identity;

namespace Auth_Data.Entities
{
    public class ApplicationUserLogin : IdentityUserLogin<long>
    {

        #region relations
        public virtual ApplicationUser User { get; set; }
        #endregion
    }
}
