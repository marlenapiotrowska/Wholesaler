using Wholesaler.Core.Dto.ResponseModels;
using Wholesaler.Frontend.Domain.ValueObjects;

namespace Wholesaler.Frontend.Domain.Interfaces;

public interface IWorkDayRepository
{
    Task<ExecutionResultGeneric<WorkdayDto>> GetWorkdayAsync(Guid workdayid);
}
