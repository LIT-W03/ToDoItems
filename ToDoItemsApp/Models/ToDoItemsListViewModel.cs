using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToDoItems.Data;

namespace ToDoItemsApp.Models
{
    public class ToDoItemsListViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<ToDoItem> ToDoItems { get; set; }
        public int? SelectedCategory { get; set; }
    }
}