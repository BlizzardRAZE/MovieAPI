using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MovieApi.Models;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities;

namespace MovieApi.Repository {
    public class MovieRepository : IMovieRepository {
        private MySqlConnection _connection;

        public MovieRepository() {
            string server = "localhost";
            string userid = "csci330user";
            string password = "csci330pass";
            string database = "entertainmentmovies";

            string connectionString=$"server={server};userid={userid};password={password};database={database};";
            
            _connection = new MySqlConnection(connectionString);
            _connection.Open();
        }

        ~MovieRepository() {
            _connection.Close();
        }

        public IEnumerable<Movie> GetAll() {
            var statement = "Select * From Movies";
            var command = new MySqlCommand(statement, _connection);
            var results = command.ExecuteReader();

            List<Movie> newList = new List<Movie>(20);

            while (results.Read()) {
                Movie m = new Movie {
                    Name = (string)results[1],
                    Year = (int)results[2],
                    Genre = (string)results[3],
                };
                newList.Add(m);
            }

            results.Close();

            return newList;
        }

        public Movie GetMovieByName(string name) {
            var statement = "Select * From Movies Where Name = @userMovieName";
            var command = new MySqlCommand(statement, _connection);
            command.Parameters.AddWithValue("@userMovieName", name);
            
            var results = command.ExecuteReader();
            Movie m = null;
            if (results.Read()) {
                m = new Movie {
                    Name = (string)results[1],
                    Year = (int)results[2],
                    Genre = (string)results[3],
                };
            } 
            results.Close();
            return m;

        }

        public void InsertMovie(Movie m) {
            var statement = "Insert Into Movies (Name, Year, Genre) Values (@n, @y, @g)";
            var command = new MySqlCommand(statement, _connection);
            command.Parameters.AddWithValue("@n", m.Name);
            command.Parameters.AddWithValue("@y", m.Year);
            command.Parameters.AddWithValue("@g", m.Genre);

            int result = command.ExecuteNonQuery();
            Console.WriteLine(result);
        }

        public void UpdateMovie(string name, Movie movieIn) {
            var statement = "Update Movies Set Name=@newName, Year=@newYear, Genre=@newGenre Where name=@updateName";
            var command = new MySqlCommand(statement, _connection);
            command.Parameters.AddWithValue("@newName", movieIn.Name);
            command.Parameters.AddWithValue("@newYear", movieIn.Year);
            command.Parameters.AddWithValue("@newGenre", movieIn.Genre);
            command.Parameters.AddWithValue("@updateName", name);

            int result = command.ExecuteNonQuery();
            Console.WriteLine(result);
        }

        public void DeleteMovie (string name) {
            var statement = "Delete From Movies Where name=@n";
            var command = new MySqlCommand(statement, _connection);
            command.Parameters.AddWithValue("@n", name);

            int result = command.ExecuteNonQuery();
            Console.WriteLine(result);
        }

    }
}