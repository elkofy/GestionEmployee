using GestionEmployee.Entities;
using GestionEmployee.Infrastructure.DataBase;
using GestionEmployee.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GestionEmployee.Repositories.Implementations
{
    public class LeaveRequestStatusRepository : ILeaveRequestStatusRepository
    {
        private readonly GestionEmployeeContext _dbContext;
        public LeaveRequestStatusRepository(GestionEmployeeContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<LeaveRequestStatus>> GetLeaveRequestStatusesAsync()
        {
            return await _dbContext.LeaveRequestStatuses.ToListAsync();
        }

        public async Task<LeaveRequestStatus> GetLeaveRequestStatusByIdAsync(int leaveRequestStatusId)
        {
            return await _dbContext.LeaveRequestStatuses.FirstOrDefaultAsync(x => x.LeaveRequestStatusId == leaveRequestStatusId);
        }

        public async Task<LeaveRequestStatus> CreateLeaveRequestStatusAsync(LeaveRequestStatus leaveRequestStatus)
        {
            await _dbContext.LeaveRequestStatuses.AddAsync(leaveRequestStatus);
            await _dbContext.SaveChangesAsync();

            return leaveRequestStatus;
        }

        public async Task UpdateLeaveRequestStatusAsync(LeaveRequestStatus leaveRequestStatus)
        {
            _dbContext.LeaveRequestStatuses.Update(leaveRequestStatus);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<LeaveRequestStatus> DeleteLeaveRequestStatusAsync(int leaveRequestStatusId)
        {
            var leaveRequestStatusToDelete = await _dbContext.LeaveRequestStatuses.FindAsync(leaveRequestStatusId);
            _dbContext.LeaveRequestStatuses.Remove(leaveRequestStatusToDelete);
            await _dbContext.SaveChangesAsync();

            return leaveRequestStatusToDelete;
        }
    }
}

