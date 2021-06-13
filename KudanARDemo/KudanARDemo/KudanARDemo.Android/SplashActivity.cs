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
    public class SplashActivity : AppCompatActivity
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // KudanARAPIキー設定(XF初期化前のため、DependencyService使用不可)
            await KudanARDemo.Droid.Common.InitKudanAR();
        }

        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}