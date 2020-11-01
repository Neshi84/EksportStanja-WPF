using Dapper;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace EksportStanja.Repository
{
    internal class SifrarnikRepository : ISifrarnikRepository
    {
        public DatabaseOptions _databaseOptions { get; set; }

        public SifrarnikRepository(IOptions<DatabaseOptions> databaseOptions)
        {
            _databaseOptions = databaseOptions.Value;
        }

        public string Get(string jkl, string kpp)
        {
            string query;

            if (kpp == "071" || kpp == "074")
            {
                query = "SELECT FabrickoIme FROM sekundarLek WHERE SifraLeka = @jkl ";
            }
            else
            {
                query = "SELECT FabrickoIme FROM LEK WHERE SifraLeka = @jkl ";
            }

            using var connection = new SqlConnection(_databaseOptions.connectionString);
            string Id = connection.ExecuteScalar<string>(query, new { jkl });

            return Id;
        }
    }
}