    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionEmployee.Infrastructure.DataBase;
using GestionEmployee.Dtos.LeaveRequest;
using GestionEmployee.Services.Contracts;
using GestionEmployee.Entities;

namespace GestionEmployee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestController : ControllerBase
    {
        private readonly ILeaveRequestService _leaveRequestService;

        public LeaveRequestController(ILeaveRequestService leaveRequestService)
        {
            _leaveRequestService = leaveRequestService;
        }

        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<List<ReadLeaveRequest>>> GetLeaveRequestsByEmployeeId(int employeeId)
        {
            if (employeeId < 0)
            {
                return BadRequest("l'id de l'employée est inférieur à 0");
            }

            try
            {
                var leaveRequestList = await _leaveRequestService.GetLeaveRequestsByEmployeeId(employeeId);
                return Ok(leaveRequestList);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("{leaveRequestId}")]
        public async Task<ActionResult<ReadLeaveRequest>> GetLeaveRequestById(int leaveRequestId)
        {
            if (leaveRequestId < 0)
            {
                return BadRequest("l'id de la demande de congé est inférieur à 0");
            }

            try
            {
                var leaveRequest = await _leaveRequestService.GetLeaveRequestById(leaveRequestId);
                return Ok(leaveRequest);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ReadLeaveRequest>> CreateLeaveRequest([FromBody] CreateLeaveRequest leaveRequest)
        {
      
            try
            {
                var leaveRequestCreate = await _leaveRequestService.CreateLeaveRequestAsync(leaveRequest);
                return Ok(leaveRequestCreate);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut("status/{leaveRequestId}")]
        public async Task<ActionResult> UpdateLeaveRequestStatus(int leaveRequestId, [FromBody] UpdateLeaveRequest updateLeaveRequestStatus)
        {
            if (leaveRequestId < 0)
            {
                return BadRequest("l'id de la demande de congé est inférieur à 0");
            }

            try
            {
                await _leaveRequestService.UpdateLeaveRequestStatus(leaveRequestId, updateLeaveRequestStatus);
                return Ok($"la demande de congé {leaveRequestId} a été mise à jour.");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLeaveRequest(int id)
        {
            if (id < 0)
            {
                return BadRequest("l'id de la demande de congé est inférieur à 0");
            }

            try
            {
                await _leaveRequestService.DeleteLeaveRequestById(id);
                return Ok($"la demande de congé {id } a été supprimé");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
