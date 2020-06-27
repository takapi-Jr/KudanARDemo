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

namespace KudanARDemo.Droid
{
    public class RotationGestureDetector
    {
        private static readonly int INVALID_POINTER_ID = -1;
        private float fX, fY, sX, sY;
        private int ptrID1, ptrID2;
        private IOnRotationGestureListener Listener;

        public bool IsInProgress { get; private set; }
        public float Angle { get; private set; }

        public RotationGestureDetector(IOnRotationGestureListener listener)
        {
            Listener = listener;
            ptrID1 = INVALID_POINTER_ID;
            ptrID2 = INVALID_POINTER_ID;
        }

        public bool onTouchEvent(MotionEvent e)
        {
            switch (e.ActionMasked)
            {
                case MotionEventActions.Down:
                    ptrID1 = e.GetPointerId(e.ActionIndex);
                    IsInProgress = false;
                    break;
                case MotionEventActions.PointerDown:
                    ptrID2 = e.GetPointerId(e.ActionIndex);
                    sX = e.GetX(e.FindPointerIndex(ptrID1));
                    sY = e.GetY(e.FindPointerIndex(ptrID1));
                    fX = e.GetX(e.FindPointerIndex(ptrID2));
                    fY = e.GetY(e.FindPointerIndex(ptrID2));
                    if (Listener != null)
                    {
                        IsInProgress = Listener.OnRotateBegin(this);
                    }
                    break;
                case MotionEventActions.Move:
                    if (IsInProgress && ptrID1 != INVALID_POINTER_ID && ptrID2 != INVALID_POINTER_ID)
                    {
                        float nfX, nfY, nsX, nsY;
                        nsX = e.GetX(e.FindPointerIndex(ptrID1));
                        nsY = e.GetY(e.FindPointerIndex(ptrID1));
                        nfX = e.GetX(e.FindPointerIndex(ptrID2));
                        nfY = e.GetY(e.FindPointerIndex(ptrID2));

                        Angle = AngleBetweenLines(fX, fY, sX, sY, nfX, nfY, nsX, nsY);

                        if (Listener != null)
                        {
                            Listener.OnRotate(this);
                        }

                        sX = nsX;
                        sY = nsY;
                        fX = nfX;
                        fY = nfY;
                    }
                    break;
                case MotionEventActions.Up:
                    if (IsInProgress)
                    {
                        ptrID1 = INVALID_POINTER_ID;
                    }
                    if (Listener != null)
                    {
                        Listener.OnRotateEnd(this);
                    }
                    break;
                case MotionEventActions.PointerUp:
                    if (IsInProgress)
                    {
                        ptrID2 = INVALID_POINTER_ID;
                    }
                    if (Listener != null)
                    {
                        Listener.OnRotateEnd(this);
                    }
                    break;
                case MotionEventActions.Cancel:
                    ptrID1 = INVALID_POINTER_ID;
                    ptrID2 = INVALID_POINTER_ID;
                    if (Listener != null)
                    {
                        Listener.OnRotateEnd(this);
                    }
                    break;
                default:
                    break;
            }
            return true;
        }

        private float AngleBetweenLines(float fX, float fY, float sX, float sY, float nfX, float nfY, float nsX, float nsY)
        {
            float angle1 = (float)Math.Atan2((fY - sY), (fX - sX));
            float angle2 = (float)Math.Atan2((nfY - nsY), (nfX - nsX));

            float angle = ((float)RadianToDegree(angle1 - angle2)) % 360;
            if (angle < -180.0f) angle += 360.0f;
            if (angle > 180.0f) angle -= 360.0f;
            return angle;
        }

        private double RadianToDegree(double angle)
        {
            return angle * (180.0f / Math.PI);
        }

        public interface IOnRotationGestureListener
        {
            public bool OnRotateBegin(RotationGestureDetector detector);
            public void OnRotate(RotationGestureDetector detector);
            public void OnRotateEnd(RotationGestureDetector detector);
        }
    }
}