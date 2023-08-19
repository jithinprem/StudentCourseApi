namespace StudentCourseApi.StudentCourseCRUD
{
    public interface IRepository<T>
    {
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task DeleteAsync(T entity);

        IEnumerable<T> GetAll();
    }
}
