namespace GestionEmployee.Entities
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string Address {  get; set; }

        public virtual ICollection<EmployeeDepartment> EmployeesDepartments { get; set; } = new List<EmployeeDepartment>();

    }
}
