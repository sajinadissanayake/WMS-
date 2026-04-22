namespace WorkforceShiftPortal.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? ManagerId { get; set; }

        // Navigation properties
        public virtual User? Manager { get; set; }
        public virtual ICollection<User>? Employees { get; set; }
    }
}
