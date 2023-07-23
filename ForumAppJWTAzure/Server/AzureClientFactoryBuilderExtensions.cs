namespace ForumAppJWTAzure.Server
{
    /// <summary>
    /// Handles building Blob Storage Client
    /// </summary>
    public static class AzureClientFactoryBuilderExtensions
    {
        /// <summary>
        /// Adds blob client
        /// </summary>
        /// <param name="builder">AzureClientFactoryBuilder.</param>
        /// <param name="serviceUriOrConnectionString">serviceUriOrConnectionString.</param>
        /// <param name="preferMsi">preferMsi.</param>
        /// <returns>AzureClientBuilder</returns>
        public static IAzureClientBuilder<BlobServiceClient, BlobClientOptions> AddBlobServiceClient(this AzureClientFactoryBuilder builder, string serviceUriOrConnectionString, bool preferMsi)
        {
            if (preferMsi && Uri.TryCreate(serviceUriOrConnectionString, UriKind.Absolute, out Uri? serviceUri))
            {
                return builder.AddBlobServiceClient(serviceUri);
            }
            else
            {
                return builder.AddBlobServiceClient(serviceUriOrConnectionString);
            }
        }

        public static IAzureClientBuilder<QueueServiceClient, QueueClientOptions> AddQueueServiceClient(this AzureClientFactoryBuilder builder, string serviceUriOrConnectionString, bool preferMsi)
        {
            if (preferMsi && Uri.TryCreate(serviceUriOrConnectionString, UriKind.Absolute, out Uri? serviceUri))
            {
                return builder.AddQueueServiceClient(serviceUri);
            }
            else
            {
                return builder.AddQueueServiceClient(serviceUriOrConnectionString);
            }
        }
    }
}