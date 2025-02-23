using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace TodoApp.Controllers
{
    /// <summary>
    /// Controller handling todo list operations and coin management.
    /// </summary>
    [ApiController]
    [Route("Todo")]
    public class TodoController : ControllerBase
    {
        // Static collections and file paths for persistent data
        private static List<TodoItem> tasks = new List<TodoItem>();
        private static string filePath = "tasks.json";          // Path for storing tasks
        private static string coinsFilePath = "coins.json";     // Path for storing coins
        private static int currentCoins = 0;                    // Current coin balance

        // Lock object for ensuring thread safety while accessing shared resources
        private static readonly object fileLock = new object();

        /// <summary>
        /// Static constructor to initialize tasks and coins from persistent storage (JSON files).
        /// </summary>
        static TodoController()
        {
            // Load tasks from JSON file if it exists
            if (System.IO.File.Exists(filePath))
            {
                string json = System.IO.File.ReadAllText(filePath);
                tasks = JsonSerializer.Deserialize<List<TodoItem>>(json) ?? new List<TodoItem>();
            }

            // Load coins from JSON file if it exists
            if (System.IO.File.Exists(coinsFilePath))
            {
                string coinsJson = System.IO.File.ReadAllText(coinsFilePath);
                currentCoins = JsonSerializer.Deserialize<int>(coinsJson);
            }
        }

        /// <summary>
        /// Endpoint to reorder tasks based on the provided new order.
        /// </summary>
        /// <param name="newOrder">New order of tasks.</param>
        /// <returns>Success message after tasks are reordered.</returns>
        [HttpPost("ReorderTasks")]
        public IActionResult ReorderTasks([FromBody] List<TodoItem> newOrder)
        {
            tasks = newOrder; // Set the new task order
            SaveTasks(); // Save the new order to file
            return Ok("Tasks reordered.");
        }

        /// <summary>
        /// Endpoint to toggle the removal of a task upon completion and reward coins.
        /// </summary>
        /// <param name="taskIndex">Index of the task to remove (mark as completed).</param>
        /// <returns>Result message with updated coin balance.</returns>
        [HttpPost("ToggleTaskRemoval")]
        public IActionResult ToggleTaskRemoval([FromBody] int taskIndex)
        {
            // Validate task index to ensure the task exists
            if (taskIndex >= 0 && taskIndex < tasks.Count)
            {
                tasks.RemoveAt(taskIndex); // Remove the completed task
                SaveTasks(); // Save the updated tasks list

                // Reward the user with 10 coins for completing a task
                currentCoins += 10;
                SaveCoins(); // Save the new coin balance

                return Ok(new { message = "Task removed.", coins = currentCoins });
            }
            return BadRequest("Invalid task index.");
        }

        /// <summary>
        /// Endpoint to get the current coin balance of the user.
        /// </summary>
        /// <returns>Current coin balance.</returns>
        [HttpGet("GetCoins")]
        public IActionResult GetCoins()
        {
            return Ok(currentCoins);
        }

        /// <summary>
        /// Endpoint to handle item purchases, deducting coins from the balance.
        /// </summary>
        /// <param name="price">Price of the item to purchase.</param>
        /// <returns>Result with remaining coins after purchase.</returns>
        [HttpPost("BuyItem")]
        public IActionResult BuyItem([FromBody] int price)
        {
            // Check if the user has enough coins to make the purchase
            if (currentCoins >= price)
            {
                currentCoins -= price; // Deduct the price from the user's coins
                SaveCoins(); // Save the updated coin balance
                return Ok(new { success = true, remainingCoins = currentCoins });
            }
            return BadRequest(new { success = false, message = "Not enough coins" });
        }

        /// <summary>
        /// Helper method to save the tasks list to a JSON file for persistence.
        /// </summary>
        private static void SaveTasks()
        {
            lock (fileLock) // Ensure thread safety for file operations
            {
                try
                {
                    string json = JsonSerializer.Serialize(tasks);
                    System.IO.File.WriteAllText(filePath, json);
                }
                catch (Exception ex)
                {
                    // Log error if saving tasks fails
                    Console.WriteLine($"Error saving tasks: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Helper method to save the coin balance to a JSON file for persistence.
        /// </summary>
        private static void SaveCoins()
        {
            lock (fileLock) // Ensure thread safety for file operations
            {
                try
                {
                    string json = JsonSerializer.Serialize(currentCoins);
                    System.IO.File.WriteAllText(coinsFilePath, json);
                }
                catch (Exception ex)
                {
                    // Log error if saving coins fails
                    Console.WriteLine($"Error saving coins: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Endpoint to add a new task to the list.
        /// </summary>
        /// <param name="newTask">New task to be added.</param>
        /// <returns>Message confirming the task has been added.</returns>
        [HttpPost("AddTask")]
        public IActionResult AddTask([FromBody] TodoItem newTask)
        {
            // Ensure the task has a valid name
            if (newTask == null || string.IsNullOrWhiteSpace(newTask.Name))
            {
                return BadRequest("Task name is required.");
            }

            tasks.Add(newTask); // Add the new task to the list
            SaveTasks(); // Save the updated tasks list
            return Ok(new { message = "Task added successfully.", tasks });
        }

        /// <summary>
        /// Endpoint to get the current list of tasks.
        /// </summary>
        /// <returns>The list of tasks.</returns>
        [HttpGet("GetTasks")]
        public IActionResult GetTasks()
        {
            return Ok(tasks); // Return the list of tasks
        }
    }

    /// <summary>
    /// Model class representing a todo item.
    /// </summary>
    public class TodoItem
    {
        public string Name { get; set; }           // Task name
        public string Description { get; set; }    // Task description
        public bool IsCompleted { get; set; }      // Completion status
    }
}
