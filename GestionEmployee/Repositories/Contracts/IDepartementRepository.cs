using GestionEmployee.Entities;

namespace GestionEmployee.Repositories.Contracts
{
    public interface IDepartementRepository
    {
        Task<List<Department>> GetDepartmentsAsync();
        Task<Department?> GetDepartmentAsync(int departmentId);
        Task<Department?> GetDepartmentByNameAsync(string departmentName);
        Task<Department> CreateDepartmentAsync(Department departmentToCreate);
        Task<Department?> DeleteDepartmentAsync(int departmentId);
        Task UpdateDepartmentAsync(Department departmentToUpdate);


    }
}
