using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace ForumAppJWTAzure.Client.Services
{
    public class SignalRService : ISignalRService
    {
        private readonly NavigationManager navigationManager;
        private readonly IConfiguration configuration;

        public SignalRService(NavigationManager navigationManager, IConfiguration configuration)
        {
            this.navigationManager = navigationManager;
            this.configuration = configuration;
        }

        public event EventHandler<string>? AddPostViewModel;

        public event EventHandler<string>? AddPostVoteViewModel;

        public HubConnection? HubConnection { get; set; }

        public async Task StartConnection()
        {
            this.HubConnection = new HubConnectionBuilder()
         .WithUrl(this.navigationManager.ToAbsoluteUri($"{this.configuration["ApiUrl"]}/{this.configuration["SignalRHubName"]}"))
         .WithAutomaticReconnect()
         .Build();

            await this.HubConnection.StartAsync();

            this.HubConnection.Closed += this.RestartConnection;

            this.RegisterServices();
        }

        public async Task RestartConnection(Exception? exception)
        {
            if (this.HubConnection == null || this.HubConnection.State != HubConnectionState.Connected)
            {
                await this.StartConnection();
            }
        }

        public void RegisterServices()
        {
            this.HubConnection?.On<string>("AddPostViewModel", (postViewModel) =>
            {
                if (postViewModel != null && this.AddPostViewModel != null)
                {
                    this.AddPostViewModel.Invoke(this, postViewModel);
                }
            });

            this.HubConnection?.On<string>("AddPostVoteViewModel", (postVoteViewModel) =>
            {
                Console.WriteLine("AddPostVoteViewModel");
                if (postVoteViewModel != null && this.AddPostVoteViewModel != null)
                {
                    this.AddPostVoteViewModel.Invoke(this, postVoteViewModel);
                }
            });
        }
    }
}
