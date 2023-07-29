using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace ForumAppJWTAzure.Client.Services
{
    public class SignalRService : ISignalRService
    {
        private readonly NavigationManager navigationManager;
        private readonly IConfigurationService Configuration;
        private readonly IAppLogService appLogService;

        public SignalRService(NavigationManager navigationManager, IConfigurationService Configuration, IAppLogService appLogService)
        {
            this.navigationManager = navigationManager;
            this.Configuration = Configuration;
            this.appLogService = appLogService;            
        }

        public event EventHandler<string>? AddPostViewModel;

        public event EventHandler<string>? AddPostVoteViewModel;

        public HubConnection? HubConnection { get; set; }

        public async Task StartConnection()
        {
            this.HubConnection = new HubConnectionBuilder()
         .WithUrl(this.navigationManager.ToAbsoluteUri($"{Configuration.Configuration["ApiUrl"]}/{Configuration.Configuration["SignalRHubName"]}"))
         .WithAutomaticReconnect()
         .Build();

            AppLogViewModel appLog = new()
            {
                FileName = "SignalRService",
                Method = "StartConnection",
                Project = Lookups.Project.Client,
                Message = $"Starting SignalR Service",
                Severity = Lookups.Severity.Info,
            };

            var logResponse = await appLogService.Create<AppLogViewModel>(appLog, ApiEndpoints.AppLog);

            await this.HubConnection.StartAsync();

            this.HubConnection.Closed += this.RestartConnection;

            await this.RegisterServices();
        }

        public async Task RestartConnection(Exception? exception)
        {
            if (this.HubConnection == null || this.HubConnection.State != HubConnectionState.Connected)
            {
                AppLogViewModel appLog = new()
                {
                    FileName = "SignalRService",
                    Method = "RestartConnection",
                    Project = Lookups.Project.Client,
                    Message = $"Restarting SignalR Service",
                    Severity = Lookups.Severity.Info,
                };

                var logResponse = await appLogService.Create<AppLogViewModel>(appLog, ApiEndpoints.AppLog);

                await this.StartConnection();
            }
        }

        public async Task RegisterServices()
        {
            AppLogViewModel appLog = new()
            {
                FileName = "SignalRService",
                Method = "RegisterServices",
                Project = Lookups.Project.Client,
                Message = $"Registering SignalR Services",
                Severity = Lookups.Severity.Info,
            };

            var logResponse = await appLogService.Create<AppLogViewModel>(appLog, ApiEndpoints.AppLog);

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
