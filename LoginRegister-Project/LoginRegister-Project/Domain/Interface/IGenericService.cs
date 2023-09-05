using LoginRegister_Project.Domain.DTOs;

namespace LoginRegister_Project.Domain.Interface
{
    public interface IGenericService<T> where T : class
    {
        public Task<List<T>> GetAllUser();
        public Task<T> AddUser(T user);
        public Task<T> GetUser(string email);
        public Task DeleteUser(int id);
        public Task<T> UpdateUser(T user);
    }
}
