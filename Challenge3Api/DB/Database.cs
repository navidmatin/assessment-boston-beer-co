using Microsoft.Data.Sqlite;
using SQLitePCL;
using System.Xml.Linq;

namespace Challenge3Api.DB
{
    public class Database
    {
        private static readonly string _connectionString = $"Data Source=Db\\BostonBeer.db;Mode=ReadWriteCreate";

        public static async Task Initialize()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = await File.ReadAllTextAsync("DB\\Challenge2DbDesign.sql");
            command.ExecuteNonQuery();
        }

        public SqliteConnection CreateConnection()
        {
            return new SqliteConnection(_connectionString);
        }
    }
}
