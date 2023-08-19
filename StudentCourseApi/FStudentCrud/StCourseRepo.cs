using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using StudentCourseApi.Models;

namespace StudentCourseApi.StudentCourseCRUD
{
    public class StCourseRepo<T> : IRepository<T> where T : class
    {

        private StudentCourseContext _context;
        public StCourseRepo(StudentCourseContext context) {
            this._context = context;
        }
        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);

        }

        public async Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll() {
            return _context.Set<T>().ToArray();
        }
    }

}
