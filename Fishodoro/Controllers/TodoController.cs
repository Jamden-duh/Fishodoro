using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;

namespace Fishodoro.Controllers
{
    /// <summary>
    /// Controller responsible for managing To-Do list operations
    /// </summary>
    [Route("Todo")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        // File path for persisting tasks
        private static string filePath = "tasks.json";

        // In-memory collection of tasks, initialized from file
        private static List<TodoItem> tasks = LoadTasks();

        /// <summary>
        /// Model class representing a To-Do item with a name and description
        /// </summary>
        public class TodoItem
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }

        /// <summary>
        /// Retrieves all tasks from the system
        /// </summary>
        /// <returns>JSON result containing all tasks</returns>
        [HttpGet("GetTasks")]
        public IActionResult GetTasks() => Ok(tasks);

        /// <summary>
        /// Adds a new task to the system
        /// </summary>
        /// <param name="task">The task to be added</param>
        /// <returns>Confirmation message</returns>
        [HttpPost("AddTask")]
        public IActionResult AddTask([FromBody] TodoItem task)
        {
            tasks.Add(task);
            SaveTasks();
            return Ok("Task added.");
        }

        /// <summary>
        /// Deletes a task from the system
        /// </summary>
        /// <param name="task">The task to be deleted</param>
        /// <returns>Confirmation message</returns>
        [HttpPost("DeleteTask")]
        public IActionResult DeleteTask([FromBody] TodoItem task)
        {
            tasks.RemoveAll(t => t.Name == task.Name && t.Description == task.Description);
            SaveTasks();
            return Ok("Task removed.");
        }

        /// <summary>
        /// Updates the order of tasks in the system
        /// </summary>
        /// <param name="newOrder">New ordered list of tasks</param>
        /// <returns>Confirmation message</returns>
        [HttpPost("ReorderTasks")]
        public IActionResult ReorderTasks([FromBody] List<TodoItem> newOrder)
        {
            tasks = newOrder;
            SaveTasks();
            return Ok("Tasks reordered.");
        }

        /// <summary>
        /// Saves the current state of tasks to the file system
        /// </summary>
        private static void SaveTasks() =>
            System.IO.File.WriteAllText(filePath, JsonSerializer.Serialize(tasks));

        /// <summary>
        /// Loads tasks from the file system, or creates a new list if no file exists
        /// </summary>
        /// <returns>List of TodoItems</returns>
        private static List<TodoItem> LoadTasks() =>
            System.IO.File.Exists(filePath)
                ? JsonSerializer.Deserialize<List<TodoItem>>(System.IO.File.ReadAllText(filePath))!
                : new List<TodoItem>();
    }
}