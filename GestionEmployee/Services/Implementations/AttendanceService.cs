using GestionEmployee.Dtos.Attendance;
using GestionEmployee.Entities;
using GestionEmployee.Repositories.Contracts;
using GestionEmployee.Services.Contracts;
using GestionEmployee.Utils;

namespace GestionEmployee.Services.Implementations
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public AttendanceService(IAttendanceRepository attendanceRepository, IEmployeeRepository employeeRepository)
        {
            _attendanceRepository = attendanceRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<List<ReadAttendance>> GetAttendanceByEmployeeId(int employeeId)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeId);

            if (employee is null)
                throw new Exception($" l'employé {employeeId} n'existe pas ");

            var attendanceList = await _attendanceRepository.GetAttendanceByEmployeeIdAsync(employeeId);

            return attendanceList.Select(attendance => new ReadAttendance
            {
                AttendanceId = attendance.AttendanceId,
                EmployeeId = attendance.EmployeeId,
                StartDate = attendance.StartDate,
                EndDate = attendance.EndDate
            }).ToList();
        }

        public async Task<ReadAttendance> GetAttendanceById(int attendanceId)
        {
            var attendance = await _attendanceRepository.GetAttendanceByIdAsync(attendanceId);

            if (attendance == null)
            {
                throw new Exception($"Echec de récupération des informations de présence car elle n'existe pas : {attendanceId}");
            }

            return new ReadAttendance
            {
                AttendanceId = attendance.AttendanceId,
                EmployeeId = attendance.EmployeeId,
                StartDate = attendance.StartDate,
                EndDate = attendance.EndDate
            };
        }

  
        public async Task<ReadAttendance> CreateAttendanceAsync(CreateAttendance attendance)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(attendance.EmployeeId);

            if (employee is null)
            {
                throw new Exception($" l'employé {attendance.EmployeeId} n'existe pas");
            }

            Validator.isEndDateGtStartDate(attendance.EndDate,attendance.StartDate);
            Validator.isInsideWorkHours(attendance.EndDate, attendance.StartDate);

            var AttendanceList = await _attendanceRepository.GetAttendanceByEmployeeIdAsync(attendance.EmployeeId);

            if (AttendanceList.Any(at =>
                (attendance.StartDate >= at.StartDate && attendance.StartDate <= at.EndDate) ||
                (attendance.EndDate >= at.StartDate && attendance.EndDate <= at.EndDate) ||
                (attendance.StartDate <= at.StartDate && attendance.EndDate >= at.EndDate)))
            {
                throw new Exception($"Cette entrée existe déjà");
            }

            var attendanceToCreate = new Attendance
            {
                EmployeeId = attendance.EmployeeId,
                StartDate = attendance.StartDate,
                EndDate = attendance.EndDate
            };

            await _attendanceRepository.CreateAttendanceAsync(attendanceToCreate);

            return new ReadAttendance
            {
                EmployeeId = attendanceToCreate.EmployeeId,
                StartDate = attendanceToCreate.StartDate,
                EndDate = attendanceToCreate.EndDate
            };
        }

        
        public async Task DeleteAttendanceById(int attendanceId)
        {
            var attendance = await _attendanceRepository.GetAttendanceByIdAsync(attendanceId);

            if (attendance == null)
            {
                throw new Exception($"Echec de suppression de la demande de présence, elle n'existe pas : {attendanceId}");
            }

            await _attendanceRepository.DeleteAttendanceByIdAsync(attendanceId);
        }
    }
}
