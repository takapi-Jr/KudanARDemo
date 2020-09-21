using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Xamarin.Forms;

namespace KudanARDemo.Droid
{
    public class Animation
    {
        public static void FadeOut(Android.Views.View view)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var alphaFadeout = new AlphaAnimation(1.0f, 0.0f);
                alphaFadeout.StartOffset = 10000;
                alphaFadeout.Duration = 3000;
                alphaFadeout.FillAfter = true;
                view.StartAnimation(alphaFadeout);
            });
            
            return;
        }
    }
}