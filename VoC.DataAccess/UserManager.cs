using BusinessModel;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.Text;

namespace VoC.DataAccess
{
    public class UserManager
    {
        public bool AddNewUser(string login, string password)
        {
            if (!CheckUser(login))
            {
                return false;
            }

            string commandText = "insert into User (Login, Password, Token, Start, ShelfLife) values (@login, @password, @token, @start, 0)";
            string addProfileCommandText = "Insert into UserProfile (Id,AverageTime, LastRequest, RequestCounter)Values(@id,@averageTime, @lastRequest, @requestCounter)";
            var data = Encoding.Default.GetBytes(password);
            var hashFunction = new SHA1CryptoServiceProvider();
            var hashedPassword = hashFunction.ComputeHash(data);

            using (SQLiteConnection connection = new SQLiteConnection(DbInit.DbInit.ConnectionString))
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();

                command.CommandText = commandText;
                command.Parameters.AddWithValue("login", login);
                command.Parameters.AddWithValue("password", Encoding.Default.GetString(hashedPassword));
                command.Parameters.AddWithValue("token", Guid.NewGuid().ToString());
                command.Parameters.AddWithValue("start", DateTime.Now);
                var userId = command.ExecuteNonQuery(System.Data.CommandBehavior.KeyInfo);

                command = connection.CreateCommand();

                command.CommandText = addProfileCommandText;
                command.Parameters.AddWithValue("id", userId);
                command.Parameters.AddWithValue("averageTime", 0);
                command.Parameters.AddWithValue("lastRequest", DateTime.Now);
                command.Parameters.AddWithValue("requestCounter", 0);
                command.ExecuteNonQuery();
            }
            return true;
        }

        private bool CheckUser(string login)
        {
            string commandText = "Select Count(*) From User where Login = @login";

            using (SQLiteConnection connection = new SQLiteConnection(DbInit.DbInit.ConnectionString))
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();

                command.CommandText = commandText;
                command.Parameters.AddWithValue("login", login);
                var result = command.ExecuteReader();
                var isExists = result.Read();
                result.Close();
                return isExists;
            }
        }

        public bool IsAuthorize(string token)
        {
            string commandText = "Select * from User where Token = @token";

            bool isAuthorize = false;

            using (SQLiteConnection connection = new SQLiteConnection(DbInit.DbInit.ConnectionString))
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();

                command.CommandText = commandText;
                command.Parameters.AddWithValue("token", token);
                var result = command.ExecuteReader();

                if (result.Read())
                {
                    isAuthorize = DateTime.Parse(result["start"].ToString()).AddHours(20) > DateTime.Now;
                    result.Close();
                    if (!isAuthorize)
                    {
                        commandText = "update User set Token = @token where Token = @oldToken";

                        command = connection.CreateCommand();

                        command.CommandText = commandText;
                        command.Parameters.AddWithValue("token", Guid.NewGuid().ToString());
                        command.Parameters.AddWithValue("oldToken", token);
                        command.ExecuteNonQuery();
                    }
                }

            }

            return isAuthorize;
        }

        public void SignOut(string token)
        {
            string commandText = "update User set Token = @token where Token = @oldToken";

            using (SQLiteConnection connection = new SQLiteConnection(DbInit.DbInit.ConnectionString))
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();

                command.CommandText = commandText;
                command.Parameters.AddWithValue("token", Guid.NewGuid());
                command.Parameters.AddWithValue("oldToken", token);
                command.ExecuteNonQuery();
            }
        }

        public bool Login(string login, string password, out string token)
        {
            var newToken = Guid.NewGuid();

            string commandText = "update User set Token = @token where Password = @password and Login = @login";

            using (SQLiteConnection connection = new SQLiteConnection(DbInit.DbInit.ConnectionString))
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();

                var data = Encoding.Default.GetBytes(password);
                var hashFunction = new SHA1CryptoServiceProvider();
                var hashedPassword = hashFunction.ComputeHash(data);

                command.CommandText = commandText;
                command.Parameters.AddWithValue("token", newToken.ToString());
                command.Parameters.AddWithValue("password", Encoding.Default.GetString(hashedPassword));
                command.Parameters.AddWithValue("login", login);
                var result = command.ExecuteNonQuery();

                if (result < 1)
                {
                    token = string.Empty;
                    return false;
                }
            }
            token = newToken.ToString();
            return true;
        }

        public int GetCurrentUserId(string token)
        {
            string commandText = "Select Id from User where Token = @token";

            using (SQLiteConnection connection = new SQLiteConnection(DbInit.DbInit.ConnectionString))
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();

                command.CommandText = commandText;
                command.Parameters.AddWithValue("token", token);
                var user = command.ExecuteReader();
                var result = user.Read() ? int.Parse(user["Id"].ToString()) : -1;
                user.Close();
                return result;
            }
        }

        public List<UserModel> GetTop(int take)
        {
            string commandText = "SELECT profile.*, user.login FROM UserProfile as profile left join User as user on user.Id = profile.Id order by RequestCounter desc LIMIT @take";
            List<UserModel> userModels = new List<UserModel>();

            using (SQLiteConnection connection = new SQLiteConnection(DbInit.DbInit.ConnectionString))
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();

                command.CommandText = commandText;
                command.Parameters.AddWithValue("take", take);
                var users = command.ExecuteReader();
                while (users.Read())
                {
                    userModels.Add(new UserModel()
                    {
                        Average = users["AverageTime"].ToString(),
                        Count = users["LastRequest"].ToString(),
                        LastRequest = users["RequestCounter"].ToString(),
                        Name = users["Login"].ToString()
                    });
                }
                users.Close();
            }
            return userModels;
        }

        public void RegistredUserActivity(int userId)
        {
            string commandText = "Select * from UserProfile where Id = @userId";
            string updateCommandText = "Update UserProfile set AverageTime = @averageTime, LastRequest = @lastRequest, RequestCounter = @requestCounter where Id = @userId";

            using (SQLiteConnection connection = new SQLiteConnection(DbInit.DbInit.ConnectionString))
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();

                command.CommandText = commandText;
                command.Parameters.AddWithValue("userId", userId);
                var userProfile = command.ExecuteReader();
                userProfile.Read();
                var averageTime = (int.Parse(userProfile["AverageTime"].ToString()) + (DateTime.Now - DateTime.Parse(userProfile["LastRequest"].ToString())).TotalMinutes) / 2;
                var currentRequestCounter = int.Parse(userProfile["RequestCounter"].ToString()) + 1;
                userProfile.Close();

                command = connection.CreateCommand();

                command.CommandText = updateCommandText;
                command.Parameters.AddWithValue("userId", userId);
                command.Parameters.AddWithValue("averageTime", (int)averageTime);
                command.Parameters.AddWithValue("lastRequest", DateTime.Now);
                command.Parameters.AddWithValue("requestCounter", currentRequestCounter);
                command.ExecuteNonQuery();
            }
        }
    }
}
