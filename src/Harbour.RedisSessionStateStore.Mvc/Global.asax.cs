using Microsoft.AspNet.Identity;
using ServiceStack.Redis;
using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Harbour.RedisSessionStateStore.Mvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private readonly IRedisClientsManager _clientManager = new PooledRedisClientManager(new[] { "172.16.83.1:6379" });
        private const string KeySeparator = @":";

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

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
            _clientManager.Dispose();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            using (var client = _clientManager.GetClient())
            {
                client.Add(
                    String.Format("{0}", Session.SessionID),
                    User.Identity.GetUserId(), DateTime.Now.AddMinutes(Session.Timeout));
            }
        }
    }
}
