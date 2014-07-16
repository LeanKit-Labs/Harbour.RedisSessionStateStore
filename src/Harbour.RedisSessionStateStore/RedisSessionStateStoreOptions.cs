namespace Harbour.RedisSessionStateStore
{
    public delegate void DistributedLockNotAcquiredHandler(string sessionId);

    public class RedisSessionStateStoreOptions
    {
        public string KeySeparator { get; set; }
        public int? DistributedLockAcquisitionTimeoutSeconds { get; set; }
        public int? DistributedLockTimeoutSeconds { get; set; }
        public DistributedLockNotAcquiredHandler OnDistributedLockNotAcquired { get; set; }

        public RedisSessionStateStoreOptions()
        {

        }

        internal RedisSessionStateStoreOptions(RedisSessionStateStoreOptions other)
        {
            KeySeparator = other.KeySeparator ?? RedisSessionStateStoreOptionDefaults.DefaultKeySeparator;
            DistributedLockAcquisitionTimeoutSeconds = other.DistributedLockAcquisitionTimeoutSeconds ??
                                                       RedisSessionStateStoreOptionDefaults
                                                           .DefaultDistributedLockAcquisitionTimeoutSeconds;
            DistributedLockTimeoutSeconds = other.DistributedLockTimeoutSeconds ??
                                            RedisSessionStateStoreOptionDefaults.DefaultDistributedLockTimeoutSeconds;
            OnDistributedLockNotAcquired = other.OnDistributedLockNotAcquired ??
                                           RedisSessionStateStoreOptionDefaults.DefaultOnDistributedLockNotAcquired;
        }
    }
}
