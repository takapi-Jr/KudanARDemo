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
using KudanARDemo.ViewModels;
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
                // ビジー状態を解除
                MainPageViewModel.IsBusy.Value = false;
                return;
            }

            var context = (Context)Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity;
            using (var intent = new Android.Content.Intent(context, typeof(MarkerARActivity)))
            {
                context.StartActivity(intent);
            }
        }

        public async Task StartMarkerlessARActivityAsync()
        {
            // パーミッションチェック
            var grantedFlag = await Common.CheckPermissions(KudanARPermissions);
            if (!grantedFlag)
            {
                // ビジー状態を解除
                MainPageViewModel.IsBusy.Value = false;
                return;
            }

            var context = (Context)Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity;
            using (var intent = new Android.Content.Intent(context, typeof(MarkerlessARActivity)))
            {
                context.StartActivity(intent);
            }
        }

        public async Task StartMarkerlessWallActivityAsync()
        {
            // パーミッションチェック
            var grantedFlag = await Common.CheckPermissions(KudanARPermissions);
            if (!grantedFlag)
            {
                // ビジー状態を解除
                MainPageViewModel.IsBusy.Value = false;
                return;
            }

            var context = (Context)Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity;
            using (var intent = new Android.Content.Intent(context, typeof(Markerless_Wall)))
            {
                context.StartActivity(intent);
            }
        }
    }
}