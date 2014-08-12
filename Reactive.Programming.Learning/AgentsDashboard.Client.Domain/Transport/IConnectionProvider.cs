using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace AgentsDashboard.Client.Domain.Transport
{
    internal interface IConnectionProvider
    {
        IObservable<IConnection> GetActiveConnection();
    }

    internal class ConnectionProvider : IConnectionProvider, IDisposable
    {
        private readonly SingleAssignmentDisposable _disposable = new SingleAssignmentDisposable();
        private readonly string _username;
        private readonly IObservable<IConnection> _connectionSequence;
        private readonly string[] _servers;

        public ConnectionProvider(string username, string[] servers)
        {
            _username = username;
            _servers = servers;
            _connectionSequence = CreateConnectionSequence();
        }

        private IObservable<IConnection> CreateConnectionSequence()
        {
            return Observable.Create<IConnection>(o =>
            {
                _log.Info("Creating new connection...");
                var connection = GetNextConnection();

                var statusSubscription = connection.StatusStream.Subscribe(
                    _ => { },
                    ex => o.OnCompleted(),
                    () =>
                    {
                        _log.Info("Status subscription completed");
                        o.OnCompleted();
                    });

                var connectionSubscription =
                    connection.Initialize().Subscribe(
                        _ => o.OnNext(connection),
                        ex => o.OnCompleted(),
                        o.OnCompleted);

                return new CompositeDisposable { statusSubscription, connectionSubscription };
            })
                .Repeat()
                .Replay(1)
                .LazilyConnect(_disposable);
        }

        public IObservable<IConnection> GetActiveConnection()
        {
            return _connectionSequence;
        }

        private IConnection GetNextConnection()
        {
            var connection = new Connection(_servers[_currentIndex++], _username);
            if (_currentIndex == _servers.Length)
            {
                _currentIndex = 0;
            }
            return connection;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}