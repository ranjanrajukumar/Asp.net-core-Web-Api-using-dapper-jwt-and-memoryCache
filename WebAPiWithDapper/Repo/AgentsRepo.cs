using WebAPiWithDapper.Context;
using WebAPiWithDapper.Entities;
using WebAPiWithDapper.IRepo;
using Dapper;

namespace WebAPiWithDapper.Repo
{
    public class AgentsRepo : IAgentsRepo
    {
        //private readonly DapperContext _context;

        //public AgentsRepo(DapperContext context)
        //{
        //    _context = context;
        //}

        private readonly IDapperContext _dbService;

        public AgentsRepo(IDapperContext dbService)
        {
            _dbService = dbService;
        }

        //public async Task<IEnumerable<Agents>> GetAgents()
        //{
            
        //    var query = "select Agents.AGENT_CODE,AGENT_NAME,Agents.WORKING_AREA,COMMISSION,Agents.PHONE_NO,COUNTRY,CUST_CODE,CUST_NAME,CUST_CITY,CUST_COUNTRY,GRADE,OPENING_AMT,RECEIVE_AMT,PAYMENT_AMT,OUTSTANDING_AMT from Agents Left join Customer on Customer.AGENT_CODE = Agents.AGENT_CODE";
        //    using (var connection = _context.CreateConnection())
        //    {
        //        var agents = await connection.QueryAsync<Agents>(query);
        //        return agents.ToList();
        //    }
           
        //}

        public async Task<IEnumerable<Agents>> GetAgents()
        {
            var query = "select Agents.AGENT_CODE,AGENT_NAME,Agents.WORKING_AREA,COMMISSION,Agents.PHONE_NO,COUNTRY,CUST_CODE,CUST_NAME,CUST_CITY,CUST_COUNTRY,GRADE,OPENING_AMT,RECEIVE_AMT,PAYMENT_AMT,OUTSTANDING_AMT from Agents Left join Customer on Customer.AGENT_CODE = Agents.AGENT_CODE";
            var agents = await _dbService.GetAll<Agents>(query, new { });
            return agents;
        }
    }
}
