using Prism;
using Prism.Ioc;
using KudanARDemo.ViewModels;
using KudanARDemo.Views;
using Xamarin.Essentials.Interfaces;
using Xamarin.Essentials.Implementation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using KudanARDemo.Models;
using System;
using System.Linq;
using Prism.Navigation;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace KudanARDemo
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<SettingPage, SettingPageViewModel>();
            containerRegistry.RegisterForNavigation<LicensePage, LicensePageViewModel>();
            containerRegistry.RegisterForNavigation<LicenseDetailPage, LicenseDetailPageViewModel>();
        }

        protected override async void OnAppLinkRequestReceived(Uri uri)
        {
            if (!uri.ToString().ToLowerInvariant().StartsWith(Common.AppDomain, StringComparison.Ordinal))
            {
                return;
            }
            
            var query = uri.Query;
            if (!query.StartsWith("?"))
            {
                return;
            }
            query = query.Substring(1);
            var parsedQuery = System.Web.HttpUtility.ParseQueryString(query);
            var activityMode = parsedQuery.AllKeys.Contains("mode") ? parsedQuery.Get("mode") : string.Empty;
            
            if (!string.IsNullOrEmpty(activityMode))
            {
                var navigationParameters = new NavigationParameters()
                {
                    { "ActivityMode", activityMode },
                };
                await NavigationService.NavigateAsync("/NavigationPage/MainPage", navigationParameters);
            }

            base.OnAppLinkRequestReceived(uri);
        }
    }
}
