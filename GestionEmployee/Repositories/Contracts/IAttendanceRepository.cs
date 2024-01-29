using GestionEmployee.Entities;

namespace GestionEmployee.Repositories.Contracts
{
    public interface IAttendanceRepository
    {
        Task<List<Attendance>> GetAttendanceByEmployeeIdAsync(int employeeId);
        Task<Attendance> GetAttendanceByIdAsync(int attendanceId);
        Task CreateAttendanceAsync(Attendance attendance);
        Task<Attendance> DeleteAttendanceByIdAsync(int attendanceId);
    }
}
