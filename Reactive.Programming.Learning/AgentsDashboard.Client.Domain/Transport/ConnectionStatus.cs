﻿namespace AgentsDashboard.Client.Domain.Transport
{
    public enum ConnectionStatus
    {
        Connecting,
        Connected,
        ConnectionSlow,
        Reconnecting,
        Reconnected,
        Closed,
        Uninitialized
    }
}