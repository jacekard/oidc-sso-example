namespace ProjectAccess.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using ProjectAccess.Areas.Identity.Data;
    using ProjectAccess.Models;

    using Model = Models;

    [Produces("application/json")]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        private readonly UserManager<AppUser> userManager;

        public TasksController(AppDbContext dbContext, UserManager<AppUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        /// api/tasks/id?

        [HttpGet("id")]
        public async Task<ActionResult<Model.Task>> GetTask(long id = 5)
        {
            var task = await dbContext.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return task;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(long id, Model.TaskStatus status)
        {
            var task = await dbContext.Tasks.FindAsync(id);

            if (task == null)
            {
                return Ok();
            }

            task.Status = status;
            dbContext.Entry(task).State = EntityState.Modified;

            await dbContext.SaveChangesAsync();

            return Ok();
        }

        [Produces("application/json")]
        [HttpPost("{projectId}")]
        public async Task<ActionResult> PostTask([FromBody] Model.Task task, long projectId)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            task.CreationDate = DateTime.Now;
            task.AuthorId = user.Id;
            task.Project = await dbContext.Projects.FindAsync(projectId);

            dbContext.Tasks.Add(task);
            await dbContext.SaveChangesAsync();

            return Ok(task.Id);
        }

        // DELETE: api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Model.Task>> DeleteTask(long id)
        {
            var task = await dbContext.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            dbContext.Tasks.Remove(task);
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        private bool TaskExists(long id)
        {
            return dbContext.Tasks.Any(e => e.Id == id);
        }
    }
}
