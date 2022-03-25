using Dapper;
using EconomicManagementAPP.Models;
using Microsoft.Data.SqlClient;

namespace EconomicManagementAPP.Services
{
    public interface IRepositorieAccounts
    {
        Task Create(Accounts account);
        Task<bool> Exist(string name, int userId);
        Task<IEnumerable<Accounts>> GetAccounts(int userId);
        Task Modify(Accounts account);
        Task<Accounts> GetAccountById(int id, int userId);
        Task Delete(int id);
    }
    public class RepositorieAccounts : IRepositorieAccounts
    {
        private readonly string connectionString;

        public RepositorieAccounts(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task Create(Accounts account)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"INSERT INTO Accounts 
                                                            (Name, AccountTypeId, Balance, Description) 
                                                            VALUES (@Name, @AccountTypeId, @Balance, @Description); 
                                                            SELECT SCOPE_IDENTITY();", account);
            account.Id = id;
        }

        public async Task<bool> Exist(string name, int userId)
        {
            using var connection = new SqlConnection(connectionString);
            var exist = await connection.QueryFirstOrDefaultAsync<int>(
                                    @"SELECT 1 FROM Accounts a
                                    JOIN AccountTypes at ON at.Id = a.AccountTypeId
                                    JOIN Users u ON u.Id = at.UserId
                                    WHERE Name = @name u.Id = @userId;",
                                    new { name, userId });
            return exist == 1;
        }

        public async Task<IEnumerable<Accounts>> GetAccounts(int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Accounts>(@"SELECT Id, Name, AccountTypeId, Balance, Description 
                                                        FROM Accounts a
                                                        JOIN AccountTypes at ON at.Id = a.AccountTypeId
                                                        JOIN Users u ON u.Id = at.UserId
                                                        WHERE u.Id = @userId;", new { userId });
        }
        public async Task Modify(Accounts account)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Accounts SET 
                                            Name = @Name,
                                            AccountTypeId = @AccountTypeId,
                                            Balance = @Balance,
                                            Description = @Description
                                            WHERE Id = @Id", account);
        }

        public async Task<Accounts> GetAccountById(int id, int userId)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryFirstOrDefaultAsync<Accounts>(@"SELECT Id, Name, AccountTypeId, Balance, Description
                                                                       FROM Account a
                                                                       JOIN AccountTypes at ON at.Id = a.AccountTypeId
                                                                       JOIN Users u ON u.Id = at.UserId
                                                                       WHERE Id = @id AND u.Id = @userId",
                                                                       new { id, userId  });
        }

        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE Accounts WHERE Id = @Id", new { id });
        }
    }
}
