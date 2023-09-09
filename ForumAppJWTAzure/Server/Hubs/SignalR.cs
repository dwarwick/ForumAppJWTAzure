

using ForumAppJWTAzure.Server.Data;

namespace ForumAppJWTAzure.Server.Hubs
{
    public class SignalR : Hub
    {
        public async Task AddToGroup(string groupName)
        {
            Console.WriteLine($"Adding to group {groupName}");
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupName);
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, groupName);

            await this.Clients.Group(groupName).SendAsync("Send", $"{this.Context.ConnectionId} has left the group {groupName}.");
        }

        public async Task AddPostViewModel(string postViewModel, int postId)
        {
            await this.Clients.Groups($"post{postId}").SendAsync("AddPostViewModel", postViewModel);
        }

        public async Task EditPostViewModel(string postViewModel, int postId)
        {
            await this.Clients.Groups($"EditPost{postId}").SendAsync("EditPostViewModel", postViewModel);
        }

        public async Task AddPostVoteViewModel(string postVoteViewModel, int postId)
        {
            await this.Clients.Groups($"postVote{postId}").SendAsync("AddPostVoteViewModel", postVoteViewModel);
        }

        public async Task ForumChanged(string forumChangedViewModel, int forumId)
        {
            await this.Clients.Groups($"forumchanged{forumId}").SendAsync("ForumChanged", forumChangedViewModel);
        }
    }
}
