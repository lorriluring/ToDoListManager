using System;
using System.Collections.Generic;
using System.Text;
using ToDoListManager.Models;
using System.Text.Json;

namespace ToDoListManager.Services
{
    // TaskManager class is responsible for managing the list of tasks in the ToDoListManager application.
    public class TaskManager
    {
        // List to store the tasks
        private List<TaskItem> tasks = new();

        //  Method to add a new task to the list with input validation for title, due date, and project name.
        public void AddTask()
        {
            string title;
            do
            {
                Console.WriteLine("Enter task title:");
                title = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(title))
                {
                    Console.WriteLine("Task title cannot be empty.");
                    Console.WriteLine("Example: Finish UML Diagram");
                }
            } while (string.IsNullOrWhiteSpace(title));

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

                    Console.WriteLine("Due date cannot be in the past.");
                }
                else
                {
                    Console.WriteLine("Invalid date format.");
                    Console.WriteLine("Example: 2026-11-15");
                }
            }

            string project;
            do
            {
                Console.WriteLine("Enter project name:");
                project = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(project))
                {
                    Console.WriteLine("Project name cannot be empty.");
                    Console.WriteLine("Example: School");
                }
            } while (string.IsNullOrWhiteSpace(project));

            int newId = tasks.Count == 0
                ? 1
                : tasks.Max(t => t.Id) + 1;

            TaskItem newTask = new TaskItem(
                newId,
                title,
                dueDate,
                false,
                project);

            tasks.Add(newTask);

            Console.WriteLine("Task added successfully!");
        }

        // Method to display all tasks in a formatted table with headers and status indicators.
        public void ShowTasks()
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks found.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("ID | Title | Due Date | Project | Status");
            Console.WriteLine("---------------------------------------------------------");

            foreach (TaskItem task in tasks)
            {
                string status = task.IsCompleted ? "Done" : "Open";

                Console.WriteLine(
                    $"{task.Id} | {task.Title} | {task.DueDate:yyyy-MM-dd} | {task.Project} | {status}");
            }
        }

        // Method to update an existing task with input validation for task ID, title, due date, and project name.
        public void UpdateTask()
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks found.");
                return;
            }

            ShowTasks();

            TaskItem selectedTask = null;

            while (selectedTask == null)
            {
                Console.Write("Enter the ID of the task you want to modify: ");
                string input = Console.ReadLine();

                if (!int.TryParse(input, out int taskId))
                {
                    Console.WriteLine("Invalid task ID.");
                    Console.WriteLine("Please enter a numeric value.");
                    Console.WriteLine("Example: 1");
                    continue;
                }

                selectedTask = tasks.FirstOrDefault(t => t.Id == taskId);

                if (selectedTask == null)
                {
                    Console.WriteLine("Task not found.");
                    Console.WriteLine("Please enter an existing task ID.");
                }
            }

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
                                Console.WriteLine("Title cannot be empty.");
                                Console.WriteLine("Example: Finish UML Diagram");
                            }
                        } while (string.IsNullOrWhiteSpace(newTitle));

                        selectedTask.Title = newTitle;
                        Console.WriteLine("Title updated successfully.");
                        break;

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

                                Console.WriteLine("Due date cannot be in the past.");
                            }
                            else
                            {
                                Console.WriteLine("Invalid date format.");
                                Console.WriteLine("Example: 2026-11-15");
                            }
                        }

                        selectedTask.DueDate = newDueDate;
                        Console.WriteLine("Due date updated successfully.");
                        break;

                    case "3":

                        string newProject;

                        do
                        {
                            Console.Write("Enter new project name: ");
                            newProject = Console.ReadLine();

                            if (string.IsNullOrWhiteSpace(newProject))
                            {
                                Console.WriteLine("Project name cannot be empty.");
                                Console.WriteLine("Example: School");
                            }
                        } while (string.IsNullOrWhiteSpace(newProject));

                        selectedTask.Project = newProject;
                        Console.WriteLine("Project updated successfully.");
                        break;

                    case "4":

                        if (selectedTask.IsCompleted)
                        {
                            Console.WriteLine("This task is already completed.");
                        }
                        else
                        {
                            selectedTask.IsCompleted = true;
                            Console.WriteLine("Task marked as complete.");
                        }

                        break;

                    case "5":

                        modifying = false;
                        break;

                    default:

                        Console.WriteLine("Invalid option.");
                        Console.WriteLine("Please choose a number between 1 and 5.");
                        break;
                }
            }
        }

        // Method to mark a task as complete with input validation for task ID and status check.
        public void CompleteTask()
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks found.");
                return;
            }

            ShowTasks();

            TaskItem selectedTask = null;

            while (selectedTask == null)
            {
                Console.Write("Enter the ID of the task to mark as complete: ");

                string input = Console.ReadLine();

                if (!int.TryParse(input, out int taskId))
                {
                    Console.WriteLine("Invalid task ID.");
                    Console.WriteLine("Please enter a numeric value.");
                    Console.WriteLine("Example: 1");
                    continue;
                }

                selectedTask = tasks.FirstOrDefault(t => t.Id == taskId);

                if (selectedTask == null)
                {
                    Console.WriteLine("Task not found.");
                    Console.WriteLine("Please select an existing task.");
                }
            }

            if (selectedTask.IsCompleted)
            {
                Console.WriteLine("This task is already marked as complete.");
                return;
            }

            selectedTask.IsCompleted = true;

            Console.WriteLine("Task marked as complete.");
        }

        // Method to delete a task from the list with confirmation prompt and input validation for task ID.
        public void DeleteTask()
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks found.");
                return;
            }

            ShowTasks();

            Console.Write("Enter the ID of the task you want to delete: ");

            if (!int.TryParse(Console.ReadLine(), out int taskId))
            {
                Console.WriteLine("Invalid input. Please enter a valid task ID.");
                return;
            }

            TaskItem taskToDelete = tasks.FirstOrDefault(t => t.Id == taskId);

            if (taskToDelete == null)
            {
                Console.WriteLine("Task not found.");
                return;
            }

            Console.WriteLine($"Are you sure you want to delete \"{taskToDelete.Title}\"?");
            Console.WriteLine("1. Yes");
            Console.WriteLine("2. No");

            string choice = Console.ReadLine();

            if (choice == "1")
            {
                tasks.Remove(taskToDelete);
                Console.WriteLine("Task deleted successfully.");
            }
            else
            {
                Console.WriteLine("Delete operation cancelled.");
            }
        }

        public void SearchTasks()
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks found.");
                return;
            }

            Console.Write("Enter search term: ");
            string searchTerm = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                Console.WriteLine("Search term cannot be empty.");
                return;
            }

            var results = tasks.Where(t =>
                t.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                t.Project.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                t.DueDate.ToString("yyyy-MM-dd").Contains(searchTerm))
                .ToList();

            if (results.Count == 0)
            {
                Console.WriteLine($"No tasks matched \"{searchTerm}\".");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Search Results:");
            Console.WriteLine("ID | Title | Due Date | Project | Status");
            Console.WriteLine("---------------------------------------------------------");

            foreach (var task in results)
            {
                string status = task.IsCompleted ? "Done" : "Open";

                Console.WriteLine(
                    $"{task.Id} | {task.Title} | {task.DueDate:yyyy-MM-dd} | {task.Project} | {status}");
            }
        }

        public void SortByDate()
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks found.");
                return;
            }

            var sortedTasks = tasks.OrderBy(t => t.DueDate);

            Console.WriteLine();
            Console.WriteLine("ID | Title | Due Date | Project | Status");
            Console.WriteLine("---------------------------------------------------------");

            foreach (var task in sortedTasks)
            {
                string status = task.IsCompleted ? "Done" : "Open";

                Console.WriteLine(
                    $"{task.Id} | {task.Title} | {task.DueDate:yyyy-MM-dd} | {task.Project} | {status}");
            }
        }

        public void SortByProject()
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks found.");
                return;
            }

            var sortedTasks = tasks.OrderBy(t => t.Project);

            Console.WriteLine();
            Console.WriteLine("ID | Title | Due Date | Project | Status");
            Console.WriteLine("---------------------------------------------------------");

            foreach (var task in sortedTasks)
            {
                string status = task.IsCompleted ? "Done" : "Open";

                Console.WriteLine(
                    $"{task.Id} | {task.Title} | {task.DueDate:yyyy-MM-dd} | {task.Project} | {status}");
            }
        }

        public void SaveToFile()
        {
            string json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText("tasks.json", json);

            Console.WriteLine("Tasks saved successfully.");
        }

        public void LoadFromFile()
        {
            if (!File.Exists("tasks.json"))
            {
                Console.WriteLine("No existing task list found.");
                Console.WriteLine("Creating a new task list...");
                return;
            }

            string json = File.ReadAllText("tasks.json");

            tasks = JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();

            Console.WriteLine("Tasks loaded successfully.");
        }

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
                        Console.WriteLine("Invalid option.");
                        Console.WriteLine("Please enter a number between 1 and 5.");
                        break;
                }
            }
        }
    }
}