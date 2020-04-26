namespace ProjectAccess.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ProjectAccess.Areas.Identity.Data;

    public class Project
    {
        /// <summary>
        /// The project primary key.
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// The project name.
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// The project description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The associated tasks with a project.
        /// </summary>
        public virtual ICollection<Task> Tasks { get; set; }

        /// <summary>
        /// The project members.
        /// </summary>
        public virtual ICollection<AppUserProjects> UserProjects {get; set;}

        /// <summary>
        /// The Creation date.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// The Ending date.
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}
