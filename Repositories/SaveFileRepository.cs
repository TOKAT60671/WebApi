using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using WebApi;
using Dapper;
using Avans.Identity.Dapper;

/*namespace WebApi.Repositories
{

    public class SaveFileRepository
    {
        public async Task<SaveFileRepository> ReadAsync(int id)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString));
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<SaveFileRepository>("SELECT * FROM SaveFileRepository WHERE Id = @Id", new { Id = id });
            }
        }
    }
       
}*/