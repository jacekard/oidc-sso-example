namespace ProjectAccess.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    using ProjectAccess.Areas.Identity.Data;
    using ProjectAccess.Models;

    public class ProjectController : Controller
    {
        private readonly AppDbContext dbContext;

        private readonly UserManager<AppUser> userManager;


        public ProjectController(AppDbContext context, UserManager<AppUser> userManager)
        {
            this.dbContext = context;
            this.userManager = userManager;
        }

        [HttpGet("Board/{id?}")]
        public async Task<IActionResult> Board(long? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }

            try
            {
                var project = await dbContext.Projects.FindAsync(id);
                var tasks = project.Tasks;
                ViewBag.CurrentProjectName = project.ProjectName;
                ViewBag.ProjectId = project.Id;

                return View(tasks);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Index()
        {
            var projects = new List<Project>();
            if (User.Identity.IsAuthenticated && User.IsInRole(RoleNames.Admin))
            {
                projects = await dbContext.Projects.ToListAsync();
            }
            else if (User.Identity.IsAuthenticated)
            {
                var user = await userManager.GetUserAsync(HttpContext.User);
                projects = user.UserProjects.Select(x => x.Project).ToList();
            }

            return View(projects);
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (!User.Identity.IsAuthenticated || id == null)
            {
                return NotFound();
            }

            var project = await dbContext.Projects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProjectName,Description,CreationDate")] Project project)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(HttpContext.User);
                user.UserProjects.Add(new AppUserProjects()
                {
                    Project = project,
                    ProjectId = project.Id,
                    User = user,
                    UserId = user.Id
                });
                dbContext.Add(project);
                await dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await dbContext.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,ProjectName,Description,CreationDate,EndDate")] Project project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    dbContext.Update(project);
                    await dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await dbContext.Projects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var project = await dbContext.Projects.FindAsync(id);
            dbContext.Projects.Remove(project);
            await dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(long id)
        {
            return dbContext.Projects.Any(e => e.Id == id);
        }
    }
}
