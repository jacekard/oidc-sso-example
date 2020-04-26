using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectAccess.Models
{
    public class ProjectRights
    {
        public Project Project { get; set; }

        public bool HasRights { get; set; }

        public ProjectRights(Project project, bool hasRights)
        {
            this.Project = project;
            this.HasRights = hasRights;
        }
    }
}
