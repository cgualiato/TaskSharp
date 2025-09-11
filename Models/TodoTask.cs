using System;
using System.Collections.Generic;

namespace TaskSharp.Models
{
    public class TodoTask
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }
        public bool Completed { get; set; } = false;
        public List<string> Tags { get; set; } = new List<string>();
    }
}
