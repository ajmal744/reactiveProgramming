using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using AgentsDashboard.Shared.Extensions;
using Microsoft.AspNet.SignalR.Client;

namespace AgentsDashboard.Client.Domain.Transport
{
    internal class Connection : IConnection
    {
        private readonly ISubject<ConnectionInfo> _statusStream;
        private HubConnection _hubConnection;
        private bool _initialized;

        public Connection(string address, string username)
        {
            Address = address;
            _statusStream = new BehaviorSubject<ConnectionInfo>(new ConnectionInfo(ConnectionStatus.Uninitialized, address));
            _hubConnection = new HubConnection(address);

            _hubConnection.Headers.Add("UserName", username);
            CreateStatus().Subscribe(
                s => _statusStream.OnNext(new ConnectionInfo(s, address)),
                _statusStream.OnError,
                _statusStream.OnCompleted);
            AgentScanHealthProxy = _hubConnection.CreateHubProxy("AgentScanHealthHub");

        }

        private IObservable<ConnectionStatus> CreateStatus()
        {
            var closed = Observable.FromEvent(h => _hubConnection.Closed += h, h => _hubConnection.Closed -= h).Select(_ => ConnectionStatus.Closed);
            var connectionSlow = Observable.FromEvent(h => _hubConnection.ConnectionSlow += h, h => _hubConnection.ConnectionSlow -= h).Select(_ => ConnectionStatus.ConnectionSlow);
            var reconnected = Observable.FromEvent(h => _hubConnection.Reconnected += h, h => _hubConnection.Reconnected -= h).Select(_ => ConnectionStatus.Reconnected);
            var reconnecting = Observable.FromEvent(h => _hubConnection.Reconnecting += h, h => _hubConnection.Reconnecting -= h).Select(_ => ConnectionStatus.Reconnecting);
            return Observable.Merge(closed, connectionSlow, reconnected, reconnecting)
                .TakeUntilInclusive(status => status == ConnectionStatus.Closed); // complete when the connection is closed (it's terminal, SignalR will not attempt to reconnect anymore)
        }

        public IObservable<Unit> Initialize()
        {
            if (_initialized)
            {
                throw new InvalidOperationException("Connection has already been initialized");
            }
            _initialized = true;

            return Observable.Create<Unit>(async observer =>
            {
                _statusStream.OnNext(new ConnectionInfo(ConnectionStatus.Connecting, Address));

                try
                {
                    await _hubConnection.Start();
                    _statusStream.OnNext(new ConnectionInfo(ConnectionStatus.Connected, Address));
                    observer.OnNext(Unit.Default);
                }
                catch (Exception e)
                {
                    observer.OnError(e);
                }

                return Disposable.Create(() =>
                {
                    try
                    {
                        _hubConnection.Stop();
                    }
                    catch (Exception e)
                    {
                        // we must never throw in a disposable
                    }
                });
            })
                .Publish()
                .RefCount();
        }


        public IHubProxy AgentScanHealthProxy { get; private set; }

        public IObservable<ConnectionInfo> StatusStream { get; private set; }

        public string Address { get; private set; }
    }
}