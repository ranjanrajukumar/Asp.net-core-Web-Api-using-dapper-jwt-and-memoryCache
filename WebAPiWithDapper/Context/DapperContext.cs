using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace WebAPiWithDapper.Context
{
    public class DapperContext:IDapperContext
    {
        //private readonly IConfiguration _configuration;
        //private readonly string _connectionString;            

        //public DapperContext(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //    _connectionString = _configuration.GetConnectionString("DefaultConnection");
        //}

    //    public IDbConnection CreateConnection()
    //=> new SqlConnection(_connectionString);

        private readonly IDbConnection _db;

        public DapperContext(IConfiguration configuration)
        {
            _db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        }
                                    


        public async Task<int> EditData(string command, object parms)
        {
            int result;

            result = await _db.ExecuteAsync(command, parms);

            return result;
        }

        public async Task<List<T>> GetAll<T>(string command, object parms)
        {

            List<T> result = new List<T>();

            result = (await _db.QueryAsync<T>(command, parms)).ToList();

            return result;
        }


        public async Task<T> GetAsync<T>(string command, object parms)
        {
            T result;

            result = (await _db.QueryAsync<T>(command, parms).ConfigureAwait(false)).FirstOrDefault();

            return result;

        }
    }
}