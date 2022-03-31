﻿using Dapper;
using EconomicManagementAPP.Interfaces;
using EconomicManagementAPP.Models;
using Microsoft.Data.SqlClient;

namespace EconomicManagementAPP.Services
{
    public class RepositorieTransactions : IRepositorieTransactions
    {
        private readonly string connectionString;
        public RepositorieTransactions(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task Create(Transactions transaction) 
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"INSERT INTO Transactions 
                                                            (UserId, TransactionDate, Total, OperationTypeId, Description, AccountId, CategoryId) VALUES
                                                            (@UserId, @TransactionDate, @Total, @OperationTypeId, @Description, @AccountId, @CategoryId); 
                                                            SELECT SCOPE_IDENTITY();", transaction);
            transaction.Id = id;
        }
        public async Task<Transactions> GetTransactionById(int id, int userId) 
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryFirstOrDefaultAsync<Transactions>(@"SELECT Id, UserId, TransactionDate, Total, OperationTypeId, Description, AccountId, CategoryId
                                                                           FROM Transactions
                                                                           WHERE Id = @id AND UserId = @userId",
                                                                           new { id, userId });
        }
        public async Task<IEnumerable<Transactions>> GetTransactions(int userId) 
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Transactions>(@"SELECT Id, UserId, TransactionDate, Total, OperationTypeId, Description, AccountId, CategoryId
                                                             FROM Transactions
                                                             WHERE UserId = @userId;", new { userId });
        }

        public async Task<IEnumerable<Transactions>> GetTransactions(int accountId, int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Transactions>(@"SELECT Id, UserId, TransactionDate, Total, OperationTypeId, Description, AccountId, CategoryId
                                                             FROM Transactions
                                                             WHERE AccountId = @accountId AND UserId = @userId;", new { accountId, userId });
        }

        public async Task Modify(Transactions transaction) 
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Transactions SET 
                                            Total = @Total,
                                            OperationTypeId = @OperationTypeId,
                                            Description = @Description,
                                            AccountId = @AccountId,
                                            CategoryId = @CategoryId
                                            WHERE Id = @Id", transaction);
        }
        public async Task Delete(int id) 
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE Transactions WHERE Id = @Id", new { id });
        }
    }
}
