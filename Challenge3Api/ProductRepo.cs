using Challenge3Api.DB;
using Challenge3Api.Models;
using System.Xml.Linq;

namespace Challenge3Api
{
    public interface IProductRepo
    {
        Task<Category> GetCategory(string name);

        Task CreateCategory(string categoryName);

        Task CreateProduct(Product product, int categoryId);

        Task RenameProduct(int id, string name);

        Task<Product> GetProduct(int id);

        IEnumerable<Product> GetProducts();

        IEnumerable<Product> GetProducts(string name);

        IEnumerable<Category> GetCategories();
    }

    /// <remarks>
    /// since we are using Sqlite none of these methods are going to be truly async. (more info: https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/async) 
    /// However designed the interface in a way that we can use it against other dbs that support async methods.
    /// </remarks>
    public class ProductRepo : IProductRepo
    {
        private readonly Database _database;

        public ProductRepo(Database db)
        {
            _database = db;
        }

        public IEnumerable<Category> GetCategories()
        {
            using var connection = _database.CreateConnection();
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Name FROM DrinkCategories";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                yield return new Category { Id = reader.GetInt32(0), Name = reader.GetString(1) };
            }
        }

        public Task<Category> GetCategory(string name)
        {
            using var connection = _database.CreateConnection();
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM DrinkCategories WHERE Name = $name";
            command.Parameters.AddWithValue("$name", name);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return Task.FromResult(new Category { Id = reader.GetInt32(0), Name = reader.GetString(1) });
            }
            else
            {
                return Task.FromResult<Category>(null);
            }
        }

        public Task CreateCategory(string categoryName)
        {
            using var connection = _database.CreateConnection();
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO DrinkCategories (Name) VALUEs ($name)";
            command.Parameters.AddWithValue("$name", categoryName.ToLower());
            if (command.ExecuteNonQuery() <= 0) 
            {
                throw new Exception("Failed to create category");
            }
            return Task.CompletedTask;
        }

        public Task CreateProduct(Product product, int categoryId)
        {
            using var connection = _database.CreateConnection();
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                @"INSERT INTO 
                    Products (Name, Category_ID, Flavor, SKU, Volume, Expiration_In_Days, PackagingType) 
                    VALUEs ($name, $category_id, $flavor, $sku, $volume, $expiration_period, $packaging_type)";
            command.Parameters.AddWithValue("$name", product.Name);
            command.Parameters.AddWithValue("$category_id", categoryId);
            command.Parameters.AddWithValue("$flavor", product.Flavor);
            command.Parameters.AddWithValue("$sku", product.SKU);
            command.Parameters.AddWithValue("$volume", product.Volume);
            command.Parameters.AddWithValue("$expiration_period", product.ExpirationInDays);
            command.Parameters.AddWithValue("$packaging_type", product.PackagingType.ToString());
            if (command.ExecuteNonQuery() <= 0)
            {
                throw new Exception("Failed to create product.");
            }
            return Task.CompletedTask;
        }

        public Task<Product> GetProduct(int id)
        {
            using var connection = _database.CreateConnection();
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"SELECT Products.Id, Products.Name, DrinkCategories.Name, Flavor, SKU, Volume, Expiration_In_Days, PackagingType
                            FROM Products 
                            JOIN DrinkCategories ON Products.Category_Id = DrinkCategories.Id 
                            WHERE Products.Id=$id";
            command.Parameters.AddWithValue("$id", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return Task.FromResult(new Product
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Category = reader.GetString(2),
                    Flavor = reader.GetString(3),
                    SKU = reader.GetString(4),
                    Volume = reader.GetInt32(5),
                    ExpirationInDays = reader.GetInt32(6),
                    PackagingType = Enum.Parse<PackagingType>(reader.GetString(7))
                });
            }
            else
            {
                return Task.FromResult<Product>(null);
            }

        }

        public IEnumerable<Product> GetProducts()
        {
            using var connection = _database.CreateConnection();
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"SELECT Products.Id, Products.Name, DrinkCategories.Name, Flavor, SKU, Volume, Expiration_In_Days, PackagingType
                            FROM Products 
                            JOIN DrinkCategories ON Products.Category_Id = DrinkCategories.Id";

            using var reader = command.ExecuteReader();
            while(reader.Read())
            {
                yield return new Product
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Category = reader.GetString(2),
                    Flavor = reader.GetString(3),
                    SKU = reader.GetString(4),
                    Volume = reader.GetInt32(5),
                    ExpirationInDays = reader.GetInt32(6),
                    PackagingType = Enum.Parse<PackagingType>(reader.GetString(7))
                };
            }
        }

        public IEnumerable<Product> GetProducts(string name)
        {
            using var connection = _database.CreateConnection();
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"SELECT Products.Id, Products.Name, DrinkCategories.Name, Flavor, SKU, Volume, Expiration_In_Days, PackagingType
                            FROM Products 
                            JOIN DrinkCategories ON Products.Category_Id = DrinkCategories.Id
                            WHERE Products.Name LIKE '%' || $name || '%'";
            command.Parameters.AddWithValue("$name", name);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                yield return new Product
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Category = reader.GetString(2),
                    Flavor = reader.GetString(3),
                    SKU = reader.GetString(4),
                    Volume = reader.GetInt32(5),
                    ExpirationInDays = reader.GetInt32(6),
                    PackagingType = Enum.Parse<PackagingType>(reader.GetString(7))
                };
            }
        }

        public Task RenameProduct(int id, string name)
        {
            using var connection = _database.CreateConnection();
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                @"UPDATE Products SET Name = $name WHERE Id=$id";
            command.Parameters.AddWithValue("$name", name);
            command.Parameters.AddWithValue("$id", id);

            if(command.ExecuteNonQuery() < 1)
            {
                throw new Exception("Failed to rename.");
            }

            return Task.CompletedTask;
        }
    }
}
