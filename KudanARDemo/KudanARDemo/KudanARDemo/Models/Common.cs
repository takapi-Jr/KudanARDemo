using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace KudanARDemo.Models
{
    public static class Common
    {
        public static readonly string AppDomain = "";

        public static readonly string AcrUserDialogs = "Acr.UserDialogs";
        public static readonly string GlideXForms = "glidex.forms";
        public static readonly string GraphQLClient = "GraphQL.Client";
        public static readonly string GraphQLClientSerializerNewtonsoft = "GraphQL.Client.Serializer.Newtonsoft";
        public static readonly string NETStandardLibrary = "NETStandard.Library";
        public static readonly string PluginCurrentActivity = "Plugin.CurrentActivity";
        public static readonly string PrismUnityForms = "Prism.Unity.Forms";
        public static readonly string ReactiveProperty = "ReactiveProperty";
        public static readonly string XamPluginMedia = "Xam.Plugin.Media";
        public static readonly string XamarinEssentialsInterfaces = "Xamarin.Essentials.Interfaces";
        public static readonly string XamarinForms = "Xamarin.Forms";
        public static readonly string XamarinFormsPancakeView = "Xamarin.Forms.PancakeView";

        public static readonly List<Permissions.BasePermission> GetImagePermissions = new List<Permissions.BasePermission>
        {
            new Permissions.StorageWrite(),
            new Permissions.StorageRead(),
        };

        public static readonly List<Permissions.BasePermission> TakePhotoPermissions = new List<Permissions.BasePermission>
        {
            new Permissions.Camera(),
            new Permissions.StorageWrite(),
            new Permissions.StorageRead(),
        };

        /// <summary>
        /// パーミッションチェック処理
        /// </summary>
        /// <returns>権限付与フラグ(true:付与された, false:付与されなかった)</returns>
        public static async Task<bool> CheckPermissions(List<Permissions.BasePermission> permissions)
        {
            foreach (var permission in permissions)
            {
                var status = await Common.CheckAndRequestPermissionAsync(permission);
                if (status != PermissionStatus.Granted)
                {
                    // Notify user permission was denied
                    return false;
                }
            }

            return true;
        }

        public static async Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission)
            where T : Permissions.BasePermission
        {
            var status = await permission.CheckStatusAsync();
            if (status != PermissionStatus.Granted)
            {
                status = await permission.RequestAsync();
            }

            return status;
        }

        /// <summary>
        /// 画像を選択してファイルパスを取得
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetImagePath()
        {
            // パーミッションチェック
            var grantedFlag = await Common.CheckPermissions(GetImagePermissions);
            if (!grantedFlag)
            {
                return null;
            }

            // Pluginの初期化
            await CrossMedia.Current.Initialize();

            // 画像選択可能か判定
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                return null;
            }

            // 画像選択画面を表示
            var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            {
                SaveMetaData = false,
                PhotoSize = PhotoSize.Full,
            });

            // 画像を選択しなかった場合は終了
            if (file == null)
            {
                System.Diagnostics.Debug.WriteLine("画像選択なし");
                return null;
            }

            // 画像ファイルをリネームして、不要な一時ファイルが溜まらないよう削除
            var dir = Path.GetDirectoryName(file.Path);
            var ext = Path.GetExtension(file.Path);
            var path = Path.Combine(dir, $"temp{ext}");
            File.Copy(file.Path, path, true);
            File.Delete(file.Path);
            file.Dispose();

            return path;
        }

        /// <summary>
        /// カメラで撮影してファイルパスを取得
        /// </summary>
        /// <returns></returns>
        public static async Task<string> TakePhoto()
        {
            // パーミッションチェック
            var grantedFlag = await Common.CheckPermissions(TakePhotoPermissions);
            if (!grantedFlag)
            {
                return null;
            }

            // Pluginの初期化
            await CrossMedia.Current.Initialize();

            // 撮影可能か判定
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                return null;
            }

            // カメラが起動し写真を撮影する。撮影した写真はストレージに保存され、ファイルの情報が return される
            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                // ストレージに保存するファイル情報
                // すでに同名ファイルがある場合は、temp_1.jpg などの様に連番がつけられ名前の衝突が回避される
                Directory = "TempPhotos",
                Name = "temp.jpg",
                SaveMetaData = false,
                PhotoSize = PhotoSize.Full,
            });

            // カメラ撮影しなかった場合は終了
            if (file == null)
            {
                System.Diagnostics.Debug.WriteLine("カメラ撮影なし");
                return null;
            }

            // 画像ファイルをリネームして、不要な一時ファイルが溜まらないよう削除
            var dir = Path.GetDirectoryName(file.Path);
            var ext = Path.GetExtension(file.Path);
            var path = Path.Combine(dir, $"temp{ext}");
            File.Copy(file.Path, path, true);
            File.Delete(file.Path);
            file.Dispose();

            return path;
        }
    }
}
