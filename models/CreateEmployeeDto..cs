namespace CompanyManager.Api.Dtos
{
    public class CreateEmployeeDto
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public int DepartmentId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfJoining { get; set; }
    }

}
