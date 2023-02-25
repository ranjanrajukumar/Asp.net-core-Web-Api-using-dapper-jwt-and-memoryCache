using WebAPiWithDapper.Context;
using WebAPiWithDapper.Entities;
using WebAPiWithDapper.IRepo;

namespace WebAPiWithDapper.Repo
{
    public class UsersRepo:IUsersRepo
    {
        private readonly IDapperContext _dbService;

        public UsersRepo(IDapperContext dbService)
        {
            _dbService = dbService;
        }

        
        public async Task<IEnumerable<Users1>> GetUserData(string userId, string password)
        {
            var query = "Select user1,password from Users where user1 = '"+userId+"' and password = '"+password+"' ";
            
            var user1 = await _dbService.GetAll<Users1>(query, new { });
              return user1;         
        }

        public async Task<IEnumerable<Users>> GetUserData()
        {
            
            var query = "Select UserName,Password,EmailId from UserLogins";
            var user = await _dbService.GetAll<Users>(query, new { });
            return user.ToList();
        }
    }
}
