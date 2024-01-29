using GestionEmployee.Entities;

namespace GestionEmployee.Repositories.Contracts
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetEmployeesAsync();

        Task<Employee> GetEmployeeByIdAsync(int employeeId);

        Task<Employee> GetEmployeeByEmailAsync(string employeeEmail);

        Task<Employee> GetEmployeeByIdWithDepartmentAsync(int employeeId);

        Task UpdateEmployeeAsync(Employee employeeToUpdate);

        Task AddEmployeeDepartment(EmployeeDepartment employeeDepartment);

        Task RemoveEmployeeDepartment(int employeeId, int departmentId);

        Task RemoveEmployeeFromDepartments(int employeeId);

        Task<Employee> CreateEmployeeAsync(Employee employeeToCreate);

        Task<Employee> DeleteEmployeeByIdAsync(int employeeId);
    }
}
