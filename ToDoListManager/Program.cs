using System;
using ToDoListManager.Models;
using ToDoListManager.Services;

namespace ToDoListManager
{
    internal class Program
    {
        // Main entry point of the application
        private static void Main(string[] args)
        {
            // Create an instance of TaskManager and load tasks from file
            TaskManager taskManager = new TaskManager();
            taskManager.LoadFromFile();

            bool running = true;
            // Main loop for the application
            while (running)
            {
                // Display the main menu to the user
                Console.WriteLine();
                // Set the console text color to dark yellow for the title
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("=== TODO LIST MANAGER ===");
                Console.ResetColor();
                Console.WriteLine("1. View Tasks");
                Console.WriteLine("2. Add New Task");
                Console.WriteLine("3. Modify Task");
                Console.WriteLine("4. Remove Task");
                Console.WriteLine("5. Save & Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();
                // Handle user input and call appropriate methods in TaskManager
                switch (choice)
                {
                    case "1":
                        taskManager.ViewTasksMenu();
                        break;

                    case "2":
                        taskManager.AddTask();
                        break;

                    case "3":
                        taskManager.UpdateTask();
                        break;

                    case "4":
                        taskManager.DeleteTask();
                        break;

                    case "5":
                        taskManager.SaveToFile();
                        running = false;
                        Console.WriteLine("See you next time!");
                        break;
                    // Handle invalid input
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid option.");
                        Console.ResetColor();
                        Console.WriteLine("Please enter a number between 1 and 5.");
                        break;
                }
            }
        }
    }
}