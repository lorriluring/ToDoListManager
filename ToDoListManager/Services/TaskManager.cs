using System;
using System.Collections.Generic;
using System.Text;
using ToDoListManager.Models;
using System.Text.Json;
using System.Linq;

namespace ToDoListManager.Services
{
    // TaskManager class is responsible for managing the list of tasks in the ToDoListManager application.
    public class TaskManager
    {
        // List to store the tasks
        private List<TaskItem> tasks = new();

        // Method to write error message in red color to the console instead of writing it for each error message separately.
        private static void WriteError(string message)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = originalColor;
        }

        // Method to write success message in green color to the console instead of writing it for each success message separately.
        private static void WriteSuccess(string message)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ForegroundColor = originalColor;
        }

        //  Method to add a new task to the list with input validation for title, due date, and project name.
        public void AddTask()
        {
            // Input validation for task title, due date, and project name
            string title;
            do
            {
                Console.WriteLine("Enter task title:");
                title = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(title))
                {
                    WriteError("Task title cannot be empty.");
                    WriteError("Example: Finish UML Diagram");
                }
            } while (string.IsNullOrWhiteSpace(title));
            // Input validation for due date
            DateTime dueDate;
            while (true)
            {
                Console.WriteLine("Enter due date (yyyy-MM-dd):");
                string input = Console.ReadLine();

                if (DateTime.TryParseExact(
                    input,
                    "yyyy-MM-dd",
                    null,
                    System.Globalization.DateTimeStyles.None,
                    out dueDate))
                {
                    if (dueDate >= DateTime.Today)
                        break;

                    WriteError("Due date cannot be in the past.");
                }
                else
                {
                    WriteError("Invalid date format.");
                    WriteError("Example: 2026-11-15");
                }
            }
            // Input validation for project name

            string project;
            do
            {
                Console.WriteLine("Enter project name:");
                project = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(project))
                {
                    WriteError("Project name cannot be empty.");
                    WriteError("Example: School");
                }
            } while (string.IsNullOrWhiteSpace(project));
            // Generate a new unique ID for the task

            int newId = tasks.Count == 0
                ? 1
                : tasks.Max(t => t.Id) + 1;
            // Create a new TaskItem object and add it to the list
            TaskItem newTask = new TaskItem(
                newId,
                title,
                dueDate,
                false,
                project);

            tasks.Add(newTask);

            WriteSuccess("Task added successfully!");
        }

        // Method to display all tasks in a formatted table with headers and status indicators.
        public void ShowTasks()
        {
            if (tasks.Count == 0)
            {
                WriteError("No tasks found.");
                return;
            }
            // Display tasks in a formatted table

            DisplayTasks(tasks);
        }

        // Method to update an existing task with input validation for task ID, title, due date, and project name.
        public void UpdateTask()
        {
            if (tasks.Count == 0)
            {
                WriteError("No tasks found.");
                return;
            }
            // Display tasks to help the user select a task to modify
            ShowTasks();

            TaskItem selectedTask = null;
            // Input validation for task ID and selection of the task to modify
            while (selectedTask == null)
            {
                Console.Write("Enter the ID of the task you want to modify: ");
                string input = Console.ReadLine();

                if (!int.TryParse(input, out int taskId))
                {
                    WriteError("Invalid task ID.");
                    WriteError("Please enter a numeric value.");
                    WriteError("Example: 1");
                    continue;
                }

                selectedTask = tasks.FirstOrDefault(t => t.Id == taskId);

                if (selectedTask == null)
                {
                    WriteError("Task not found.");
                    WriteError("Please enter an existing task ID.");
                }
            }
            // Menu for modifying the selected task with options to edit title, due date, project name, mark as complete, or return to the main menu.

            bool modifying = true;

            while (modifying)
            {
                Console.WriteLine();
                Console.WriteLine($"Selected Task: {selectedTask.Title}");
                Console.WriteLine("1. Edit Title");
                Console.WriteLine("2. Edit Due Date");
                Console.WriteLine("3. Edit Project");
                Console.WriteLine("4. Mark as Complete");
                Console.WriteLine("5. Return");

                Console.Write("Choose an option: ");
                string choice = Console.ReadLine();
                // Input validation and handling of the user's choice for modifying the selected task
                switch (choice)
                {
                    case "1":

                        string newTitle;

                        do
                        {
                            Console.Write("Enter new title: ");
                            newTitle = Console.ReadLine();

                            if (string.IsNullOrWhiteSpace(newTitle))
                            {
                                WriteError("Title cannot be empty.");
                                WriteError("Example: Finish UML Diagram");
                            }
                        } while (string.IsNullOrWhiteSpace(newTitle));

                        selectedTask.Title = newTitle;
                        WriteSuccess("Title updated successfully.");
                        break;
                    // Input validation for due date with format checking and past date restriction
                    case "2":

                        DateTime newDueDate;

                        while (true)
                        {
                            Console.Write("Enter new due date (yyyy-MM-dd): ");
                            string dateInput = Console.ReadLine();

                            if (DateTime.TryParseExact(
                                dateInput,
                                "yyyy-MM-dd",
                                null,
                                System.Globalization.DateTimeStyles.None,
                                out newDueDate))
                            {
                                if (newDueDate >= DateTime.Today)
                                    break;

                                WriteError("Due date cannot be in the past.");
                            }
                            else
                            {
                                WriteError("Invalid date format.");
                                WriteError("Example: 2026-11-15");
                            }
                        }

                        selectedTask.DueDate = newDueDate;
                        WriteSuccess("Due date updated successfully.");
                        break;
                    // Input validation for project name with non-empty restriction
                    case "3":

                        string newProject;

                        do
                        {
                            Console.Write("Enter new project name: ");
                            newProject = Console.ReadLine();

                            if (string.IsNullOrWhiteSpace(newProject))
                            {
                                WriteError("Project name cannot be empty.");
                                WriteError("Example: School");
                            }
                        } while (string.IsNullOrWhiteSpace(newProject));

                        selectedTask.Project = newProject;
                        WriteSuccess("Project updated successfully.");
                        break;
                    // Check if the task is already completed before marking it as complete
                    case "4":

                        if (selectedTask.IsCompleted)
                        {
                            WriteError("This task is already completed.");
                        }
                        else
                        {
                            selectedTask.IsCompleted = true;
                            WriteSuccess("Task marked as complete.");
                        }

                        break;

                    // Exit the modification menu and return to the main menu
                    case "5":

                        modifying = false;
                        break;

                    default:

                        WriteError("Invalid option.");
                        WriteError("Please choose a number between 1 and 5.");
                        break;
                }
            }
        }

        // Method to mark a task as complete with input validation for task ID and status check.
        public void CompleteTask()
        {
            if (tasks.Count == 0)
            {
                WriteError("No tasks found.");
                return;
            }
            // Display tasks to help the user select a task to mark as complete
            ShowTasks();

            TaskItem selectedTask = null;

            while (selectedTask == null)
            {
                Console.Write("Enter the ID of the task to mark as complete: ");

                string input = Console.ReadLine();
                // Input validation for task ID and selection of the task to mark as complete
                if (!int.TryParse(input, out int taskId))
                {
                    WriteError("Invalid task ID.");
                    WriteError("Please enter a numeric value.");
                    WriteError("Example: 1");
                    continue;
                }
                // Find the task with the specified ID in the list of tasks
                selectedTask = tasks.FirstOrDefault(t => t.Id == taskId);

                if (selectedTask == null)
                {
                    WriteError("Task not found.");
                    WriteError("Please select an existing task.");
                }
            }
            // Check if the selected task is already marked as complete before updating its status
            if (selectedTask.IsCompleted)
            {
                WriteError("This task is already marked as complete.");
                return;
            }
            // Mark the selected task as complete and provide feedback to the user
            selectedTask.IsCompleted = true;

            WriteSuccess("Task marked as complete.");
        }

        // Method to delete a task from the list with confirmation prompt and input validation for task ID.
        public void DeleteTask()
        {
            if (tasks.Count == 0)
            {
                WriteError("No tasks found.");
                return;
            }
            // Display tasks to help the user select a task to delete
            ShowTasks();

            Console.Write("Enter the ID of the task you want to delete: ");

            if (!int.TryParse(Console.ReadLine(), out int taskId))
            {
                WriteError("Invalid input. Please enter a valid task ID.");
                return;
            }
            // Find the task with the specified ID in the list of tasks
            TaskItem taskToDelete = tasks.FirstOrDefault(t => t.Id == taskId);

            if (taskToDelete == null)
            {
                WriteError("Task not found.");
                return;
            }
            // Confirmation prompt before deleting the task
            Console.WriteLine($"Are you sure you want to delete \"{taskToDelete.Title}\"?");
            Console.WriteLine("1. Yes");
            Console.WriteLine("2. No");

            string choice = Console.ReadLine();
            // Handle the user's choice for deleting the task with appropriate feedback
            if (choice == "1")
            {
                tasks.Remove(taskToDelete);
                WriteSuccess("Task deleted successfully.");
            }
            else
            {
                Console.WriteLine("Delete operation cancelled.");
            }
        }

        // Method to search for tasks based on title, project name, or due date with input validation for search term.
        public void SearchTasks()
        {
            if (tasks.Count == 0)
            {
                WriteError("No tasks found.");
                return;
            }
            // Prompt the user to enter a search term and validate the input
            Console.Write("Enter search term: ");
            string searchTerm = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                WriteError("Search term cannot be empty.");
                return;
            }
            // Perform the search by filtering tasks based on title, project name, or due date
            var results = tasks.Where(t =>
                t.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                t.Project.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                t.DueDate.ToString("yyyy-MM-dd").Contains(searchTerm))
                .ToList();
            // Display the search results or inform the user if no tasks matched the search term
            if (results.Count == 0)
            {
                WriteError($"No tasks matched \"{searchTerm}\".");
                return;
            }

            DisplayTasks(results);
        }

        // Method to sort tasks by due date and display them in a formatted table.
        public void SortByDate()
        {
            if (tasks.Count == 0)
            {
                WriteError("No tasks found.");
                return;
            }

            DisplayTasks(tasks.OrderBy(t => t.DueDate));
        }

        // Method to sort tasks by project name and display them in a formatted table.
        public void SortByProject()
        {
            if (tasks.Count == 0)
            {
                WriteError("No tasks found.");
                return;
            }

            DisplayTasks(tasks.OrderBy(t => t.Project));
        }

        // Method to save the list of tasks to a JSON file with pretty-print formatting.
        public void SaveToFile()
        {
            string json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText("tasks.json", json);

            WriteSuccess("Tasks saved successfully.");
        }

        // Method to load the list of tasks from a JSON file with error handling for file existence and deserialization.
        public void LoadFromFile()
        {
            if (!File.Exists("tasks.json"))
            {
                WriteError("No existing task list found.");
                WriteError("Creating a new task list...");
                return;
            }

            string json = File.ReadAllText("tasks.json");

            tasks = JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();

            WriteSuccess("Tasks loaded successfully.");
        }

        // Private method to display a list of tasks in a formatted table with headers and status indicators.
        private void DisplayTasks(IEnumerable<TaskItem> tasksToDisplay)
        {
            const string rowFormat = "| {0,-4} | {1,-30} | {2,-10} | {3,-20} | {4,-6} |";

            Console.WriteLine();
            Console.WriteLine(string.Format(rowFormat, "ID", "Title", "Due Date", "Project", "Status"));
            Console.WriteLine(new string('-', 84));

            foreach (var task in tasksToDisplay)
            {
                // Determine the status of the task and truncate title and project name if they exceed specified lengths
                string status = task.IsCompleted
                    ? "\u001b[32mDone\u001b[0m" // Green text for completed tasks
                    : "\u001b[34mOpen\u001b[0m"; // Blue text for open tasks
                string title = task.Title.Length > 30 ? task.Title[..27] + "..." : task.Title;
                string project = task.Project.Length > 20 ? task.Project[..17] + "..." : task.Project;

                Console.WriteLine(string.Format(rowFormat, task.Id, title, task.DueDate.ToString("yyyy-MM-dd"), project, status));
            }
        }

        // Method to display a menu for viewing tasks with options to show all tasks, sort by due date, sort by project, search tasks, or return to the main menu.
        public void ViewTasksMenu()
        {
            bool viewing = true;

            while (viewing)
            {
                Console.WriteLine();
                Console.WriteLine("=== VIEW TASKS ===");
                Console.WriteLine("1. Show All Tasks");
                Console.WriteLine("2. Sort By Due Date");
                Console.WriteLine("3. Sort By Project");
                Console.WriteLine("4. Search Tasks");
                Console.WriteLine("5. Return");

                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                // Input validation and handling of the user's choice for viewing tasks
                switch (choice)
                {
                    case "1":
                        ShowTasks();
                        break;

                    case "2":
                        SortByDate();
                        break;

                    case "3":
                        SortByProject();
                        break;

                    case "4":
                        SearchTasks();
                        break;

                    case "5":
                        viewing = false;
                        break;

                    default:
                        WriteError("Invalid option.");
                        WriteError("Please enter a number between 1 and 5.");
                        break;
                }
            }
        }
    }
}