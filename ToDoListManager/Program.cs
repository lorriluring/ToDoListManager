using System;
using ToDoListManager.Models;
using ToDoListManager.Services;

namespace ToDoListManager
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            TaskManager taskManager = new TaskManager();
            taskManager.LoadFromFile();

            bool running = true;

            while (running)
            {
                Console.WriteLine();
                Console.WriteLine("=== TODO LIST MANAGER ===");
                Console.WriteLine("1. View Tasks");
                Console.WriteLine("2. Add New Task");
                Console.WriteLine("3. Modify Task");
                Console.WriteLine("4. Remove Task");
                Console.WriteLine("5. Save & Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

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

                    default:
                        Console.WriteLine("Invalid option.");
                        Console.WriteLine("Please enter a number between 1 and 5.");
                        Console.WriteLine("Example: 2");
                        break;
                }
            }
        }
    }
}