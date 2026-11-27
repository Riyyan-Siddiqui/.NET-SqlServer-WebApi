using Microsoft.EntityFrameworkCore;
using WebAPI_tutorial.Data;
using WebAPI_tutorial.Dto;
using WebAPI_tutorial.IServices;

namespace WebAPI_tutorial.Services
{
    public class EmployeeService(AppDbContext _context) : IEmployeeService
    {
        public async Task<Tuple<int, List<EmployeeDto>>> GetAllEmployeeAsync()
        {
            try
            {
                return new Tuple<int, List<EmployeeDto>>(1, await _context.Employees.AsNoTracking().Select(x => new EmployeeDto // AsNoTracking() is used to improve performance by disabling change tracking for the retrieved entities. This is useful when you only need to read data and don't plan to modify it.
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    DOB = x.DOB,
                    Position = x.Position,
                    Department = x.Department,
                    CreatedAt = x.CreatedAt,
                    LastModifiedAt = x.LastModifiedAt
                }).ToListAsync());

            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<Tuple<int, string>> CreateEmployee(EmployeeDto employee)
        {
            try
            {
                var existing = await _context.Employees.AnyAsync(x => x.Email == employee.Email);
                if (existing)
                {
                    return new Tuple<int, string>(0, "Employee with this email already exists");
                }
                await _context.AddAsync(new Entities.Employee
                {
                    Id = Guid.NewGuid(),
                    Name = employee.Name,
                    Email = employee.Email,
                    DOB = employee.DOB,
                    Position = employee.Position,
                    Department = employee.Department,
                    CreatedAt = DateTime.Now,
                    LastModifiedAt = DateTime.Now
                });

                await _context.SaveChangesAsync();
                return new Tuple<int, string>(1, "Employee created successfully!");

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Tuple<int, string>> UpdateEmployee(EmployeeDto employee)
        {
            try
            {
                if (employee == null)
                {
                    return new Tuple<int, string>(-1, "Please fill all the details.");
                }
                var existing = await _context.Employees.FirstOrDefaultAsync(x => x.Email == employee.Email);
                if (existing == null)
                {
                    return new Tuple<int, string>(0, "Employee doesnot exist with this email address");
                }

                existing.Position = string.IsNullOrWhiteSpace(employee.Position) ? existing.Position : employee.Position;
                existing.DOB = employee.DOB ?? existing.DOB;
                existing.Name = string.IsNullOrWhiteSpace(employee.Name) ? existing.Name : employee.Name;
                existing.Email = string.IsNullOrWhiteSpace(employee.Email) ? existing.Email : employee.Email;
                existing.Department = string.IsNullOrWhiteSpace(employee.Department) ? existing.Department : employee.Department;
                existing.LastModifiedAt = DateTime.Now;

                _context.Employees.Update(existing);
                await _context.SaveChangesAsync();
                return new Tuple<int, string>(1, "Employee updated successfully!");

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Tuple<int, string>> DeleteEmployee(Guid Id)
        {
            try
            {
                var data = await _context.Employees.FirstOrDefaultAsync(x => x.Id == Id);

                if (data == null)
                {
                    return new Tuple<int, string>(0, "Employee with this ID does not exist");
                }

                _context.Employees.Remove(data);
                await _context.SaveChangesAsync();

                return new Tuple<int, string>(1, "Employee deleted successfully");


            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Tuple<int, EmployeeDto>> GetEmployeeByID(Guid Id)
        {
            try
            {
                var data = await _context.Employees.AsNoTracking().Select(x => new EmployeeDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    DOB = x.DOB,
                    Position = x.Position,
                    Department = x.Department,
                    CreatedAt = x.CreatedAt,
                    LastModifiedAt = x.LastModifiedAt
                }).FirstOrDefaultAsync(x => x.Id == Id);

                if(data == null)
                {
                    return new Tuple<int, EmployeeDto>(0, null);
                }
                return new Tuple<int, EmployeeDto>(1, data);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}


