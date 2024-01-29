using GestionEmployee.Dtos.Department;
using GestionEmployee.Dtos.Employee;

namespace GestionEmployee.Services.Contracts
{
    public interface IEmployeeService
    {
        Task<List<ReadEmployee>> GetEmployees();
        Task<DetailEmployee> GetEmployeeByIdAsync(int employeeId);
        Task UpdateEmployeeAsync(int employeeId, UpdateEmployee employee);
        Task AddDepartmentToEmployee(int employeeId, int departmentId);
        Task RemoveDepartmentFromEmployee(int employeeId, int departmentId);
        Task DeleteEmployeeById(int employeeId);
        Task<ReadEmployee> CreateEmployeeAsync(CreateEmployee employee);
        Task<List<ReadDepartment>> GetDepartmentsForEmployee(int employeeId);
    }
}
