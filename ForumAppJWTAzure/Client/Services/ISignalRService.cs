using Microsoft.AspNetCore.SignalR.Client;

namespace ForumAppJWTAzure.Client.Services
{
    public interface ISignalRService
    {
        event EventHandler<string>? AddPostViewModel;

        event EventHandler<string>? EditPostViewModel;

        event EventHandler<string>? AddPostVoteViewModel;

        HubConnection? HubConnection { get; set; }

        Task StartConnection();

        Task RestartConnection(Exception? exception);
    }
}
