using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using EU.Kudan.Kudan;
using Prism;
using Prism.Ioc;

namespace KudanARDemo.Droid
{
    [Activity(Label = "KudanAR", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme.Splash", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static MainActivity Instance { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // スプラッシュスクリーンの後に通常のテーマに戻るよう設定
            base.Window.RequestFeature(WindowFeatures.ActionBar);
            base.SetTheme(Resource.Style.MainTheme);

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            // KudanARAPIキー設定
            var key = ARAPIKey.Instance;
            key.SetAPIKey("QDJFL+/L+a6KPmYOyGKdmxsaW45VNzMNWuP7ovEZMFzuBcw6IwDglo+a2pOwOV60bEMfNfEAFtmv/AaKWDGfC3V93MwJk3o74tC2Lh4OFokEWXqJmGGwrx7hAayiUdOGpz37ptsvlXHdap9Nl6cMGPe+cc2sVxBpGO60z5QES8VQvA3k7SnMNm0sYO1gJCb+Ryx/3EOuPHr8C506WbOjIqxrCWXOdp+wnQUQ4kZVI6KnXWEiZUu9Hr9/61PPEy+l62lgBneO0bFwTH6yriz+JuFaZIQW6NQmSEuin40HbVHpLJKPqWWVwOuocgExMkv7FBtfeonCM/zHGzxGRCXTlfPKQiVi5O3q5ArLXP1SJEFM0c4XqSREUcUnBQv6sqNht8nBPkg/qAeZGiIceIvoA6w8yTWV6BfBrnF76kbmgtkdNQ1XikmA73CsA3/upcxcpL4Rp2is4ZQyZvNgbZO7fvhqEIKbb0Cixm3b8uL6G45OQHyOYQBh+AolvOXUmQrYPZgXGXlWGPWPKJTrZg4rWrwceHvn+yGdtnaWCuLghL2BKBBziWb0d9AnQ8xgS16JNDlxVdFjZgucimffdDKFCJfpFVzktCPt6rQLGsuU6BUvpdImPnHOlL/pFX34IQPqZZ7Y2XrV+XxHaoJi3bWym5YESmUsrJkhAwKELv0obU0=");

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App(new AndroidInitializer()));

            Instance = this;
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

