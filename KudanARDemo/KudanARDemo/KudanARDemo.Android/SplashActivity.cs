using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.AppCompat.App;
using EU.Kudan.Kudan;
using KudanARDemo.Models;

namespace KudanARDemo.Droid
{
    [Activity(Theme = "@style/MainTheme.Splash",
              MainLauncher = true,
              NoHistory = true)]
    [IntentFilter(new[] { Intent.ActionView }, AutoVerify = true, Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = "https", DataHost = "", DataPathPrefix = "")]
    public class SplashActivity : AppCompatActivity
    {
        // Launches the startup task
        protected override async void OnResume()
        {
            base.OnResume();

            // KudanARAPIキー設定(XF初期化前のため、DependencyService使用不可)
            await KudanARDemo.Droid.Common.InitKudanAR();

            var action = Intent.Action;
            var strLink = Intent.DataString;
            var intent = new Intent(Application.Context, typeof(MainActivity));
            if (Intent.ActionView == action && !string.IsNullOrWhiteSpace(strLink))
            {
                intent.SetAction(Intent.ActionView);
                intent.SetData(Intent.Data);
            }

            StartActivity(intent);
        }
    }
}