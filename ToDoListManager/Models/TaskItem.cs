using System;
using System.Collections.Generic;
using System.Text;
using ToDoListManager.Models;

namespace ToDoListManager.Models
{
    // TaskItem class represents a single task in the ToDoListManager application.
    public class TaskItem
    {
        // Constructor to initialize a TaskItem with its properties
        public TaskItem(int id, string title, DateTime dueDate, bool isCompleted, string project)
        {
            Id = id;
            Title = title;
            DueDate = dueDate;
            IsCompleted = isCompleted;
            Project = project;
        }

        // Properties of the TaskItem class
        public int Id { get; set; }

        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public string Project { get; set; }
    }
}