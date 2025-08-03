using Wholesaler.Core.Dto.RequestModels;
using Wholesaler.Core.Dto.ResponseModels;
using Wholesaler.Frontend.DataAccess.Http;
using Wholesaler.Frontend.Domain.Interfaces;
using Wholesaler.Frontend.Domain.ValueObjects;

namespace Wholesaler.Frontend.DataAccess;

public class WholesalerClient : RequestService,
    IUserService,
    IClientRepository,
    IWorkDayRepository,
    IWorkTaskRepository,
    IUserRepository,
    IRequirementRepository,
    IStorageRepository,
    IDeliveryRepository
{
    private const string ApiPath = "http://localhost:5050";

    public Task<ExecutionResultGeneric<UserDto>> TryLoginWithDataFromUserAsync(string loginFromUser, string passwordFromUser)
    {
        var request = new Request<LoginUserRequestModel, UserDto>()
        {
            Path = $"{ApiPath}/users/actions/login",
            Method = HttpMethod.Post,
            Content = new()
            {
                Login = loginFromUser,
                Password = passwordFromUser
            }
        };

        return SendAsync(request);
    }

    public Task<ExecutionResultGeneric<WorkdayDto>> StartWorkingAsync(Guid userId)
    {
        var request = new Request<StartWorkdayRequestModel, WorkdayDto>()
        {
            Path = $"{ApiPath}/workdays/actions/start",
            Method = HttpMethod.Post,
            Content = new()
            {
                UserId = userId
            }
        };

        return SendAsync(request);
    }

    public Task<ExecutionResultGeneric<WorkdayDto>> GetWorkdayAsync(Guid workdayid)
    {
        var request = new Request<HttpRequestMessage, WorkdayDto>()
        {
            Path = $"{ApiPath}/workdays/{workdayid}",
            Method = HttpMethod.Get
        };

        return SendAsync(request);
    }

    public Task<ExecutionResultGeneric<WorkdayDto>> FinishWorkingAsync(Guid userId)
    {
        var request = new Request<FinishWorkdayRequestModel, WorkdayDto>()
        {
            Path = $"{ApiPath}/workdays/actions/finish",
            Method = HttpMethod.Post,
            Content = new()
            {
                UserId = userId
            }
        };

        return SendAsync(request);
    }

    public Task<ExecutionResultGeneric<WorkTaskDto>> AssignTaskAsync(Guid workTaskId, Guid userId)
    {
        var request = new Request<AssignTaskRequestModel, WorkTaskDto>()
        {
            Path = $"{ApiPath}/worktasks/{workTaskId}/actions/assign",
            Method = HttpMethod.Post,
            Content = new()
            {
                UserId = userId
            }
        };

        return SendAsync(request);
    }

    public Task<ExecutionResultGeneric<List<WorkTaskDto>>> GetNotAssignWorkTasksAsync()
    {
        var request = new Request<HttpRequestMessage, List<WorkTaskDto>>()
        {
            Path = $"{ApiPath}/worktasks/unassigned",
            Method = HttpMethod.Get
        };

        return SendAsync(request);
    }

    public Task<ExecutionResultGeneric<List<WorkTaskDto>>> GetAssignedTaskAsync()
    {
        var request = new Request<HttpRequestMessage, List<WorkTaskDto>>()
        {
            Path = $"{ApiPath}/worktasks/assigned",
            Method = HttpMethod.Get
        };

        return SendAsync(request);
    }

    public Task<ExecutionResultGeneric<List<UserDto>>> GetEmployeesAsync()
    {
        var request = new Request<HttpRequestMessage, List<UserDto>>()
        {
            Path = $"{ApiPath}/employees",
            Method = HttpMethod.Get
        };

        return SendAsync(request);
    }

    public Task<ExecutionResultGeneric<List<WorkTaskDto>>> GetAssignedTaskToAnEmployeeAsync(Guid userId)
    {
        var request = new Request<HttpRequestMessage, List<WorkTaskDto>>()
        {
            Path = $"{ApiPath}/worktasks/assignedToAnEmployee?userId={userId}",
            Method = HttpMethod.Get
        };

        return SendAsync(request);
    }

    public Task<ExecutionResultGeneric<WorkTaskDto>> ChangeOwnerAsync(Guid workTaskId, Guid newOwnerId)
    {
        var request = new Request<ChangeOwnerRequestModel, WorkTaskDto>()
        {
            Path = $"{ApiPath}/worktasks/{workTaskId}/actions/changeOwner",
            Method = HttpMethod.Patch,
            Content = new()
            {
                NewOwnerId = newOwnerId
            }
        };

        return SendAsync(request);
    }

    public Task<ExecutionResultGeneric<WorkTaskDto>> StartWorkTaskAsync(Guid workTaskId)
    {
        var request = new Request<Guid, WorkTaskDto>()
        {
            Path = $"{ApiPath}/worktasks/{workTaskId}/actions/start",
            Method = HttpMethod.Post
        };

        return SendAsync(request);
    }

    public Task<ExecutionResultGeneric<WorkTaskDto>> StopWorkTaskAsync(Guid workTaskId)
    {
        var request = new Request<Guid, WorkTaskDto>()
        {
            Path = $"{ApiPath}/worktasks/{workTaskId}/actions/stop",
            Method = HttpMethod.Post
        };

        return SendAsync(request);
    }

    public Task<ExecutionResultGeneric<WorkTaskDto>> FinishWorkTaskAsync(Guid workTaskId)
    {
        var request = new Request<Guid, WorkTaskDto>()
        {
            Path = $"{ApiPath}/worktasks/{workTaskId}/actions/finish",
            Method = HttpMethod.Post
        };

        return SendAsync(request);
    }

    public Task<ExecutionResultGeneric<List<WorkTaskDto>>> GetStartedWorkTasksAsync()
    {
        var request = new Request<HttpRequestMessage, List<WorkTaskDto>>()
        {
            Path = $"{ApiPath}/worktasks/started",
            Method = HttpMethod.Get
        };

        return SendAsync(request);
    }

    public Task<ExecutionResultGeneric<List<WorkTaskDto>>> GetFinishedWorkTasksAsync()
    {
        var request = new Request<HttpRequestMessage, List<WorkTaskDto>>()
        {
            Path = $"{ApiPath}/worktasks/finished",
            Method = HttpMethod.Get
        };

        return SendAsync(request);
    }

    public Task<ExecutionResultGeneric<RequirementDto>> AddAsync(int quantity, Guid clientId, Guid storageId)
    {
        var request = new Request<AddRequirementRequestModel, RequirementDto>()
        {
            Path = $"{ApiPath}/requirements",
            Method = HttpMethod.Post,
            Content = new()
            {
                ClientId = clientId,
                Quantity = quantity,
                StorageId = storageId
            }
        };

        return SendAsync(request);
    }

    public Task<ExecutionResultGeneric<RequirementDto>> EditQuantityAsync(Guid id, int quantity)
    {
        var request = new Request<UdpateRequirementRequestModel, RequirementDto>()
        {
            Path = $"{ApiPath}/requirements/{id}",
            Method = HttpMethod.Patch,
            Content = new()
            {
                Quantity = quantity
            }
        };

        return SendAsync(request);
    }

    public Task<ExecutionResultGeneric<List<RequirementDto>>> GetAllRequirementsAsync()
    {
        var request = new Request<HttpRequestMessage, List<RequirementDto>>()
        {
            Path = $"{ApiPath}/requirements",
            Method = HttpMethod.Get
        };

        return SendAsync(request);
    }

    public Task<ExecutionResultGeneric<List<RequirementDto>>> GetRequirementsAsync(Guid storageId)
    {
        var request = new Request<HttpRequestMessage, List<RequirementDto>>()
        {
            Path = $"{ApiPath}/requirements/withStorageId?storageId={storageId}",
            Method = HttpMethod.Get
        };

        return SendAsync(request);
    }

    public Task<ExecutionResultGeneric<List<RequirementDto>>> GetRequirementsByStatusAsync(string status)
    {
        var request = new Request<HttpRequestMessage, List<RequirementDto>>()
        {
            Path = $"{ApiPath}/requirements/byStatus?status={status}",
            Method = HttpMethod.Get
        };

        return SendAsync(request);
    }

    public Task<ExecutionResultGeneric<RequirementDto>> CompleteRequirementAsync(Guid id)
    {
        var request = new Request<Guid, RequirementDto>()
        {
            Path = $"{ApiPath}/requirements/{id}/actions/complete",
            Method = HttpMethod.Patch
        };

        return SendAsync(request);
    }

    public Task<ExecutionResultGeneric<List<ClientDto>>> GetAllClientsAsync()
    {
        var request = new Request<HttpRequestMessage, List<ClientDto>>()
        {
            Path = $"{ApiPath}/clients",
            Method = HttpMethod.Get
        };

        return SendAsync(request);
    }

    public Task<ExecutionResultGeneric<StorageDto>> AddAsync(string name)
    {
        var request = new Request<AddStorageRequestModel, StorageDto>()
        {
            Path = $"{ApiPath}/storages",
            Method = HttpMethod.Post,
            Content = new()
            {
                Name = name
            }
        };

        return SendAsync(request);
    }

    public Task<ExecutionResultGeneric<List<StorageDto>>> GetAllStoragesAsync()
    {
        var request = new Request<HttpRequestMessage, List<StorageDto>>()
        {
            Path = $"{ApiPath}/storages",
            Method = HttpMethod.Get
        };

        return SendAsync(request);
    }

    public Task<ExecutionResultGeneric<StorageDto>> DeliverAsync(Guid id, int quantity, Guid personId)
    {
        var request = new Request<UpdateStorageRequestModel, StorageDto>()
        {
            Path = $"{ApiPath}/storages/{id}/actions/deliver",
            Method = HttpMethod.Patch,
            Content = new()
            {
                Quantity = quantity,
                PersonId = personId
            }
        };

        return SendAsync(request);
    }

    public Task<ExecutionResultGeneric<float>> GetCostsAsync(long from, long to)
    {
        var request = new Request<HttpRequestMessage, float>()
        {
            Path = $"{ApiPath}/raports/costs?from={from}&to={to}",
            Method = HttpMethod.Get
        };

        return SendAsync(request);
    }
}
