namespace CompanyManager.Api.Dtos
{
    public class CreateEmployeeDto
    {
        public string Name { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
    }
}
