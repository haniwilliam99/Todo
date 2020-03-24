using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Todo.Models
{
    public class TaskViewModel
    {
        public int Id { get; set; }

        public DateTime DueDate { get; set; }
        public string Title { get; set; }
    }
}