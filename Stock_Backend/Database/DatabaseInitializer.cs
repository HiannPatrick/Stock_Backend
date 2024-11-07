using MySql.Data.MySqlClient;
using System.Reflection;

namespace Stock_Backend.Database
{
    public class DatabaseInitializer
    {
        private readonly string _connectionString;

        public DatabaseInitializer( string connectionString = "Server=localhost;Database=sakila;User=root;Password=root;" )
        {
            _connectionString = connectionString;
        }

        public void InitializeDatabase()
        {
            using( var connection = new MySqlConnection( _connectionString ) )
            {
                string assemblyLocation = Assembly.GetExecutingAssembly().Location;
                string directory = Directory.GetCurrentDirectory();
                string[] files = Directory.GetFiles(directory, "*.sql", SearchOption.AllDirectories);

                connection.Open();

                foreach( string file in files.OrderBy( o => o ) )
                {
                    string script = File.ReadAllText(file);

                    using( var command = new MySqlCommand( script, connection ) )
                    {
                        command.ExecuteNonQuery();
                    }
                }

                connection.Close();
            }
        }
    }
}
