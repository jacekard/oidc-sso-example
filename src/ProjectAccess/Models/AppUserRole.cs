using Microsoft.AspNetCore.Identity;

namespace ProjectAccess.Areas.Identity.Data
{
    /// <summary>
    /// Relation between Users and Roles
    /// </summary>
    public class AppUserRole : IdentityUserRole<long>
    {
        /// <summary>
        /// The user Navigation Property
        /// </summary>
        public virtual AppUser User { get; set; }

        /// <summary>
        /// The role Navigation Property
        /// </summary>
        public virtual AppRole Role { get; set; }
    }
}
