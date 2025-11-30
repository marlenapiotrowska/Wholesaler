using Microsoft.AspNetCore.Mvc;
using Wholesaler.Backend.Api.Factories.Interfaces;
using Wholesaler.Backend.Domain.Interfaces;
using Wholesaler.Backend.Domain.Repositories;
using Wholesaler.Core.Dto.RequestModels;
using Wholesaler.Core.Dto.ResponseModels;

namespace Wholesaler.Backend.Api.Controllers;

[ApiController]
[Route("workdays")]
public class WorkdayController : ControllerBase
{
    private readonly IUserService _service;
    private readonly IWorkdayRepository _workdayRepository;
    private readonly IWorkdayDtoFactory _workdayDtoFactory;

    public WorkdayController(IUserService service, IWorkdayRepository workdayRepository, IWorkdayDtoFactory workdayDtoFactory)
    {
        _service = service;
        _workdayRepository = workdayRepository;
        _workdayDtoFactory = workdayDtoFactory;
    }

    [HttpPost]
    [Route("actions/start")]
    public async Task<ActionResult<WorkdayDto>> StartWorkdayAsync([FromBody] StartWorkdayRequestModel request)
    {
        var workday = _service.StartWorkday(request.UserId);
        var workdayDto = _workdayDtoFactory.Create(workday);

        return Ok(workdayDto);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<WorkdayDto>> GetWorkdayAsync(Guid id)
    {
        var workday = _workdayRepository.Get(id);
        var workdayDto = _workdayDtoFactory.Create(workday);

        return Ok(workdayDto);
    }

    [HttpPost]
    [Route("actions/finish")]
    public async Task<ActionResult<WorkdayDto>> FinishWorkdayAsync([FromBody] FinishWorkdayRequestModel request)
    {
        var workday = _service.FinishWorkday(request.UserId);
        var workdayDto = _workdayDtoFactory.Create(workday);

        return Ok(workdayDto);
    }
}
