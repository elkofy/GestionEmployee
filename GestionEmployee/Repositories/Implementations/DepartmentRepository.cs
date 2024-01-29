using GestionEmployee.Entities;
using GestionEmployee.Infrastructure.DataBase;
using GestionEmployee.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GestionEmployee.Repositories.Implementations
{
    public class DepartmentRepository : IDepartementRepository
    {
        private readonly GestionEmployeeContext _dbContext;
        public DepartmentRepository(GestionEmployeeContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Department>> GetDepartmentsAsync()
        {
            return await _dbContext.Departments.ToListAsync();
        }
        public async Task<Department?> GetDepartmentAsync( int departmentId)
        {
                return await _dbContext.Departments.FirstOrDefaultAsync(x => x.DepartmentId == departmentId);
        }
        public async Task<Department?> GetDepartmentByNameAsync(string departmentName)
        {
                return await _dbContext.Departments.FirstOrDefaultAsync(x => x.Name == departmentName);
        }
        public async Task<Department> CreateDepartmentAsync(Department departmentToCreate)
        {
            await _dbContext.Departments.AddAsync(departmentToCreate);
            await _dbContext.SaveChangesAsync();

            return departmentToCreate;
        }

        public async Task<Department?> DeleteDepartmentAsync(int departmentId)
        {
            var departmentToDelete = await _dbContext.Departments.FindAsync(departmentId);
            _dbContext.Departments.Remove(departmentToDelete);
            await _dbContext.SaveChangesAsync();
            return departmentToDelete;

        }
        public async Task UpdateDepartmentAsync(Department departmentToUpdate)
        {
            _dbContext.Departments.Update(departmentToUpdate);
            await _dbContext.SaveChangesAsync();
        }


    }
}
