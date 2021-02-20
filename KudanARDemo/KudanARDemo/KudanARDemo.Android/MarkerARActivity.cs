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
using AndroidX.Core.View;
using EU.Kudan.Kudan;
using KudanARDemo.Models;
using KudanARDemo.ViewModels;
using Xamarin.Forms;

namespace KudanARDemo.Droid
{
    [Activity(Label = "MarkerAR")]
    public class MarkerARActivity : ARActivity, IARImageTrackableListener, GestureDetector.IOnGestureListener, ScaleGestureDetector.IOnScaleGestureListener, RotationGestureDetector.IOnRotationGestureListener
    {
        public GestureDetectorCompat GestureDetect { get; set; }
        public ScaleGestureDetector ScaleGestureDetect { get; set; }
        public RotationGestureDetector RotationGestureDetect { get; set; }
        public ARImageTrackable ImageTrackable { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here

            // Activityテーマを設定
            base.SetTheme(Resource.Style.ARActivityTheme);

            // 説明文を表示
            SetContentView(Resource.Layout.OverlayView);
            var textView = FindViewById<TextView>(Resource.Id.textView1);
            var label = GetString(Resource.String.marker_description);
            textView.SetText(label, TextView.BufferType.Normal);

            // フェードアウト処理
            Animation.FadeOut(textView);

            GestureDetect = new GestureDetectorCompat(this, this);
            ScaleGestureDetect = new ScaleGestureDetector(this, this);
            RotationGestureDetect = new RotationGestureDetector(this);
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
            //var modelNode = CreateModelNode();
            //ImageTrackable.World.AddChild(modelNode);

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

#if false   // 3Dモデル使用

        private ARModelNode CreateModelNode()
        {
            // Import model
            var modelImporter = new ARModelImporter();
            modelImporter.LoadFromAsset("bigBen.jet");
            var modelNode = modelImporter.Node;

            // Load model texture
            var texture2D = new ARTexture2D();
            texture2D.LoadFromAsset("bigBenTexture.png");

            // Apply model texture to model texture material
            var material = new ARLightMaterial();
            material.SetTexture(texture2D);
            material.SetAmbient(0.8f, 0.8f, 0.8f);

            // Apply texture material to models mesh nodes
            foreach (var meshNode in modelImporter.MeshNodes)
            {
                meshNode.Material = material;
            }

            modelNode.RotateByDegrees(90.0f, 1.0f, 0.0f, 0.0f);
            modelNode.ScaleByUniform(0.25f);

            return modelNode;
        }

#endif

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

        public override bool OnTouchEvent(MotionEvent e)
        {
            GestureDetect.OnTouchEvent(e);
            ScaleGestureDetect.OnTouchEvent(e);
            RotationGestureDetect.onTouchEvent(e);
            return base.OnTouchEvent(e);
        }

        public bool OnDown(MotionEvent e)
        {
            return true;
        }

        public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            return false;
        }

        public void OnLongPress(MotionEvent e)
        {
        }

        public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
        {
            return false;
        }

        public void OnShowPress(MotionEvent e)
        {
        }

        public bool OnSingleTapUp(MotionEvent e)
        {
            return false;
        }

        public bool OnScale(ScaleGestureDetector detector)
        {
            // 拡大縮小処理
            var scale = detector.ScaleFactor;
            ImageTrackable.World.Children.FirstOrDefault()?.ScaleByUniform(scale);

            return true;
        }

        public bool OnScaleBegin(ScaleGestureDetector detector)
        {
            return true;
        }

        public void OnScaleEnd(ScaleGestureDetector detector)
        {
        }

        public bool OnRotateBegin(RotationGestureDetector detector)
        {
            return true;
        }

        public void OnRotate(RotationGestureDetector detector)
        {
            // 回転処理
            var angle = detector.Angle;
            ImageTrackable.World.Children.FirstOrDefault()?.RotateByDegrees(angle, 0.0f, 0.0f, 1.0f);
        }

        public void OnRotateEnd(RotationGestureDetector detector)
        {
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();

            // ビジー状態を解除
            MainPageViewModel.IsBusy.Value = false;
        }
    }
}