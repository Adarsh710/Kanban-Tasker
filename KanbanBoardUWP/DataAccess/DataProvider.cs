﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KanbanBoardUWP.Model;
using Microsoft.Data.Sqlite;
using Syncfusion.UI.Xaml.Kanban;

namespace KanbanBoardUWP.DataAccess
{
    public static class DataProvider
    {
        public static void InitializeDatabase()
        {
            using (SqliteConnection db =
                new SqliteConnection("Filename=sqliteNewTest.db"))
            {
                db.Open();

                string tableCommand = "CREATE TABLE IF NOT " +
                    "EXISTS MyTable (" +
                    "Id INTEGER PRIMARY KEY, " +
                    "Title NVARCHAR(2048) NULL, " +
                    "Description NVARCHAR(2048) NULL, " +
                    "Category NVARCHAR(2048) NULL, " +
                    "ColorKey NVARCHAR(2048) NULL, " +
                    "Tags NVARCHAR(2048) NULL)";

                SqliteCommand createTable = new SqliteCommand(tableCommand, db);
                createTable.ExecuteReader();
                db.Close();
            }
        }

        public static void AddTask(int id, string title, string desc, string categ, string colorKey, string tags)
        {
            using (SqliteConnection db =
                new SqliteConnection("Filename=sqliteNewTest.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand
                {
                    Connection = db,

                    // Use parameterized query to prevent SQL injection attacks
                    CommandText = "INSERT INTO MyTable VALUES (@id, @title, @desc, @categ, @colorKey, @tags);"
                };
                insertCommand.Parameters.AddWithValue("@id", id);
                insertCommand.Parameters.AddWithValue("@title", title);
                insertCommand.Parameters.AddWithValue("@desc", desc);
                insertCommand.Parameters.AddWithValue("@categ", categ);
                insertCommand.Parameters.AddWithValue("@colorKey", colorKey);
                insertCommand.Parameters.AddWithValue("@tags", tags);
                insertCommand.ExecuteReader();

                db.Close();
            }
        }

        public static void DeleteTask(string id)
        {
            // Delete task from db
            using (SqliteConnection db =
                new SqliteConnection("Filename=sqliteNewTest.db"))
            {
                db.Open();
                SqliteCommand deleteCommand = new SqliteCommand
                    ("DELETE FROM MyTable WHERE Id=@id", db);
                deleteCommand.Parameters.AddWithValue("id", id);
                deleteCommand.ExecuteNonQuery();

                db.Close();
            }
        }

        //=====================================================================
        // FUNCTIONS & EVENTS FOR EDITING A TASK
        //=====================================================================
        public static ObservableCollection<CustomKanbanModel> GetData()
        {
            ObservableCollection<CustomKanbanModel> tasks = new ObservableCollection<CustomKanbanModel>();

            // Get tasks and return the collection
            using (SqliteConnection db =
                new SqliteConnection("Filename=sqliteNewTest.db"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT Id, Title, Description, Category, ColorKey, Tags from MyTable", db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                // Query the db and get the tasks
                while (query.Read())
                {
                    string[] tags;
                    if(query.GetString(5).ToString() == "")
                        tags = new string[] { }; // Empty array if no tags are in the col
                    else
                        tags = query.GetString(5).Split(","); // Turn string of tags into string array, fills listview

                    CustomKanbanModel row = new CustomKanbanModel()
                    {
                        ID = query.GetString(0),
                        Title = query.GetString(1),
                        Description = query.GetString(2),
                        Category = query.GetString(3),
                        ColorKey = query.GetString(4),
                        Tags = tags // Turn string of tags into string array, fills listview
                    };
  
                    tasks.Add(row);
                }
                db.Close();
            }
            return tasks;
        }

        public static void UpdateTask(string id, string title, string descr, string category, string colorKey, string tags)
        {
            using (SqliteConnection db =
                new SqliteConnection("Filename=sqliteNewTest.db"))
            {
                db.Open();

                // Update item
                SqliteCommand updateCommand = new SqliteCommand
                    ("UPDATE MyTable SET Title=@title, Description=@desc, Category=@categ, ColorKey=@colorKey, Tags=@tags WHERE Id=@id", db);
                updateCommand.Parameters.AddWithValue("@title", title);
                updateCommand.Parameters.AddWithValue("@desc", descr);
                updateCommand.Parameters.AddWithValue("@categ", category);
                updateCommand.Parameters.AddWithValue("@colorKey", colorKey);
                updateCommand.Parameters.AddWithValue("@tags", tags);
                updateCommand.Parameters.AddWithValue("@id", id);
                updateCommand.ExecuteNonQuery();

                db.Close();
            }
        }

    }
}
