
using Application.Interface;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Data;

public class SqlDbConnection : IDbConnection<SqlConnection>

{   private readonly string _connectionString; 
    public SqlDbConnection(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SqlConnection GetConnection()
       =>  new SqlConnection(_connectionString);
    
}
