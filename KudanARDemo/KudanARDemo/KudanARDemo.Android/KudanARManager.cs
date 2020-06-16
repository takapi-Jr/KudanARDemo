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

[assembly: Dependency(typeof(KudanARManager))]
namespace KudanARDemo.Droid
{
    public class KudanARManager : IKudanARManager
    {
        public async Task StartMarkerARActivityAsync()
        {
            // パーミッションチェック
            var grantedFlag = await CheckPermissions();
            if (!grantedFlag)
            {
                return;
            }

            MainActivity.Instance.StartActivity(new Android.Content.Intent(MainActivity.Instance, typeof(MarkerARActivity)));
        }

        public async Task StartMarkerlessARActivityAsync()
        {
            // パーミッションチェック
            var grantedFlag = await CheckPermissions();
            if (!grantedFlag)
            {
                return;
            }

            MainActivity.Instance.StartActivity(new Android.Content.Intent(MainActivity.Instance, typeof(MarkerlessARActivity)));
        }

        public async Task StartMarkerlessWallActivityAsync()
        {
            // パーミッションチェック
            var grantedFlag = await CheckPermissions();
            if (!grantedFlag)
            {
                return;
            }

            MainActivity.Instance.StartActivity(new Android.Content.Intent(MainActivity.Instance, typeof(Markerless_Wall)));
        }

        /// <summary>
        /// パーミッションチェック処理
        /// </summary>
        /// <returns>権限付与フラグ(true:付与された, false:付与されなかった)</returns>
        private async Task<bool> CheckPermissions()
        {
            var status = await Common.CheckAndRequestPermissionAsync(new Permissions.Camera());
            if (status != PermissionStatus.Granted)
            {
                // Notify user permission was denied
                return false;
            }

            status = await Common.CheckAndRequestPermissionAsync(new Permissions.StorageWrite());
            if (status != PermissionStatus.Granted)
            {
                // Notify user permission was denied
                return false;
            }

            status = await Common.CheckAndRequestPermissionAsync(new Permissions.StorageRead());
            if (status != PermissionStatus.Granted)
            {
                // Notify user permission was denied
                return false;
            }

            return true;
        }
    }
}