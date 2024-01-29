using GestionEmployee.Dtos.Department;

namespace GestionEmployee.Services.Contracts
{
    public interface IDepartmentService
    {
        Task<List<ReadDepartment>> GetDepartments();
        Task<ReadDepartment?> GetDepartment(int departementId);
        Task<ReadDepartment> CreateDepartmentAsync(CreateDepartment department);
        Task DeleteDepartmentAsync(int departementId);
        Task UpdateDepartmentAsync(int departmentId, UpdateDepartment department);


    }
}
