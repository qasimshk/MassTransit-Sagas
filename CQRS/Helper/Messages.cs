using CQRS.Business;
using CQRS.Business.Utils;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace CQRS.Helper
{
    public sealed class Messages
    {
        private readonly IServiceScopeFactory _provider;

        public Messages(IServiceScopeFactory provider)
        {
            _provider = provider;
        }

        public async Task<Results> DispatchAsync(ICommand command) // command
        {
            using (var scope = _provider.CreateScope())
            {
                var srv = scope.ServiceProvider;
                Type type = typeof(ICommandHandler<>);
                Type[] typeArgs = { command.GetType() };
                Type handlerType = type.MakeGenericType(typeArgs);
                dynamic handler = srv.GetService(handlerType);
                Results result = await handler.Handle((dynamic)command);
                return result;
            }
        }

        public async Task<Results> DispatchAsync<T>(
            JsonPatchDocument<T> command, dynamic Id) where T : class // command
        {
            using (var scope = _provider.CreateScope())
            {
                var srv = scope.ServiceProvider;
                Type type = typeof(ICommandPatchHandler<>);
                Type[] typeArgs = { command.GetType() };
                Type handlerType = type.MakeGenericType(typeArgs);
                dynamic handler = srv.GetService(handlerType);
                Results result = await handler.Handle(command, Id);
                return result;
            }
        }

        public async Task<T> DispatchAsync<T>(IQuery<T> query) // query
        {
            using (var scope = _provider.CreateScope())
            {
                var srv = scope.ServiceProvider;
                Type type = typeof(IQueryHandler<,>);
                Type[] typeArgs = { query.GetType(), typeof(T) };
                Type handlerType = type.MakeGenericType(typeArgs);
                dynamic handler = srv.GetService(handlerType);
                T result = await handler.Handle((dynamic)query);
                return result;
            }
        }
    }
}