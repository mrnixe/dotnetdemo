using System;

namespace TodoListSite.Models
{
    public class TodoItem
    {
        public Guid Id { get; set; }

        public bool IsDone { get; set; }

        public string UserID {get; set;}
        public string Title { get; set; }

        public DateTimeOffset? DueAt { get; set; }
    }
}