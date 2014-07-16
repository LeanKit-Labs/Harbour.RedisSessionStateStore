using System.Diagnostics;

namespace Harbour.RedisSessionStateStore
{
    public static class RedisSessionStateStoreOptionDefaults
    {
        public static readonly string DefaultKeySeparator = "/";
        public static readonly int DefaultDistributedLockAcquisitionTimeoutSeconds = 1;
        public static readonly int DefaultDistributedLockTimeoutSeconds = 1;

        public static readonly DistributedLockNotAcquiredHandler DefaultOnDistributedLockNotAcquired =
            sessionId => Debug.WriteLine("Session \"{0}\" could not establish distributed lock. " +
                                         "This most likely means you have to increase the " +
                                         "DistributedLockAcquireSeconds/DistributedLockTimeoutSeconds.", sessionId);
    }
}
