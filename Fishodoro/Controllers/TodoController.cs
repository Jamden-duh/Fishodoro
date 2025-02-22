using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;

namespace Fishodoro.Controllers
{
    [Route("Todo")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private static string filePath = "tasks.json";
        private static List<TodoItem> tasks = LoadTasks();

        public class TodoItem
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }

        [HttpGet("GetTasks")]
        public IActionResult GetTasks() => Ok(tasks);

        [HttpPost("AddTask")]
        public IActionResult AddTask([FromBody] TodoItem task)
        {
            tasks.Add(task);
            SaveTasks();
            return Ok("Task added.");
        }

        [HttpPost("DeleteTask")]
        public IActionResult DeleteTask([FromBody] TodoItem task)
        {
            tasks.RemoveAll(t => t.Name == task.Name && t.Description == task.Description);
            SaveTasks();
            return Ok("Task removed.");
        }

        [HttpPost("ReorderTasks")]
        public IActionResult ReorderTasks([FromBody] List<TodoItem> newOrder)
        {
            tasks = newOrder;
            SaveTasks();
            return Ok("Tasks reordered.");
        }

        private static void SaveTasks() =>
            System.IO.File.WriteAllText(filePath, JsonSerializer.Serialize(tasks));

        private static List<TodoItem> LoadTasks() =>
            System.IO.File.Exists(filePath)
                ? JsonSerializer.Deserialize<List<TodoItem>>(System.IO.File.ReadAllText(filePath))!
                : new List<TodoItem>();
    }
}
