using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gestures;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.View;
using Com.Jme3.Math;
using EU.Kudan.Kudan;
using KudanARDemo.Models;
using KudanARDemo.ViewModels;

namespace KudanARDemo.Droid
{
    [Activity(Label = "MarkerlessARActivity")]
    public class MarkerlessARActivity : ARActivity, GestureDetector.IOnGestureListener, ScaleGestureDetector.IOnScaleGestureListener, RotationGestureDetector.IOnRotationGestureListener
    {
        public GestureDetectorCompat GestureDetect { get; set; }
        public ScaleGestureDetector ScaleGestureDetect { get; set; }
        public RotationGestureDetector RotationGestureDetect { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here

            // ArbiTrack を開始および停止するジェスチャ認識機能を作成
            GestureDetect = new GestureDetectorCompat(this, this);
            ScaleGestureDetect = new ScaleGestureDetector(this, this);
            RotationGestureDetect = new RotationGestureDetector(this);
        }

        public override void Setup()
        {
            base.Setup();

            // 設定する AR コンテンツをここに記述

            //////////////////////////////////////////////////////////////////
            // ターゲットとして使用されるノードを作成
            var angles = new[] { -(float)Math.PI / 2.0f, (float)Math.PI / 2.0f, 0.0f };
            var floorOrientation = new Quaternion(angles);

            var floorScale = new Vector3f(0.25f, 0.25f, 0.25f);
            var floorTarget = CreateImageNode(MainPageViewModel.ImageNodeInfo.Value, floorOrientation, floorScale);

            // ジャイロスコープ配置マネージャーのワールド空間にターゲット ノードを追加
            AddNodeToGyroPlaceManager(floorTarget);

            //////////////////////////////////////////////////////////////////
            // トラッキングされるノードを作成
            var trackingScale = Vector3f.UnitXyz;
            var trackingImageNode = CreateImageNode(MainPageViewModel.ImageNodeInfo.Value, floorOrientation, trackingScale);

            // ArbiTrack のセットアップ
            SetUpArbiTrack(floorTarget, trackingImageNode);
        }

        private ARImageNode CreateImageNode(ImageInfo imageInfo, Quaternion orientation, Vector3f scale)
        {
            // ノードを作成
            var texture = CreateTexture2D(imageInfo);
            var imageNode = new ARImageNode(texture);

            // 回転およびスケーリング
            imageNode.Orientation = orientation;
            imageNode.Scale = scale;

            return imageNode;
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

        private void AddNodeToGyroPlaceManager(ARNode node)
        {
            // ジャイロスコープの配置を初期化
            var gyroPlaceManager = ARGyroPlaceManager.Instance;
            gyroPlaceManager.Initialise();

            // デバイスのジャイロスコープとともに移動するように、
            // ジャイロスコープ配置マネージャーのワールド空間にターゲット ノードを追加
            gyroPlaceManager.World.AddChild(node);
        }

        private void SetUpArbiTrack(ARNode targetNode, ARNode childNode)
        {
            // ArbiTrack を初期化
            var arbiTrack = ARArbiTrack.Instance;
            arbiTrack.Initialise();

            // ターゲットノードを設定
            arbiTrack.TargetNode = targetNode;

            // ArbiTrack のワールド空間に子としてノードを追加
            arbiTrack.World.AddChild(childNode);
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
            var arbiTrack = ARArbiTrack.Instance;

            if (arbiTrack.IsTracking)
            {
                // ArbiTrack がトラッキング中の場合は、トラッキングを停止してワールド空間がレンダリングされないようにし、ターゲット ノードを表示
                arbiTrack.Stop();
                arbiTrack.TargetNode.Visible = true;
            }
            else
            {
                // トラッキング中でない場合は、トラッキングを開始してターゲット ノードを非表示にする
                arbiTrack.Start();
                arbiTrack.TargetNode.Visible = false;
            }

            return false;
        }

        public bool OnScale(ScaleGestureDetector detector)
        {
            // ノード画像が固定されている状態(トラッキング中)のみ拡大縮小を許可
            var arbiTrack = ARArbiTrack.Instance;
            if (arbiTrack.IsTracking)
            {
                var scale = detector.ScaleFactor;
                arbiTrack.World.Children.FirstOrDefault()?.ScaleByUniform(scale);
            }

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
            // ノード画像が固定されている状態(トラッキング中)のみ回転を許可
            var arbiTrack = ARArbiTrack.Instance;
            if (arbiTrack.IsTracking)
            {
                var angle = detector.Angle;
                arbiTrack.World.Children.FirstOrDefault()?.RotateByDegrees(angle, 0.0f, 0.0f, 1.0f);
            }
        }

        public void OnRotateEnd(RotationGestureDetector detector)
        {
        }
    }
}