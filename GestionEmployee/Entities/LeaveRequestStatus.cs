namespace GestionEmployee.Entities
{
    public class LeaveRequestStatus
    {
        public int LeaveRequestStatusId { get; set; }

        public string Status { get; set; } = null!;

        public virtual ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
    }
}
