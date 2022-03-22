using EconomicManagementAPP.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace EconomicManagementAPP.Services
{
    public interface IRepositorieAccountTypes
    {
        Task Create(AccountTypes accountTypes); // Se agrega task por el asincronismo
        Task<bool> Exist(string Name, int UserId);
        Task<IEnumerable<AccountTypes>> getAccounts(int UserId);
    }
    public class RepositorieAccountTypes : IRepositorieAccountTypes
    {
        private readonly string connectionString;

        public RepositorieAccountTypes(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        // El async va acompañado de Task
        public async Task Create(AccountTypes accountTypes)
        {
            using var connection = new SqlConnection(connectionString);
            // Requiere el await - tambien requiere el Async al final de la query
            var id = await connection.QuerySingleAsync<int>($@"INSERT INTO AccountTypes 
                                                (Name, UserId, OrderAccount) 
                                                VALUES (@Name, @UserId, @OrderAccount); SELECT SCOPE_IDENTITY();", accountTypes);
            accountTypes.Id = id;
        }

        //Cuando retorna un tipo de dato se debe poner en el Task Task<bool>
        public async Task<bool> Exist(string Name, int UserId)
        {
            using var connection = new SqlConnection(connectionString);
            // El select 1 es traer lo primero que encuentre y el default es 0
            var exist = await connection.QueryFirstOrDefaultAsync<int>(
                                    @"SELECT 1
                                    FROM AccountTypes
                                    WHERE Name = @Name AND UserId = @UserId;",
                                    new { Name, UserId });
            return exist == 1;
        }

        // Obtenemos las cuentas del usuario
        public async Task<IEnumerable<AccountTypes>> getAccounts(int UserId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<AccountTypes>(@"SELECT Id, Name, OrderAccount
                                                            FROM AccountTypes
                                                            WHERE UserId = @UserId
                                                            ORDER BY OrderAccount", new { UserId });
        }
    }
}
