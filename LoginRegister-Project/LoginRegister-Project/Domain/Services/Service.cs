using LoginRegister_Project.Domain.Models;
using LoginRegister_Project.Domain.Interface;
using Microsoft.EntityFrameworkCore;

namespace LoginRegister_Project.Domain.Services
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

        public async Task<List<T>> GetAllUser()
        {
            return await _dbcontext.Set<T>().ToListAsync();
        }

        public async Task<T> GetUser(string email)
        {
            return await _dbcontext.Set<T>().FindAsync(email);
        }


        public async Task DeleteUser(int id)
        {
            var deleteUser = await _dbcontext.Set<T>().FindAsync(id);
            _dbcontext.Set<T>().Remove(deleteUser);
            await _dbcontext.SaveChangesAsync();
        }
    }
}
