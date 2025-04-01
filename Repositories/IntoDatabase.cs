using System.Runtime.InteropServices.Marshalling;
using Dapper;
using Microsoft.Data.SqlClient;
using Avans.Identity.Dapper;

namespace WebApi.Repositories
{
    public class IntoDatabase
    {
        //int lastInsertedId = await connection.ExecuteScalarAsync<int>("INSERT INTO [SomeObject] OUTPUT INSERTED.Id VALUES ( @Name)", someObject);
    }
}
