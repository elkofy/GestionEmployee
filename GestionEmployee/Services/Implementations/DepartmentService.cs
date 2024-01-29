using GestionEmployee.Dtos.Department;
using GestionEmployee.Entities;
using GestionEmployee.Repositories.Contracts;
using GestionEmployee.Services.Contracts;
using GestionEmployee.Utils;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GestionEmployee.Services.Implementations
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartementRepository _departementRepository;

        public DepartmentService(IDepartementRepository departementRepository)
        {
            _departementRepository = departementRepository;
        }
        public async Task<List<ReadDepartment>> GetDepartments()
        {
            var departments = await _departementRepository.GetDepartmentsAsync();

            List<ReadDepartment> readDepartments = new List<ReadDepartment>();

            foreach (var department in departments)
            {
                readDepartments.Add(new ReadDepartment()
                {
                    Id = department.DepartmentId,
                    Name = department.Name,
                    Address = department.Address,
                    Description = department.Description,
                });
            }

            return readDepartments;
        }

        public async Task<ReadDepartment?> GetDepartment( int departementId)
        {
            var department = await _departementRepository.GetDepartmentAsync(departementId);
            return department == null ? throw new Exception("Aucun Département pour l'id : " + departementId) : new ReadDepartment()
            {
                Id = department.DepartmentId,
                Name = department.Name,
                Address = department.Address,
                Description = department.Description,

            };
        }
        public async Task<ReadDepartment> CreateDepartmentAsync (CreateDepartment department)
        {
            var departmentGet = await _departementRepository.GetDepartmentByNameAsync(department.Name);
            if (departmentGet is not null)
            {
                throw new Exception($"Echec de création d'un département : Il existe déjà un département avec ce nom {department.Name}");
            }
            ValidateDepartement(department);

            department.Name = department.Name.Trim();
            department.Description = department.Description.Trim();
            department.Address = department.Address.Trim();

            var departementTocreate = new Department()
            {   
                Name = department.Name,
                Description = department.Description,
                Address = department.Address,
            };
            var departmentCreated = await _departementRepository.CreateDepartmentAsync(departementTocreate);

            return new ReadDepartment()
            {
                Id = departmentCreated.DepartmentId,
                Name = departmentCreated.Name,
                Address = department.Address,
                Description = department.Description,
            };
        }

        public async Task DeleteDepartmentAsync(int departementId)
        {
            var departement = await _departementRepository.GetDepartmentAsync(departementId);
           if (departement== null) {
                throw new Exception($"Aucun Département pour l'id {departementId}" );
            }
               await _departementRepository.DeleteDepartmentAsync(departementId);            
            
        }

        public async Task UpdateDepartmentAsync(int departmentId, UpdateDepartment department)
        {
            var departmentGet = await _departementRepository.GetDepartmentAsync(departmentId)
                ?? throw new Exception($"Il n'existe aucun departement avec cet identifiant : {departmentId}");

            var departmentGetByName = await _departementRepository.GetDepartmentByNameAsync(department.Name);
            if (departmentGetByName is not null && departmentId != departmentGetByName.DepartmentId)
            {
                throw new Exception($"Il existe déjà un département avec ce nom {department.Name}");
            }
            ValidateDepartement(department);

            departmentGet.Name = department.Name.Trim();
            departmentGet.Description = department.Description.Trim();
            departmentGet.Address = department.Address.Trim();

            await _departementRepository.UpdateDepartmentAsync(departmentGet);

        }

        private void ValidateDepartement(dynamic department)
        {

            Validator.isStringValid(department.Name, "Le nom", 2, 30);
            Validator.isStringValid(department.Address, "L'adresse", 10, 20);
            Validator.isStringValid(department.Description, "La description", 5, 255);
            Validator.noSpecialCharacterAllowed(department.Name, "Le nom");
            Validator.noSpecialCharacterAllowed(department.Address, "L'adresse");

        }




    }
}
