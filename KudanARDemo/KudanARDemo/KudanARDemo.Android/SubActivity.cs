using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using EU.Kudan.Kudan;

namespace KudanARDemo.Droid
{
    [Activity(Label = "SubActivity")]
    public class SubActivity : ARActivity, IARImageTrackableListener
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
            ImageTrackable = new ARImageTrackable("Lego_Marker");
            ImageTrackable.LoadFromAsset("Kudan_Lego_Marker.jpg");
            
            //////////////////////////////////////////////////////////////////
            // 画像トラッカーの 1 つのインスタンスを取得
            var imageTracker = ARImageTracker.Instance;
            imageTracker.Initialise();

            // 画像トラッカブルを画像トラッカーに追加
            imageTracker.AddTrackable(ImageTrackable);
            
            //////////////////////////////////////////////////////////////////
            // 画像で画像ノードを初期化
            var imageNode = new ARImageNode("Kudan_Cow.png");
            //var imageNode = new ARImageNode("img_zamarin.png");
            //var imageNode = new ARImageNode("neko.png");

            // 画像ノードをトラッカブルのワールド空間の子として追加
            ImageTrackable.World.AddChild(imageNode);

            //////////////////////////////////////////////////////////////////
            // リスナー登録
            ImageTrackable.AddListener(this);
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