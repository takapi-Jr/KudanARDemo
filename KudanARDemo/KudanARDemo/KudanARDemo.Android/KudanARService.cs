using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using KudanARDemo.Droid;
using KudanARDemo.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(KudanARService))]
namespace KudanARDemo.Droid
{
    public class KudanARService : IKudanARService
    {
        public static readonly List<Permissions.BasePermission> KudanARPermissions = new List<Permissions.BasePermission>
        {
            new Permissions.Camera(),
            new Permissions.StorageWrite(),
            new Permissions.StorageRead(),
        };

        public async Task StartMarkerARActivityAsync()
        {
            // パーミッションチェック
            var grantedFlag = await Common.CheckPermissions(KudanARPermissions);
            if (!grantedFlag)
            {
                return;
            }

            MainActivity.Instance.StartActivity(new Android.Content.Intent(MainActivity.Instance, typeof(MarkerARActivity)));
        }

        public async Task StartMarkerlessARActivityAsync()
        {
            // パーミッションチェック
            var grantedFlag = await Common.CheckPermissions(KudanARPermissions);
            if (!grantedFlag)
            {
                return;
            }

            MainActivity.Instance.StartActivity(new Android.Content.Intent(MainActivity.Instance, typeof(MarkerlessARActivity)));
        }

        public async Task StartMarkerlessWallActivityAsync()
        {
            // パーミッションチェック
            var grantedFlag = await Common.CheckPermissions(KudanARPermissions);
            if (!grantedFlag)
            {
                return;
            }

            MainActivity.Instance.StartActivity(new Android.Content.Intent(MainActivity.Instance, typeof(Markerless_Wall)));
        }
    }
}