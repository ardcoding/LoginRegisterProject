using LoginRegister_Project.IService;
using LoginRegister_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace LoginRegister_Project.Service
{
    public class Service<T> : IGenericService<T> where T : class
    {
        List<User> _user = new List<User>();
        private readonly UserDbContext _dbcontext;
        public Service(UserDbContext dbcontext) { 
            _dbcontext = dbcontext;
        }
        public async Task<T> AddUser(T entity)
        {
            await _dbcontext.Set<T>().AddAsync(entity);
            await _dbcontext.SaveChangesAsync();
            return entity;
        }

        public async Task<T> GetUser(int id)
        {
            return await _dbcontext.Set<T>().FindAsync(id);
        }
    }
}
