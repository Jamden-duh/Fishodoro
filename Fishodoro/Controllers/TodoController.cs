using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Fishodoro.Controllers
{
    [Route("Todo")] // API route prefix
    [ApiController] // Designates this class as an API controller
    public class TodoController : ControllerBase
    {
        private static string filePath = "tasks.json"; // Path to store tasks data
        private static List<string> tasks = LoadTasks(); // Load tasks from file at startup

        // Endpoint to retrieve all tasks
        [HttpGet("GetTasks")]
        public IActionResult GetTasks() => Ok(tasks); // Return tasks as JSON

        // Endpoint to add a new task
        [HttpPost("AddTask")]
        public IActionResult AddTask([FromBody] string task)
        {
            tasks.Add(task); // Add task to the list
            SaveTasks(); // Save updated task list to file
            return Ok("Task added."); // Respond with success message
        }

        // Endpoint to delete a task
        [HttpPost("DeleteTask")]
        public IActionResult DeleteTask([FromBody] string task)
        {
            tasks.Remove(task); // Remove the specified task from the list
            SaveTasks(); // Save updated task list to file
            return Ok("Task removed."); // Respond with success message
        }

        // Endpoint to reorder tasks
        [HttpPost("ReorderTasks")]
        public IActionResult ReorderTasks([FromBody] List<string> newOrder)
        {
            tasks = newOrder; // Replace current task list with new order
            SaveTasks(); // Save updated task list to file
            return Ok("Tasks reordered."); // Respond with success message
        }

        // Helper method to save tasks to file
        private static void SaveTasks() => System.IO.File.WriteAllText(filePath, JsonSerializer.Serialize(tasks));

        // Helper method to load tasks from file (if file exists)
        private static List<string> LoadTasks() =>
            System.IO.File.Exists(filePath) ? JsonSerializer.Deserialize<List<string>>(System.IO.File.ReadAllText(filePath))! : new List<string>();
    }

}
