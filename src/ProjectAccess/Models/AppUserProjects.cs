namespace ProjectAccess.Areas.Identity.Data
{
    using ProjectAccess.Models;

    /// <summary>
    /// Relation between Users and Projects.
    /// </summary>
    public class AppUserProjects
    {
        /// <summary>
        /// The user Navigation Property.
        /// </summary>
        public virtual AppUser User { get; set; }

        /// The user id.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// The project Navigation Property.
        /// </summary>
        public virtual Project Project { get; set; }

        /// <summary>
        /// The project id.
        /// </summary>
        public long ProjectId { get; set; }
    }
}
