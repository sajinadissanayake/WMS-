namespace WorkforceShiftPortal.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // Admin / Manager / Employee

        // Department assignment
        public int? DepartmentId { get; set; }
        public virtual Department? Department { get; set; }

        // Navigation: Employee's shifts
        public virtual ICollection<Shift>? Shifts { get; set; }
    }
}
