using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionEmployee.Infrastructure.DataBase;
using GestionEmployee.Entities;

using GestionEmployee.Services.Contracts;
using GestionEmployee.Dtos.Department;

namespace GestionEmployee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departementService;

        public DepartmentsController(IDepartmentService departementService)
        {
            _departementService = departementService;
        }

        // GET: api/Departements
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartements()
        {
            try
            {
                var departements = await _departementService.GetDepartments();
                return Ok(departements);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        // GET: api/Departements/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartement(int id)
        {
            if (id < 0)
            {
                return BadRequest("l'id est inférieur à 0");
            }
            try
            {
                var departement = await _departementService.GetDepartment(id);
                return Ok(departement);

            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // PUT: api/Departements/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartement(int id, [FromBody] UpdateDepartment department)
        {
            if (id < 0)
            {
                return BadRequest("l'id est inférieur à 0");
            }

            try
            {
                await _departementService.UpdateDepartmentAsync(id, department);
                return Ok(new
                {
                    Message = $"Succès de la mise à jour du departement : {id}",
                });
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }

        //// POST: api/Departements
        [HttpPost]
        public async Task<ActionResult<Department>> PostDepartement([FromBody] CreateDepartment department)
        {

            if (department == null || string.IsNullOrWhiteSpace(department.Name)
              || string.IsNullOrWhiteSpace(department.Address) || string.IsNullOrWhiteSpace(department.Description))
            {
                return BadRequest("les informations sont null ou vides");
            }
            try
            {
                var departmentCreated = await _departementService.CreateDepartmentAsync(department);
                return Ok(departmentCreated);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        //// DELETE: api/Departements/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartement(int id)
        {
            try
            {
                await _departementService.DeleteDepartmentAsync(id);
                return Ok("La supression a bien été effectué");
            }
            catch (Exception ex) 
            {
                return Problem(ex.Message);
            }
        }
    }
}
