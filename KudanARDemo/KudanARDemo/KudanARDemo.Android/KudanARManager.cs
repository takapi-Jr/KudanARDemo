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
using static Xamarin.Essentials.Permissions;

[assembly: Dependency(typeof(KudanARManager))]
namespace KudanARDemo.Droid
{
    public class KudanARManager : IKudanARManager
    {
        public async Task StartARActivityAsync()
        {
            var status = await Common.CheckAndRequestPermissionAsync(new Permissions.Camera());
            if (status != PermissionStatus.Granted)
            {
                // Notify user permission was denied
                return;
            }

            status = await Common.CheckAndRequestPermissionAsync(new Permissions.StorageWrite());
            if (status != PermissionStatus.Granted)
            {
                // Notify user permission was denied
                return;
            }

            status = await Common.CheckAndRequestPermissionAsync(new Permissions.StorageRead());
            if (status != PermissionStatus.Granted)
            {
                // Notify user permission was denied
                return;
            }

            MainActivity.Instance.StartActivity(new Android.Content.Intent(MainActivity.Instance, typeof(SubActivity)));
        }
    }
}