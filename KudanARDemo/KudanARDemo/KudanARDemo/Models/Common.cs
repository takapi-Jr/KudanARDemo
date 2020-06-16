using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KudanARDemo.Models
{
    public class Common
    {
        public static async Task<string> GetImagePath()
        {
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
                PhotoSize = PhotoSize.Medium,
            });

            // 画像を選択しなかった場合は終了
            if (file == null)
            {
                System.Diagnostics.Debug.WriteLine("画像選択なし");
                return null;
            }

            return file.Path;
        }

        public static async Task<string> TakePhoto()
        {
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
                PhotoSize = PhotoSize.Medium,
            });

            // カメラ撮影しなかった場合は終了
            if (file == null)
            {
                System.Diagnostics.Debug.WriteLine("カメラ撮影なし");
                return null;
            }

            return file.Path;
        }
    }
}
