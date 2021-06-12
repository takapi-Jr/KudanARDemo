using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using EU.Kudan.Kudan;
using KudanARDemo.Models;
using Prism;
using Prism.Ioc;

namespace KudanARDemo.Droid
{
    [Activity(Label = "KudanAR", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme.Splash", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            // スプラッシュスクリーンの後に通常のテーマに戻るよう設定
            base.Window.RequestFeature(WindowFeatures.ActionBar);
            base.SetTheme(Resource.Style.MainTheme);

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            // KudanARAPIキー設定
            if (string.IsNullOrEmpty(ApiKey.KudanARApiKey))
            {
                var appSyncResponse = await AppSyncService.Instance.ExecQueryAsync<GetKudanARApiKey>(AppSyncService.ApiName_GetKudanARApiKey);
                ApiKey.KudanARApiKey = appSyncResponse?.Response?.Data?.ApiKey;
            }
            ARAPIKey.Instance.SetAPIKey(ApiKey.KudanARApiKey);

            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Android.Glide.Forms.Init(this, debug: true);
            Acr.UserDialogs.UserDialogs.Init(this);
            LoadApplication(new App(new AndroidInitializer()));
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}

