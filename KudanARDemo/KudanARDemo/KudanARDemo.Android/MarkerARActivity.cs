using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using EU.Kudan.Kudan;
using KudanARDemo.Models;
using KudanARDemo.ViewModels;
using Xamarin.Forms;

namespace KudanARDemo.Droid
{
    [Activity(Label = "MarkerARActivity")]
    public class MarkerARActivity : ARActivity, IARImageTrackableListener
    {
        public ARImageTrackable ImageTrackable { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
        }

        public override void Setup()
        {
            base.Setup();

            // 設定する AR コンテンツをここに記述

            //////////////////////////////////////////////////////////////////
            // 画像トラッカブルを初期化して画像をロード
            ImageTrackable = CreateImageTrackable(MainPageViewModel.ImageMarkerInfo.Value);

            //////////////////////////////////////////////////////////////////
            // 画像トラッカーの 1 つのインスタンスを取得
            var imageTracker = ARImageTracker.Instance;
            imageTracker.Initialise();

            // 画像トラッカブルを画像トラッカーに追加
            imageTracker.AddTrackable(ImageTrackable);

            //////////////////////////////////////////////////////////////////
            // 画像で画像ノードを初期化
            //var imageNode = new ARImageNode("Kudan_Cow.png");
            var texture = CreateTexture2D(MainPageViewModel.ImageNodeInfo.Value);
            var imageNode = new ARImageNode(texture);

            //////////////////////////////////////////////////////////////////
            // imageNode のサイズを Trackable のサイズに合わせる
            var textureMaterial = imageNode.Material as ARTextureMaterial;
            var scale = ImageTrackable.Width / textureMaterial.Texture.Width;
            imageNode.ScaleByUniform(scale);

            // 画像ノードをトラッカブルのワールド空間の子として追加
            ImageTrackable.World.AddChild(imageNode);

            //////////////////////////////////////////////////////////////////
            // リスナー登録
            ImageTrackable.AddListener(this);
        }

        private ARImageTrackable CreateImageTrackable(ImageInfo imageInfo)
        {
            // 画像トラッカブルを初期化して画像をロード
            // ビルドアクションがAndroidAssetのファイル名を指定
            var imageTrackable = new ARImageTrackable("Lego_Marker");
            //imageTrackable.LoadFromAsset("Kudan_Lego_Marker.jpg");
            if (imageInfo.IsAsset)
            {
                imageTrackable.LoadFromAsset(imageInfo.ImagePath);
            }
            else
            {
                imageTrackable.LoadFromPath(imageInfo.ImagePath);
            }

            return imageTrackable;
        }

        private ARTexture2D CreateTexture2D(ImageInfo imageInfo)
        {
            // 画像で画像ノードを初期化
            // ビルドアクションがAndroidAssetのファイル名を指定
            var texture = new ARTexture2D();
            if (imageInfo.IsAsset)
            {
                texture.LoadFromAsset(imageInfo.ImagePath);
            }
            else
            {
                texture.LoadFromPath(imageInfo.ImagePath);
            }

            return texture;
        }

        public void DidDetect(ARImageTrackable p0)
        {
            System.Diagnostics.Debug.WriteLine("Did Detect");
        }

        public void DidLose(ARImageTrackable p0)
        {
            System.Diagnostics.Debug.WriteLine("Did Lose");
        }

        public void DidTrack(ARImageTrackable p0)
        {
            System.Diagnostics.Debug.WriteLine("Did Track");
        }
    }
}