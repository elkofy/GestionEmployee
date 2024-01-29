using GestionEmployee.Dtos.Department;
using GestionEmployee.Dtos.Employee;
using GestionEmployee.Entities;
using GestionEmployee.Repositories.Contracts;
using GestionEmployee.Repositories.Implementations;
using GestionEmployee.Services.Contracts;
using GestionEmployee.Utils;

namespace GestionEmployee.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartementRepository _departementRepository;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        public EmployeeService(IEmployeeRepository employeeRepository, IDepartementRepository departementRepository,
        IAttendanceRepository attendanceRepository, ILeaveRequestRepository leaveRequestRepository)
        {
            _employeeRepository = employeeRepository;
            _departementRepository = departementRepository;
            _attendanceRepository = attendanceRepository;
            _leaveRequestRepository = leaveRequestRepository;
        }
        public async Task<List<ReadEmployee>> GetEmployees()
        {
            var employees = await _employeeRepository.GetEmployeesAsync();

            List<ReadEmployee> readEmployees = new List<ReadEmployee>();

            foreach (var employee in employees)
            {
                readEmployees.Add(new ReadEmployee()
                {
                    Id = employee.EmployeeId,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                });
            }

            return readEmployees;
        }


        public async Task<DetailEmployee> GetEmployeeByIdAsync(int employeeId)
        {
            var employee = await _employeeRepository.GetEmployeeByIdWithDepartmentAsync(employeeId);

            if (employee is null)
                throw new Exception($"Echec de recupération des informations de l'employé car il n'existe pas : {employeeId}");


           List<String> departmentNames = new List<String>();
            foreach (var department in employee.EmployeesDepartments)
            {
                var departementName = _departementRepository.GetDepartmentAsync(department.DepartmentId);
                departmentNames.Add(departementName.Result.Name);
            }

            return new DetailEmployee()
            {
                Id = employee.EmployeeId,
                FirstName = employee.FirstName, 
                LastName = employee.LastName,
                BirthDate = employee.BirthDate,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                Position = employee.Position,
                Departements = departmentNames
            };
        }

        public async Task UpdateEmployeeAsync(int employeeId, UpdateEmployee employee)
        {
            var employeeGet = await _employeeRepository.GetEmployeeByIdAsync(employeeId)
                ?? throw new Exception($"Echec de mise à jour d'un employé : Il n'existe aucun employé avec cet identifiant : {employeeId}");

            var employeeGetByEmail = await _employeeRepository.GetEmployeeByEmailAsync(employee.Email);
            if (employeeGetByEmail is not null && employeeId != employeeGetByEmail.EmployeeId)
            {
                throw new Exception($"Echec de mise à jour d'un employé : Il existe déjà un employé avec cette Email {employeeGetByEmail.Email}");
            }
            ValidateEmployee(employee);
            employeeGet.FirstName = employee.FirstName.Trim();
            employeeGet.LastName = employee.LastName.Trim();
            employeeGet.BirthDate = employee.BirthDate;
            employeeGet.Email = employee.Email.Trim();
            employeeGet.PhoneNumber = employee.PhoneNumber.Trim();
            employeeGet.Position = employee.Position.Trim();

            await _employeeRepository.UpdateEmployeeAsync(employeeGet);
        }

        public async Task AddDepartmentToEmployee(int employeeId, int departmentId)
        {
            var employee = await _employeeRepository.GetEmployeeByIdWithDepartmentAsync(employeeId);
            if (employee == null)
            {
                throw new Exception($"Echec d'ajout d'un département à un employé : Il n'existe aucun employé avec cet identifiant : {employeeId}");
            }

            if (employee.EmployeesDepartments.Any(x => x.DepartmentId == departmentId))
            {
                throw new Exception($"L'employé est déjà associé à ce département : {departmentId}");
            }

            var department = _departementRepository.GetDepartmentAsync(departmentId);
            if (department == null)
            {
                throw new Exception($"Echec d'ajout d'un département à un employé : Il n'existe aucun département avec cet identifiant : {departmentId}");
            }

            var employeeDepartment = new EmployeeDepartment
            {
                EmployeeId = employeeId,
                DepartmentId = departmentId
            };

            await _employeeRepository.AddEmployeeDepartment(employeeDepartment);
        }

        public async Task RemoveDepartmentFromEmployee(int employeeId, int departmentId)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeId);
            if (employee == null)
            {
                throw new Exception($"Echec d'ajout d'un département à un employé : Il n'existe aucun employé avec cet identifiant : {employeeId}");
            }

            var department = await _departementRepository.GetDepartmentAsync(departmentId);
            if (department == null)
            {
                throw new Exception($"Il n'existe aucun département avec cet identifiant : {departmentId}");
            }

            await _employeeRepository.RemoveEmployeeDepartment(employeeId, departmentId);
        }

        public async Task DeleteEmployeeById(int employeeId)
        {
            var employeeGet = await _employeeRepository.GetEmployeeByIdAsync(employeeId)
                ?? throw new Exception($"Echec de suppression d'un employé : Il n'existe aucun employé avec cet identifiant : {employeeId}");

            var attendances = await _attendanceRepository.GetAttendanceByEmployeeIdAsync(employeeId);
            if (attendances.Any())
            {
                throw new Exception($"Echec de suppression d'un employé : il possède des présences");
            }

            var leaveRequest = await _leaveRequestRepository.GetLeaveRequestsByEmployeeIdAsync(employeeId);
            if (leaveRequest.Any())
            {
                throw new Exception($"Echec de suppression d'un employé : il possède des congés");
            }

            await _employeeRepository.RemoveEmployeeFromDepartments(employeeId);

            await _employeeRepository.DeleteEmployeeByIdAsync(employeeId);
        }

        public async Task<ReadEmployee> CreateEmployeeAsync(CreateEmployee employee)
        {
            var employeeGet = await _employeeRepository.GetEmployeeByEmailAsync(employee.Email);
            if (employeeGet is not null)
            {
                throw new Exception($"il existe déjà un employé avec cette email {employee.Email}");
            }

            ValidateEmployee(employee);

            employee.FirstName = employee.FirstName.Trim();
            employee.LastName = employee.LastName.Trim();
            employee.Position = employee.Position.Trim();
            employee.Email = employee.Email.Trim();
            employee.PhoneNumber = employee.PhoneNumber.Trim();


            var employeeTocreate = new Employee()
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                BirthDate = employee.BirthDate,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                Position = employee.Position,
            };

            var employeeCreated = await _employeeRepository.CreateEmployeeAsync(employeeTocreate);

            return new ReadEmployee()
            {
                Id = employeeCreated.EmployeeId,
                FirstName = employeeCreated.FirstName,
                LastName = employeeCreated.LastName,
                Email = employeeCreated.Email,
            };
        }

        public async Task<List<ReadDepartment>> GetDepartmentsForEmployee(int employeeId)
        {
            var employee = await _employeeRepository.GetEmployeeByIdWithDepartmentAsync(employeeId);

            if (employee == null)
            {
                throw new Exception($" L'employé {employeeId} n'existe pas.");
            }

            var departments = employee.EmployeesDepartments
                .Select(ed => new ReadDepartment
                {
                    Id = ed.Department.DepartmentId,
                    Name = ed.Department.Name,
                })
                .ToList();

            if (!departments.Any())
            {
                throw new Exception($"L'employé {employeeId} n'est associé à aucun départements.");
            }

            return departments;
        }

        private void ValidateEmployee(dynamic employee)
        {
            Validator.isStringValid(employee.FirstName, "Le prénom", 2, 30);
            Validator.isStringValid(employee.LastName, "Le nom de famille", 2, 30);
            Validator.isStringValid(employee.Position, "Le poste", 5, 20);
            Validator.isMailValid(employee.Email);
            Validator.IsValidPhoneNumber(employee.PhoneNumber);
            Validator.hasMinimumWorkingAge(employee.BirthDate);
            Validator.noSpecialCharacterAllowed(employee.FirstName, "Le nom");
            Validator.noSpecialCharacterAllowed(employee.LastName, "Le nom de famille");
            Validator.noSpecialCharacterAllowed(employee.Position, "Le poste");
        } 
    }
}

