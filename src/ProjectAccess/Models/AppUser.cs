namespace ProjectAccess.Areas.Identity.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Identity;
    using ProjectAccess.Models;

    public class AppUser : IdentityUser<long>
    {
        /// <summary>
        /// The user permitted projects
        /// </summary>
        public virtual ICollection<AppUserProjects> UserProjects { get; set; }

        /// <summary>
        /// The user roles.
        /// </summary>
        public virtual ICollection<AppUserRole> UserRoles { get; set; }

        /// <summary>
        /// The user claims.
        /// </summary>
        public virtual ICollection<IdentityUserClaim<long>> Claims { get; set; }

        /// <summary>
        /// The user logins.
        /// </summary>
        public virtual ICollection<IdentityUserLogin<long>> Logins { get; set; }

        /// <summary>
        /// The user tokens.
        /// </summary>
        public virtual ICollection<IdentityUserToken<long>> Tokens { get; set; }

        /// <summary>
        /// The user full name.
        /// </summary>
        [PersonalData]
        public string Name { get; set; }

        /// <summary>
        /// The user date of birth.
        /// </summary>
        [PersonalData]
        public DateTime DOB { get; set; }

        public bool IsAdmin
        {
            get
            {
                return this.UserRoles.Any(x => x.Role.Name.Equals(RoleNames.Admin));
            }
        }
    }
}
