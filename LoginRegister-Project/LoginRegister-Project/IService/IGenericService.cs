namespace LoginRegister_Project.IService
{
    public interface IGenericService<T> where T : class
    {
        public Task<T> GetUser(int id);
        public Task<T> AddUser(T user);
    }
}
