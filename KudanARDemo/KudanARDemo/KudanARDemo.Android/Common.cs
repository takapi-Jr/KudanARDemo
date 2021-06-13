using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using EU.Kudan.Kudan;
using KudanARDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KudanARDemo.Droid
{
    public class Common
    {
        public static async Task InitKudanAR()
        {
            // KudanARAPIキー設定
            if (string.IsNullOrEmpty(ApiKey.KudanARApiKey))
            {
                var appSyncResponse = await AppSyncService.Instance.ExecQueryAsync<GetKudanARApiKey>(AppSyncService.ApiName_GetKudanARApiKey);
                ApiKey.KudanARApiKey = appSyncResponse?.Response?.Data?.ApiKey;
                ARAPIKey.Instance.SetAPIKey(ApiKey.KudanARApiKey);
            }
        }
    }
}