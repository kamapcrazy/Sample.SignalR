using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Sample.SignalR.Client.Implementations
{
    public class HubProxy<TServer, TClient> : IDisposable
    {
        private readonly IList<IDisposable> _disposables;
        private bool _disposed;

        private readonly HubConnection _hubConnection;

        public HubProxy(HubConnection hubConnection)
        {
            _disposables = new List<IDisposable>();
            _hubConnection = hubConnection;
        }

        public async Task<T> CallAsync<T>(Expression<Func<TServer, T>> func)
        {
            return await _hubConnection.InvokeAsync<T>(GetMethodCallName(func));
        }

        public IDisposable SubscribeOn<T1>(
            Expression<Func<TClient, Func<T1, Task>>> eventToBind,
            Action<T1> callback)
        {
            var disposable = _hubConnection.On(GetMethodCallNameFromUnaryExp(eventToBind), callback);
            _disposables.Add(disposable);
            return disposable;
        }

        public IDisposable SubscribeOn<T1, T2>(
            Expression<Func<TClient, Func<T1, T2, Task>>> eventToBind,
            Action<T1, T2> callback)
        {
            var disposable = _hubConnection.On(GetMethodCallNameFromUnaryExp(eventToBind), callback);
            _disposables.Add(disposable);
            return disposable;
        }

        public IDisposable SubscribeOn<T1, T2, T3, T4>(
            Expression<Func<TClient, Func<T1, T2, T3, T4, Task>>> eventToBind,
            Action<T1, T2, T3, T4> callback)
        {
            var disposable = _hubConnection.On(GetMethodCallNameFromUnaryExp(eventToBind), callback);
            _disposables.Add(disposable);
            return disposable;
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
                _disposables.Remove(disposable);
            }

            _disposed = true;
        }

        private static string GetMethodCallName(LambdaExpression expression)
        {
            if (expression.Body is MethodCallExpression methodCallExpression)
            {
                return methodCallExpression.Method.Name;
            }

            throw new NotSupportedException("Unsupported func!");
        }

        private static string GetMethodCallNameFromUnaryExp(LambdaExpression lambdaExpression)
        {
            var unaryExpression = (UnaryExpression)lambdaExpression.Body;
            var methodCallExpression = (MethodCallExpression)unaryExpression.Operand;

            if (methodCallExpression.Object == null)
            {
                throw new Exception("Can't get method info!");
            }

            var methodInfo = (MethodInfo)((ConstantExpression)methodCallExpression.Object).Value;

            return methodInfo.Name;
        }
    }
}
