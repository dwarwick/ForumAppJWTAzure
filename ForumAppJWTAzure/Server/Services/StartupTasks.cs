using ForumAppJWTAzure.Shared.Models;

namespace ForumAppJWTAzure.Server.Services
{
    public class StartupTasks
    {        
        public async Task PrimeMlModels(ITagService tagService)
        {
            Console.WriteLine("Priming Models");
            ModelInput modelInput = new ModelInput()
            {
                Title = "Test",
                Body = "Test",
                Tags = string.Empty
            };

            await tagService.MakePredictions(modelInput);
        }
    }
}
