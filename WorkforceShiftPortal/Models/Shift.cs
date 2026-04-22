namespace WorkforceShiftPortal.Models
{
    public class Shift
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public virtual User? Employee { get; set; }

        public int DepartmentId { get; set; }
        public virtual Department? Department { get; set; }

        public string ShiftType { get; set; } = string.Empty;

        // Daily shift assignment
        public DateTime Date { get; set; }
    }

}
