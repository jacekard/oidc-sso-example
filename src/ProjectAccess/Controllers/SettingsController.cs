namespace ProjectAccess.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using ProjectAccess.Models;
    using ProjectAccess.Areas.Identity.Data;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Authorization;

    public class SettingsController : Controller
    {
        private readonly AppDbContext dbContext;

        private readonly UserManager<AppUser> userManager;

        public SettingsController(AppDbContext dbContext, UserManager<AppUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles = RoleNames.Admin)]
        public async Task<IActionResult> Index()
        {
            ViewBag.CurrentUser = HttpContext.User.Identity.Name;

            return View(await dbContext.Users.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await dbContext.Users.FindAsync(id);
            ViewBag.CurrentUser = user.Id;

            var userProjects = user.UserProjects.Select(x => x.Project).ToList();
            var allProjects = await dbContext.Projects.Select(x => new ProjectRights(x, false)).ToListAsync();

            foreach (var project in userProjects)
            {
                allProjects.Where(x => x.Project.Equals(project)).First().HasRights = true;
            }

            return View(allProjects);
        }

        [HttpPut("admin")]
        [Authorize(Roles = RoleNames.Admin)]
        public async Task<IActionResult> UpdateAdminRole(long userId, bool adminRole)
        {
            var user = await dbContext.Users.FindAsync(userId);

            if (user == null)
            {
                return BadRequest();
            }

            if (adminRole)
            {
                await userManager.AddToRoleAsync(user, RoleNames.Admin);
            }
            else
            {
                await userManager.RemoveFromRoleAsync(user, RoleNames.Admin);
            }

            dbContext.Entry(user).State = EntityState.Modified;

            await dbContext.SaveChangesAsync();

            return Ok();
        }


        [HttpDelete("user")]
        [Authorize(Roles = RoleNames.Admin)]
        public async Task<IActionResult> UpdateUserRole(long userId)
        {
            var user = await dbContext.Users.FindAsync(userId);

            if (user == null)
            {
                return BadRequest();
            }

            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("project")]
        public async Task<IActionResult> UpdateProjectRights(long userId, long projectId, bool giveRights)
        {
            var user = await dbContext.Users.FindAsync(userId);
            var project = await dbContext.Projects.FindAsync(projectId);

            if (user == null || project == null)
            {
                return BadRequest();
            }

            if (giveRights)
            {
                user.UserProjects.Add(new AppUserProjects
                {
                    Project = project,
                    ProjectId = project.Id,
                    User = user,
                    UserId = user.Id
                });
            }
            else
            {
                var rights = user.UserProjects.Where(x => x.UserId.Equals(userId) && x.ProjectId.Equals(projectId)).Single();
                user.UserProjects.Remove(rights);
            }

            await dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}