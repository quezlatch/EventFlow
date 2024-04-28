namespace EventFlow.AzureStorage.Extensions;

public static class EventFlowOptionsBlobStorageExtensions
    {
        public static IEventFlowOptions UseAzureBlobStorageEventStore(
            this IEventFlowOptions eventFlowOptions)
        {
            return eventFlowOptions;
        }


        public static IEventFlowOptions UseAzureBlobStorageEventStore(
            this IEventFlowOptions eventFlowOptions,
            Uri uri,
            string? connectionNamePrefix = null)
        {
            return eventFlowOptions;
        }
    }