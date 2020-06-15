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
using EU.Kudan.Kudan;

namespace KudanARDemo.Droid
{
    [Activity(Label = "MarkerlessARActivity")]
    public class MarkerlessARActivity : ARActivity, GestureDetector.IOnGestureListener
    {
        public GestureDetectorCompat GestureDetect { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here

            // ArbiTrack を開始および停止するジェスチャ認識機能を作成
            GestureDetect = new GestureDetectorCompat(this, this);
        }

        public override void Setup()
        {
            base.Setup();

            // 設定する AR コンテンツをここに記述

            //////////////////////////////////////////////////////////////////
            // ArbiTrack を初期化
            var arbiTrack = ARArbiTrack.Instance;
            arbiTrack.Initialise();

            // ジャイロスコープの配置を初期化 
            var gyroPlaceManager = ARGyroPlaceManager.Instance;
            gyroPlaceManager.Initialise();

            //////////////////////////////////////////////////////////////////
            // ターゲットとして使用されるノードを作成
            var targetNode = new ARImageNode("Kudan_Cow.png");

            // デバイスのジャイロスコープとともに移動するように、
            // ジャイロスコープ配置マネージャーのワールド空間にターゲット ノードを追加
            gyroPlaceManager.World.AddChild(targetNode);

            // 正しく表示されるようにノードを回転およびスケーリング
            targetNode.RotateByDegrees(90.0f, 1.0f, 0.0f, 0.0f);
            targetNode.RotateByDegrees(180.0f, 0.0f, 1.0f, 0.0f);

            targetNode.ScaleByUniform(0.3f);

            // 正しく表示されるようにノードを回転およびスケーリング
            arbiTrack.TargetNode = targetNode;

            //////////////////////////////////////////////////////////////////
            // トラッキングされるノードを作成
            var trackingNode = new ARImageNode("Kudan_Cow.png");

            // 正しく表示されるようにノードを回転
            trackingNode.RotateByDegrees(90.0f, 1.0f, 0.0f, 0.0f);
            trackingNode.RotateByDegrees(180.0f, 0.0f, 1.0f, 0.0f);

            // ArbiTracker のワールド空間に子としてノードを追加
            arbiTrack.World.AddChild(trackingNode);
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            GestureDetect.OnTouchEvent(e);
            return base.OnTouchEvent(e);
        }

        public bool OnDown(MotionEvent e)
        {
            return false;
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
    }
}