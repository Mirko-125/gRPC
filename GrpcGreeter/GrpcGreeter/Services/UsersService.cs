using GrpcGreeter;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;

namespace GrpcGreeter.Services
{
    public class UsersService : Users.UsersBase
    {
        private readonly ILogger<UsersService> _logger;
        public UsersService(ILogger<UsersService> logger)
        {
            _logger = logger;
        }
        public override Task<Message> AddUser(User request, ServerCallContext context)
        {
            if (AlternativeBase.Instance().ContainsKey(request.Id))
            {
                return Task.FromResult(new Message { Text = "User already exists." });
            }
            AlternativeBase.Instance().Add(request.Id, new UserData(request.Name, request.LastName, request.Address, request.CellphoneNumber[1]));
            return Task.FromResult(new Message { Text = $"Added {request.Name} {request.LastName} - Id:{request.Id}." });
        }
        public override async Task AddUsers(IAsyncStreamReader<User> requestStream, IServerStreamWriter<Message> responseStream, ServerCallContext context)
        {
            await foreach (var request in requestStream.ReadAllAsync())
            {
                if (AlternativeBase.Instance().ContainsKey(request.Id)) continue;
                AlternativeBase.Instance().Add(request.Id, new UserData(request.Name, request.LastName, request.Address, request.CellphoneNumber[1]));

                await responseStream.WriteAsync(new Message { Text = $"Added {request.Name} {request.LastName} - Id:{request.Id}." });
            }
        }
        public override Task<Empty> RemoveUser(Id request, ServerCallContext context)
        {
            if (!AlternativeBase.Instance().ContainsKey(request.Id_))
            {
                return Task.FromResult(new Empty());
            }

            AlternativeBase.Instance().Remove(request.Id_);

            return Task.FromResult(new Empty());
        }
        public override async Task<Empty> RemoveUsers(IAsyncStreamReader<Id> requestStream, ServerCallContext context)
        {
            await foreach (var identifier in requestStream.ReadAllAsync())
            {
                if (!AlternativeBase.Instance().ContainsKey(identifier.Id_)) continue;

                AlternativeBase.Instance().Remove(identifier.Id_);
            }

            return await Task.FromResult(new Empty());
        }
        
    }
}
