using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionEmployee.Infrastructure.DataBase;
using GestionEmployee.Dtos.Department;
using GestionEmployee.Dtos.Employee;
using GestionEmployee.Services.Contracts;
using System.Text.RegularExpressions;
using GestionEmployee.Services.Implementations;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GestionEmployee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<List<ReadEmployee>>> GetAll()
        {
            try
            {
                var employees = await _employeeService.GetEmployees();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Employees/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ReadEmployee>> GetEmployee(int id)
        {
            if (id < 0)
            {
                return BadRequest("l'id est inférieur à 0");
            }

            try
            {
                var employeeReceived = await _employeeService.GetEmployeeByIdAsync(id);
                return Ok(employeeReceived);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        // POST api/<EmployeesController>
        [HttpPost]
        public async Task<ActionResult<ReadEmployee>> Post([FromBody] CreateEmployee employee)
        {
            try
            {
                var employeeCreated = await _employeeService.CreateEmployeeAsync(employee);
                return Ok(employeeCreated);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        // PUT: api/Employees/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> PutEmployee(int id, [FromBody] UpdateEmployee employee)
        {
            if (id < 0)
            {
                return BadRequest("l'id est inférieur à 0");
            }

            try
            {
                await _employeeService.UpdateEmployeeAsync(id, employee);
                return Ok($"L'employé avec l'id {id} a été modifié");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            if (id < 0)
            {
                return BadRequest("l'id est inférieur à 0");
            }

            try
            {
                await _employeeService.DeleteEmployeeById(id);
                return Ok($"L'employé avec l'id {id} a été supprimé");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("{employeeId}/departments")]
        public async Task<ActionResult<List<ReadDepartment>>> GetDepartmentsForEmployee(int employeeId)
        {
            if (employeeId < 0)
            {
                return BadRequest("l'id est inférieur à 0");
            }
            try
            {
                var departments = await _employeeService.GetDepartmentsForEmployee(employeeId);
                return Ok(departments);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut("{employeeId}/departments/{departmentId}")]
        public async Task<ActionResult> AddDepartmentToEmployee(int employeeId, int departmentId)
        {
            if (employeeId < 0)
            {
                return BadRequest("l'id de l'employée est inférieur à 0");
            }
            if (departmentId < 0)
            {
                return BadRequest("l'id du département est inférieur à 0");
            }

            try
            {
                await _employeeService.AddDepartmentToEmployee(employeeId, departmentId);
                return Ok($"L'employé avec l'id {employeeId} a été ajouté au département");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete("{employeeId}/departments/{departmentId}")]
        public async Task<ActionResult> RemoveDepartmentFromEmployee(int employeeId, int departmentId)
        {
            if (employeeId < 0)
            {
                return BadRequest("l'id de l'employée est inférieur à 0");
            }
            if (departmentId < 0)
            {
                return BadRequest("l'id du departement est inférieur à 0");
            }

            try
            {
                await _employeeService.RemoveDepartmentFromEmployee(employeeId, departmentId);
                return Ok($"L'employé avec l'id {employeeId} a été supprimé du département");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

       
    }
}
