using Grpc.Core;
using GrpcService1;

namespace GrpcService1.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    private readonly User[] _users;

    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
        _users =  new User[]
        {
            new User { Id = "1", FirstName = "John", LastName = "Doe" },
            new User { Id = "2", FirstName = "Jane", LastName = "Doe" }
        };
    }

    public override Task<HelloResponse> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloResponse
        {
            Message = "From server: " + request.Name
        });
    }

    public override Task<GetUsersResponse> GetUsers(GetUsersRequest request, ServerCallContext context)
    {
        return Task.FromResult(new GetUsersResponse
        {
            Users = { _users }
        });
    }

    public override Task<GetUserByIdResponse> GetUserById(GetUserByIdRequest request, ServerCallContext context)
    {
        var user = _users.FirstOrDefault(u => u.Id == request.UserId);

        if (user == null)
        {
            var status = new Status(StatusCode.NotFound, $"User with ID '{request.UserId}' not found.");
            
            throw new RpcException(status);
        }
        
        return Task.FromResult(new GetUserByIdResponse
        {
            User = user
        });
    }
}