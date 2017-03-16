using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToDoItems.Data;

namespace ToDoItemsApp.Models
{
    public class NewTaskViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
    }

}