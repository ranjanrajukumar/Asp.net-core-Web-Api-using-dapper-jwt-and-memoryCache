using WebAPiWithDapper.Entities;

namespace WebAPiWithDapper.IRepo
{
    public interface IAgentsRepo
    {
        public Task<IEnumerable<Agents>> GetAgents();
    }
}
