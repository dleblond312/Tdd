using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Tdd.Controllers;
using Tdd.Helpers;
using Tdd.Models;
using Tdd.Providers;
using Tdd.Services;

[assembly: OwinStartup(typeof(Tdd.App_Start.Startup))]
namespace Tdd.App_Start
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var container = new UnityContainer();
            this.RegisterTypes(container);

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new SignalRContractResolver();
            var serializer = JsonSerializer.Create(settings);
            GlobalHost.DependencyResolver.Register(typeof(JsonSerializer), () => serializer);
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            GlobalHost.DependencyResolver.Register(typeof(GameHub), () => new GameHub(container.Resolve<GameService>(), container.Resolve<ChatService>()));
            app.MapSignalR(new HubConfiguration
            {
                EnableDetailedErrors = true
            });

            this.ConfigureOAuth(app);
        }

        private void ConfigureOAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext<AuthContext>(() => new AuthContext());
            app.CreatePerOwinContext<UserManager<IdentityUser>>(CreateManager);

            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/api/oauth/token"),
                AuthorizeEndpointPath = new PathString("/login"),
                Provider = new AuthorizationServerProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30)

            });
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        private static UserManager<IdentityUser> CreateManager(IdentityFactoryOptions<UserManager<IdentityUser>> options, IOwinContext context)
        {
            var userStore = new UserStore<IdentityUser>(context.Get<AuthContext>());
            var owinManager = new UserManager<IdentityUser>(userStore);
            return owinManager;
        }

        private void RegisterTypes(IUnityContainer container)
        {
            container.RegisterTypes(
                AllClasses.FromLoadedAssemblies().Where(
                    t => t.Name.ToLower().Contains("service")),
                WithMappings.FromAllInterfacesInSameAssembly,
                WithName.Default,
                WithLifetime.ContainerControlled);

            container.RegisterType<GameHub>(new InjectionFactory(CreateGameHub));
        }

        private GameHub CreateGameHub(IUnityContainer container)
        {
            var gameHub = new GameHub(container.Resolve<IGameService>(), container.Resolve<IChatService>());
            return gameHub;
        }
    }
}