using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionEmployee.Infrastructure.DataBase;
using GestionEmployee.Dtos.Attendance;
using GestionEmployee.Services.Contracts;

namespace GestionEmployee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendancesController : ControllerBase
    {
            private readonly IAttendanceService _attendanceService;

            public AttendancesController(IAttendanceService attendanceService)
            {
                _attendanceService = attendanceService;
            }

            [HttpGet("employee/{employeeId}")]
            public async Task<ActionResult<List<ReadAttendance>>> GetAttendanceByEmployeeId(int employeeId)
            {
                if (employeeId < 0)
                {
                    return BadRequest("l'id est inférieur à 0");
                }
                try
                {
                    var attendanceList = await _attendanceService.GetAttendanceByEmployeeId(employeeId);
                    return Ok(attendanceList);
                }
                catch (Exception ex)
                {
                    return Problem(ex.Message);
                }
            }

            [HttpGet("{attendanceId}")]
            public async Task<ActionResult<ReadAttendance>> GetAttendanceById(int attendanceId)
            {
                if (attendanceId < 0)
                {
                return BadRequest("l'id est inférieur à 0");
                }

                try
                {
                    var attendance = await _attendanceService.GetAttendanceById(attendanceId);
                    return Ok(attendance);
                }
                catch (Exception ex)
                {
                    return Problem(ex.Message);
                }
            }

            [HttpPost]
            public async Task<ActionResult<ReadAttendance>> CreateAttendance([FromBody] CreateAttendance attendance)
            {
                try
                {
                    var attendanceId = await _attendanceService.CreateAttendanceAsync(attendance);
                    return Ok(attendanceId);
                }
                catch (Exception ex)
                {
                    return Problem(ex.Message);
                }
            }

            [HttpDelete("{id}")]
            public async Task<ActionResult> DeleteAttendance(int id)
            {
                if (id < 0)
                {
                return BadRequest("l'id est inférieur à 0");
            }

            try
                {
                    await _attendanceService.DeleteAttendanceById(id);
                    return Ok($"la présence {id} à été supprimé");
                }
                catch (Exception ex)
                {
                    return Problem(ex.Message);
                }
            }
        }
    }

