
using LoginApp.Models;
using SQLite;
using System;
using System.Collections.Generic;

namespace LoginApp
{
    class DBController
    {
        public static SQLiteConnection GetConn()
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return new SQLiteConnection(System.IO.Path.Combine(folder, "login.db"));
        }

        public static void InitDB()
        {
            var db = GetConn();
            db.CreateTable<User>();

            if (!DBExists(db))
            {
                InsertSampleDataIntoDB(db);
            }
        }

        /**
         * Returns true if Database Table already exists.
         **/
        private static bool DBExists(SQLiteConnection db)
        {
            return !(db.Table<User>().Count() == 0);
        }

        /**
         * Adds sample Users to table.
         * */
        private static void InsertSampleDataIntoDB(SQLiteConnection db)
        {
            AddUser(db, new User { username = "abeman", password = "Orange1", info = "I like oranges." });
            AddUser(db, new User { username = "janisgreat", password = "Janisgreat1", info = "Jan is pretty great!" });
            AddUser(db, new User { username = "puppylover", password = "PuppyLover2" });
        }

        /**
         * Adds a User to the User table.
         * */
        public static void AddUser(SQLiteConnection db, User user)
        {
            db.Insert(user);
            Console.WriteLine("Added user: " + user.username + " at id: " + user.id);
        }

        /**
         * Returns User with passed username. If no User found, returns a blank User.
         **/
        public static User GetUser(SQLiteConnection db, string username)
        {
            string query = "SELECT * FROM User WHERE username=?";
            try
            {
                List<User> userList = db.Query<User>(query, username);
                if (userList.Count > 0)
                {
                    return userList[0];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("GetUser ERROR: " + e.ToString());
            }
            return new User();
        }

        /**
         * Returns all users.
         * */
         public static List<User> GetAllUsers()
        {
            string query = "SELECT * FROM User";
            return GetConn().Query<User>(query);
        }

        /**
         * Returns true if username and password match a record in the User table.
         * */
        public static bool AuthUserLogin(SQLiteConnection db, string username, string password)
        {
            string query = "SELECT * FROM User WHERE username=? AND password=?";
            return (1 == db.Query<User>(query, username, password).Count);
        }

        /**
         * Returns a list of all User's usernames.
         * */
        public static List<string> GetAllUsernames()
        {
            List<User> users = GetAllUsers();
            List<string> usernames = new List<string>();
            foreach (var user in users)
            {
                usernames.Add(user.username);
            }
            return usernames;
        }

        public static bool DoesUserNameExist(string username)
        {
            string query = "SELECT * FROM User WHERE username=?";
            return (GetConn().Query<User>(query, username).Count > 0);
        }

        public static void InsertUser(User user)
        {
            GetConn().Insert(user);
        }
    }
}