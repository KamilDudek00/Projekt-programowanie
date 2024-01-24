using Shop.Models;
namespace Shop.Repositories
{
    public class UserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddEntity(User entity)
        {
            _dbContext.Users.Add(entity);
            SaveChanges();
        }

        public IEnumerable<User> GetAllEntities()
        {
            return _dbContext.Users.ToList();
        }

        public User GetUserByEmail(string email)
        {
            return _dbContext.Users.FirstOrDefault(x => x.Email == email);
        }

        public void RemoveUserById(int id)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Id == id);
            _dbContext.Users.Remove(user);
            SaveChanges();
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}