using System.Data.SqlClient;
using System.Collections.Generic;
using System;

namespace ToDoItems.Data
{
    public class ToDoItemsDb
    {
        private string _connectionString;

        public ToDoItemsDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddCategory(string name)
        {
            ExecuteAgainstDb(command =>
            {
                command.CommandText = "INSERT INTO Categories (Name) VALUES (@name)";
                command.Parameters.AddWithValue("@name", name);
                command.ExecuteNonQuery();
            });
        }

        public void AddTask(ToDoItem item, int categoryId)
        {
            ExecuteAgainstDb(command =>
            {
                command.CommandText = "INSERT INTO ToDoItems (Title, Description, DueDate, Completed, CategoryId) " +
                                      "VALUES (@title, @desc, @due, @completed, @catId)";
                command.Parameters.AddWithValue("@title", item.Title);
                command.Parameters.AddWithValue("@desc", item.Description);
                command.Parameters.AddWithValue("@due", item.DueDate);
                command.Parameters.AddWithValue("@completed", item.Completed);
                command.Parameters.AddWithValue("@catId", categoryId);

                command.ExecuteNonQuery();
            });
        }

        public IEnumerable<ToDoItem> GetOverdue()
        {
            return QueryToDoItems("DueDate < GETDATE()");
        }

        public IEnumerable<ToDoItem> GetUnresolved()
        {
            return QueryToDoItems("Completed = 0");
        }

        public IEnumerable<ToDoItem> GetItemsForCategory(int categoryId)
        {
            return ExecuteAgainstDb(command =>
            {
                command.CommandText = "SELECT t.*, c.Name as CategoryName " +
                                      "FROM ToDoItems T JOIN Categories C " +
                                      "ON c.Id = T.categoryId " +
                                      $"WHERE c.Id = @categoryId";
                command.Parameters.AddWithValue("@categoryId", categoryId);
                SqlDataReader reader = command.ExecuteReader();
                return GetFromReader(reader);
            });
        }

        private IEnumerable<ToDoItem> QueryToDoItems(string whereClause)
        {
            return ExecuteAgainstDb(command =>
            {
                command.CommandText = "SELECT t.*, c.Name as CategoryName " +
                                      "FROM ToDoItems T JOIN Categories C " +
                                      "ON c.Id = T.categoryId " +
                                      $"WHERE {whereClause}";
                SqlDataReader reader = command.ExecuteReader();
                return GetFromReader(reader);
            });
        }

        private IEnumerable<ToDoItem> GetFromReader(SqlDataReader reader)
        {
            List<ToDoItem> items = new List<ToDoItem>();
            while (reader.Read())
            {
                ToDoItem item = new ToDoItem();
                item.Id = (int)reader["Id"];
                item.Title = (string)reader["Title"]; ;
                item.Description = (string)reader["Description"];
                item.DueDate = (DateTime)reader["DueDate"];
                item.Completed = (bool)reader["Completed"];
                item.CategoryName = (string)reader["CategoryName"];
                items.Add(item);
            }


            return items;
        }

        public IEnumerable<Category> GetCategories()
        {
            return ExecuteAgainstDb(command =>
             {
                 command.CommandText = "SELECT * FROM Categories";
                 SqlDataReader reader = command.ExecuteReader();
                 List<Category> categories = new List<Category>();
                 while (reader.Read())
                 {
                     Category c = new Category();
                     c.Id = (int)reader["Id"];
                     c.Name = (string)reader["Name"];
                     categories.Add(c);
                 }

                 return categories;
             });
        }



        private void ExecuteAgainstDb(Action<SqlCommand> action)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                action(command);
            }
        }

        private TResult ExecuteAgainstDb<TResult>(Func<SqlCommand, TResult> func)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                connection.Open();
                return func(command);
            }
        }
    }
}