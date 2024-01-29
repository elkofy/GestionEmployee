using GestionEmployee.Dtos.LeaveRequest;
using GestionEmployee.Entities;
using GestionEmployee.Repositories.Contracts;
using GestionEmployee.Services.Contracts;
using GestionEmployee.Utils;


namespace GestionEmployee.Services.Implementations
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILeaveRequestStatusRepository _leaveRequestStatusRepository;

        public LeaveRequestService(ILeaveRequestRepository leaveRequestRepository, IEmployeeRepository employeeRepository, ILeaveRequestStatusRepository leaveRequestStatusRepository)
        {
            _employeeRepository = employeeRepository;
            _leaveRequestRepository = leaveRequestRepository;
            _leaveRequestStatusRepository = leaveRequestStatusRepository;
        }

        public async Task<List<ReadLeaveRequest>> GetLeaveRequestsByEmployeeId(int employeeId)
        {
            var leaveRequestList = await _leaveRequestRepository.GetLeaveRequestsByEmployeeIdAsync(employeeId);
            // leaveRequestList peut être une liste vide
            return leaveRequestList.Select(leaveRequest => new ReadLeaveRequest
            {
                LeaveRequestId = leaveRequest.LeaveRequestId,
                EmployeeId = leaveRequest.EmployeeId,
                LeaveRequestStatusId = leaveRequest.LeaveRequestStatusId,
                RequestDate = leaveRequest.RequestDate,
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate
            }).ToList();
        }

        public async Task<ReadLeaveRequest> GetLeaveRequestById(int leaveRequestId)
        {
            var leaveRequest = await _leaveRequestRepository.GetLeaveRequestByIdAsync(leaveRequestId);

            if (leaveRequest == null)
            {
                throw new Exception($"La demande de congé  {leaveRequestId} n'existe pas");
            }

            return new ReadLeaveRequest
            {
                LeaveRequestId = leaveRequest.LeaveRequestId,
                EmployeeId = leaveRequest.EmployeeId,
                LeaveRequestStatusId = leaveRequest.LeaveRequestStatusId,
                RequestDate = leaveRequest.RequestDate,
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate
            };
        }

        public async Task<ReadLeaveRequest> CreateLeaveRequestAsync(CreateLeaveRequest leaveRequest)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(leaveRequest.EmployeeId);
            if (employee is null)
            {
                throw new Exception($"L'employé n'existe pas {leaveRequest.EmployeeId} n'existe pas");
            }

            Validator.isEndDateGtStartDate(leaveRequest.EndDate, leaveRequest.StartDate);
            Validator.isInsideWorkHours(leaveRequest.StartDate, (DateTime)leaveRequest.EndDate);

            var LeaveRequests = await _leaveRequestRepository.GetLeaveRequestsByEmployeeIdAsync(leaveRequest.EmployeeId);

            if (LeaveRequests.Any(lr =>
                (leaveRequest.StartDate >= lr.StartDate && leaveRequest.StartDate <= lr.EndDate) ||
                (leaveRequest.EndDate >= lr.StartDate && leaveRequest.EndDate <= lr.EndDate) ||
                (leaveRequest.StartDate <= lr.StartDate && leaveRequest.EndDate >= lr.EndDate)))
            {
                throw new Exception($"l'employé a déjà une demande de congé pour cette période.");
            }

            var leaveRequestEntity = new LeaveRequest
            {
                EmployeeId = leaveRequest.EmployeeId,
                LeaveRequestStatusId = 1,
                RequestDate = DateTime.Now,
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate
            };

            var leaveRequestCreated = await _leaveRequestRepository.CreateLeaveRequestAsync(leaveRequestEntity);

            return new ReadLeaveRequest
            {
                LeaveRequestId = leaveRequestCreated.LeaveRequestId,
                EmployeeId = leaveRequestCreated.EmployeeId,
                LeaveRequestStatusId = leaveRequestCreated.LeaveRequestStatusId,
                RequestDate = leaveRequestCreated.RequestDate,
                StartDate = leaveRequestCreated.StartDate,
                EndDate = leaveRequestCreated.EndDate
            };
        }

        public async Task UpdateLeaveRequestStatus(int leaveRequestId, UpdateLeaveRequest updateLeaveRequestStatus)
        {
            var leaveRequest = await _leaveRequestRepository.GetLeaveRequestByIdAsync(leaveRequestId);
            if (leaveRequest == null)
            {
                throw new Exception($"La demande de congé {leaveRequestId} n'existe pas");
            }

            var leaveRequestStatus = await _leaveRequestStatusRepository.GetLeaveRequestStatusByIdAsync(updateLeaveRequestStatus.LeaveRequestStatusId);
            if (leaveRequestStatus == null)
            {
                throw new Exception($"Le statut de congé {leaveRequestId} n'existe pas");
            }

            leaveRequest.LeaveRequestStatusId = updateLeaveRequestStatus.LeaveRequestStatusId;
            await _leaveRequestRepository.UpdateLeaveRequestAsync(leaveRequest);
        }

        public async Task DeleteLeaveRequestById(int leaveRequestId)
        {
            var leaveRequest = await _leaveRequestRepository.GetLeaveRequestByIdAsync(leaveRequestId);

            if (leaveRequest == null)
            {
                throw new Exception($"La demande de congé {leaveRequestId} n'existe pas");
             }

            await _leaveRequestRepository.DeleteLeaveRequestByIdAsync(leaveRequestId);
        }
    }
}

