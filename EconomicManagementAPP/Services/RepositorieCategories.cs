using Dapper;
using EconomicManagementAPP.Models;
using Microsoft.Data.SqlClient;

namespace EconomicManagementAPP.Services
{
    public interface IRepositorieCategories 
    {
        Task Create(Categories categorie);
        Task<bool> Exist(string name);
        Task<Categories> GetOperationTypeById(int id);
        Task<IEnumerable<Categories>> GetCategories(int userId);
        Task Modify(Categories categorie);
        Task Delete(int id);
    }
    public class RepositorieCategories : IRepositorieCategories
    {
        private readonly string connectionString;
        public RepositorieCategories(IConfiguration configuration) 
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task Create(Categories categorie) 
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"INSERT INTO Categories 
                                                            (Name, OperationTypeId, UserId) VALUES
                                                            (@Name, @OperationTypeId, @UserId); 
                                                            SELECT SCOPE_IDENTITY();", categorie);
            categorie.Id = id;
        }
        public async Task<bool> Exist(string name)
        {
            using var connection = new SqlConnection(connectionString);
            var exist = await connection.QueryFirstOrDefaultAsync<int>(@"SELECT 1 FROM OperationTypes
                                                                        WHERE Name = @name",
                                                                        new { name });
            return exist == 1;
        }
        public async Task<Categories> GetOperationTypeById(int id)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryFirstOrDefaultAsync<Categories>(@"SELECT Id, Name, OperationTypeId, UserId
                                                                           FROM Categories
                                                                           WHERE Id = @id",
                                                                           new { id });
        }
        public async Task<IEnumerable<Categories>> GetCategories(int userId)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryAsync<Categories>(@"SELECT Id, Name, OperationTypeId, UserId
                                                             FROM Categories
                                                             WHERE UserId = @userId", new { userId });
        }
        public async Task Modify(Categories categorie)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Categories SET 
                                            Name = @Name,
                                            OperationTypeId = @OperationTypeID
                                            WHERE Id = @Id", categorie);
        }
        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE Categories WHERE Id = @Id", new { id });
        }
    }
}
