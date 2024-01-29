using GestionEmployee.Dtos.LeaveRequestStatus;

namespace GestionEmployee.Services.Contracts
{
    public interface ILeaveRequestStatusService
    {
        Task<List<ReadLeaveRequestStatus>> GetLeaveRequestStatuses();
        Task<ReadLeaveRequestStatus> GetLeaveRequestStatusById(int leaveRequestStatusId);
        Task<ReadLeaveRequestStatus> CreateLeaveRequestStatusAsync(CreateLeaveRequestStatus leaveRequestStatus);
        Task UpdateLeaveRequestStatus(int leaveRequestStatusId, UpdateLeaveRequestStatus updateLeaveRequestStatus);
        Task DeleteLeaveRequestStatus(int leaveRequestStatusId);
    }
}
