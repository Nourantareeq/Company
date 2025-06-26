using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CompanyManager.Api.Data;
using CompanyManager.Api.Models;
using CompanyManager.Api.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace CompanyManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees()
        {
            var employees = await _context.Employees
                .Include(e => e.Department)
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Position = e.Position,
                    DepartmentId = e.DepartmentId,
                    DepartmentName = e.Department.Name
                })
                .ToListAsync();

            return Ok(employees);
        }

        // GET: api/employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
                return NotFound();

            var dto = new EmployeeDto
            {
                Id = employee.Id,
                Name = employee.Name,
                Position = employee.Position,
                DepartmentId = employee.DepartmentId,
                DepartmentName = employee.Department?.Name ?? ""
            };

            return Ok(dto);
        }

        // POST: api/employees
        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> PostEmployee(CreateEmployeeDto dto)
        {
            var employee = new Employee
            {
                Name = dto.Name,
                Position = dto.Position,
                DepartmentId = dto.DepartmentId
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            var department = await _context.Departments.FindAsync(dto.DepartmentId);

            var dtoResponse = new EmployeeDto
            {
                Id = employee.Id,
                Name = employee.Name,
                Position = employee.Position,
                DepartmentId = employee.DepartmentId,
                DepartmentName = department?.Name ?? ""
            };

            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, dtoResponse);
        }

        // PUT: api/employees/5
        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeDto>> PutEmployee(int id, Employee employee)
        {
            if (id != employee.Id)
                return BadRequest();

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Employees.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            var updated = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (updated == null)
                return NotFound();

            var dto = new EmployeeDto
            {
                Id = updated.Id,
                Name = updated.Name,
                Position = updated.Position,
                DepartmentId = updated.DepartmentId,
                DepartmentName = updated.Department?.Name ?? ""
            };

            return Ok(dto);
        }

        // DELETE: api/employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return NotFound();

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
