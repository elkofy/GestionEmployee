﻿using GestionEmployee.Dtos.LeaveRequest;

namespace GestionEmployee.Services.Contracts
{
    public interface ILeaveRequestService
    {
        Task<List<ReadLeaveRequest>> GetLeaveRequestsByEmployeeId(int employeeId);
        Task<ReadLeaveRequest> GetLeaveRequestById(int leaveRequestId);
        Task<ReadLeaveRequest> CreateLeaveRequestAsync(CreateLeaveRequest leaveRequest);
        Task UpdateLeaveRequestStatus(int leaveRequestId, UpdateLeaveRequest updateLeaveRequestStatus);
        Task DeleteLeaveRequestById(int leaveRequestId);
    }
}
