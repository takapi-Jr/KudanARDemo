using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
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
    [Activity(Label = "Markerless_Wall")]
    public class Markerless_Wall : ARActivity, GestureDetector.IOnGestureListener, ScaleGestureDetector.IOnScaleGestureListener, IARArbiTrackListener, RotationGestureDetector.IOnRotationGestureListener
    {
        public GestureDetectorCompat GestureDetect { get; set; }
        public ScaleGestureDetector ScaleGestureDetect { get; set; }
        public RotationGestureDetector RotationGestureDetect { get; set; }
        public ARNode WallTargetNode { get; set; }

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
            // デバイスの向きを検出
            var wallOrientation = WallOrientationForDeviceOrientation();

            //////////////////////////////////////////////////////////////////
            // ターゲットとして使用されるノードを作成
            var targetPosition = new Vector3f(0, 0, -1000);     // スクリーンから1000離れた場所に置く
            var wallScale = new Vector3f(0.5f, 0.5f, 0.5f);
            this.WallTargetNode = CreateImageNode(MainPageViewModel.ImageNodeInfo.Value, wallOrientation, wallScale, targetPosition);

            //////////////////////////////////////////////////////////////////
            // ターゲットノードをコンテンツビューポートに関連付けられたカメラノードの子として追加
            this.ARView.ContentViewPort.Camera.AddChild(this.WallTargetNode);

            //////////////////////////////////////////////////////////////////
            // ArbiTrack ワールド空間に配置する画像ノードを作成
            var trackingImageNode = CreateImageNode(MainPageViewModel.ImageNodeInfo.Value, Quaternion.Identity, Vector3f.UnitXyz, new Vector3f(0.0f, 0.0f, 0.0f));
            //var trackingImageNode = CreateModelNode(Quaternion.Identity, Vector3f.UnitXyz, new Vector3f(0.0f, 0.0f, 0.0f));

            // ArbiTrack のセットアップ
            SetUpArbiTrack(this.WallTargetNode, trackingImageNode);
        }

        private static Quaternion WallOrientationForDeviceOrientation()
        {
            var context = ARRenderer.Instance.Activity as Context;
            var windowManager = context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
            var display = windowManager.DefaultDisplay;
            var displayRotation = display.Rotation;

            // The angles we will rotate our wall node by
            // The components are {x,y,z} in radians
            var angles = new[] { 0.0f, 0.0f, 0.0f };
            var rotation = Quaternion.Identity;

            switch (displayRotation)
            {
                case SurfaceOrientation.Rotation0:
                    angles[2] = (float)Math.PI / 2.0f;
                    rotation = new Quaternion(angles);
                    break;
                case SurfaceOrientation.Rotation90:
                    rotation = Quaternion.Identity;
                    break;
                case SurfaceOrientation.Rotation180:
                    angles[2] = -(float)Math.PI / 2.0f;
                    rotation = new Quaternion(angles);
                    break;
                case SurfaceOrientation.Rotation270:
                    angles[2] = (float)Math.PI;
                    rotation = new Quaternion(angles);
                    break;
                default:
                    break;
            }

            return rotation;
        }

        private ARImageNode CreateImageNode(ImageInfo imageInfo, Quaternion orientation, Vector3f scale, Vector3f position)
        {
            // ノードを作成
            var texture = CreateTexture2D(imageInfo);
            var imageNode = new ARImageNode(texture);

            // 回転およびスケーリング
            imageNode.Orientation = orientation;
            imageNode.Scale = scale;
            imageNode.Position = position;

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

#if false   // 3Dモデル使用

        private ARModelNode CreateModelNode(Quaternion orientation, Vector3f scale, Vector3f position)
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

            modelNode.RotateByDegrees(90.0f, 0.0f, 0.0f, 1.0f);
            modelNode.ScaleByUniform(0.25f);
            modelNode.Position = position;

            return modelNode;
        }

#endif

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

            // アクティビティを ArbiTrack デリゲートとして追加
            arbiTrack.AddListener(this);
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            this.WallTargetNode.Orientation = WallOrientationForDeviceOrientation();
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

        public void ArbiTrackStarted()
        {
            var arbiTrack = ARArbiTrack.Instance;

            // Rotate the tracking node so that it has the same full orientation as the target node
            // As the target node is a child of the camera world and the tracking node is a child of arbitrack's world, we must first rotate the tracking node by the inverse of arbitrack's world orientation.
            // This is so to the eye it has the same orientation as the target node

            // At this point we can update the orientation of the tracking node as arbitrack will have updated it's orientation
            var targetOrientation = arbiTrack.TargetNode.Orientation;
            var trackingNode = arbiTrack.World.Children.FirstOrDefault();
            trackingNode.Orientation = arbiTrack.World.Orientation.Inverse().Mult(targetOrientation);
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