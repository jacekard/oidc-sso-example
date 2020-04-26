namespace ProjectAccess.Areas.Identity.Data
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class AppRole : IdentityRole<long>
    {
        public virtual ICollection<AppUserRole> UserRoles { get; set; }

        /// <summary>
        /// The role description.
        /// </summary>
        public string Description { get; set; }
    }
}
