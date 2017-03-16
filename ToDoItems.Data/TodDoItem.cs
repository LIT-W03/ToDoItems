using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoItems.Data
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool Completed { get; set; }
        public string CategoryName { get; set; }
        
        public bool IsOverdue
        {
            get { return DueDate < DateTime.Now; }
        }
    }
}
