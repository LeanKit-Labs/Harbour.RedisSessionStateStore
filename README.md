This code is derivative of [Harbour.RedisSessionStateStore](https://github.com/TheCloudlessSky/Harbour.RedisSessionStateStore). Please go there to see the readme of inner workings and whatnot. I'll be pushing stuff back there soon, I promise. :)

------------

Configure the session state provider in your web.config as such:

```xml
<system.web>
  <sessionState mode="Custom" customProvider="RedisSessionStateProvider">
    <providers>
      <clear />
      <add name="RedisSessionStateProvider" type="Harbour.RedisSessionStateStore.RedisSessionStateStoreProvider" />
    </providers>
  </sessionState>
</system.web>
```

Then, wire up the provider in global.asax:

```csharp
private readonly IRedisClientsManager _clientManager = new PooledRedisClientManager(new[] { "ipAddress:port" });
private const string KeySeparator = @":";

protected void Application_Start()
{
    // Or use your IoC container to wire this up.
	RedisSessionStateStoreProvider.SetClientManager(_clientManager);
    RedisSessionStateStoreProvider.SetOptions(new RedisSessionStateStoreOptions()
    {
        KeySeparator = KeySeparator,
        OnDistributedLockNotAcquired =
            sessionId => Console.WriteLine("Session \"{0}\" could not establish distributed lock. " +
                                           "This most likely means you have to increase the " +
                                           "DistributedLockAcquireSeconds/DistributedLockTimeoutSeconds.",
                sessionId)
    });
}

protected void Application_End()
{
    this.clientManager.Dispose();
}

// This adds a new object to track the ProviderUserKey value into another redis doc.
// It uses the session ID key only, not the prefix or key separators, since other apps
// may not have access to global.asax or web.config info.
// This doc is managed internally by the session state provider and has its timeout extended
// or is deleted appropriately in conjunction with actions on the actual session object.
protected void Session_Start(object sender, EventArgs e)
{
    using (var client = _clientManager.GetClient())
    {
        client.Add(
            String.Format("{0}", Session.SessionID),
            User.Identity.GetUserId(), DateTime.Now.AddMinutes(Session.Timeout));
    }
}
```