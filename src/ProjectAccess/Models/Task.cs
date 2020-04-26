namespace ProjectAccess.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public enum TaskStatus
    {
        Todo,
        InProgress,
        Done
    }

    public class Task
    {
        /// <summary>
        /// The task primary key.
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// The task title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The task description.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The task status.
        /// </summary>
        public TaskStatus Status { get; set; }

        /// <summary>
        /// The task creation date.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// The task owner.
        /// </summary>
        public long AuthorId { get; set; }

        /// <summary>
        /// The project foreign key.
        /// </summary>
        public virtual Project Project { get; set; }
    }
}
