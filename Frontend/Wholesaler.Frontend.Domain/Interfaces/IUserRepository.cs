using Wholesaler.Core.Dto.ResponseModels;
using Wholesaler.Frontend.Domain.ValueObjects;

namespace Wholesaler.Frontend.Domain.Interfaces;

public interface IUserRepository
{
    Task<ExecutionResultGeneric<List<UserDto>>> GetEmployeesAsync();
}
