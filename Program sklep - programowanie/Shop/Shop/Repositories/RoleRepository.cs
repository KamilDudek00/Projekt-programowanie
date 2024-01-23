using Shop.Models;

namespace Shop.Repositories
{
    public class RoleRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public RoleRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Role GetRole(RoleType roleType)
        {
            var role = _dbContext.Roles.Where(x => x.Name == roleType.ToString()).FirstOrDefault();
            if(role == null)
            {
                role = new Role()
                {
                    Name = roleType.ToString(),
                };
                _dbContext.Roles.Add(role);
                _dbContext.SaveChanges();
                return role;
            }
            else
            {
                return role;
            }
        }
    }
}