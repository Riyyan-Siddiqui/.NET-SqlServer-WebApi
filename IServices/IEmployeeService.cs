using WebAPI_tutorial.Dto;

namespace WebAPI_tutorial.IServices
{
    public interface IEmployeeService
    {
        Task<Tuple<int, List<EmployeeDto>>> GetAllEmployeeAsync();
        Task<Tuple<int, string>> CreateEmployee(EmployeeDto employee);

        Task<Tuple<int, string>> UpdateEmployee(EmployeeDto employee);

        Task<Tuple<int, string>> DeleteEmployee(Guid Id);

        Task<Tuple<int, EmployeeDto>> GetEmployeeByID(Guid Id);

    }
}
