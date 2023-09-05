using LoginRegister_Project.Domain.Interface;
using LoginRegister_Project.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LoginRegister_Project.Domain.Services
{
    public class UserService : IGenericService<User>
    {
        private readonly UserDbContext _dbcontext;
        public UserService(UserDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task<User> AddUser(User user)
        {
            await _dbcontext.Set<User>().AddAsync(user);
            await _dbcontext.SaveChangesAsync();
            return user;
        }

        public async Task<List<User>> GetAllUser()
        {
            return await _dbcontext.Set<User>().ToListAsync();
        }

        public async Task<User> GetUser(string email)
        {
           return await _dbcontext.Users.FirstOrDefaultAsync(x=>x.Email == email);
        }

        public async Task DeleteUser(int id)
        {
            var deleteUser = await _dbcontext.Users.FindAsync(id);
            _dbcontext.Users.Remove(deleteUser);
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<User> UpdateUser(User user)
        {
            _dbcontext.Users.Update(user);
            await _dbcontext.SaveChangesAsync();
            return user;
        }

    }
}
