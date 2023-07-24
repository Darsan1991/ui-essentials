using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace DGames.Essentials.UI
{
    public class CurveNormalizedAnimation : NormalizedAnimation
    {
        private readonly AnimationCurve _curve;

        public CurveNormalizedAnimation(AnimationCurve curve)
        {
            _curve = curve;
        }

        // ReSharper disable once TooManyArguments
        public override IEnumerator Anim(Action<float> onFrame, Action onComplete = null, float time = 1,float startNormTime=0)
        {
            yield return MoveTowardsEnumerator(_curve, onFrame, onComplete, time,startNormTime);
        }
        
        // ReSharper disable once TooManyArguments
        public static IEnumerator MoveTowardsEnumerator(AnimationCurve curve, Action<float> onCallOnFrame = null, Action onFinished = null,
            float time = 1,float offset=0f)
        {
            var start = curve.keys.Min(k=>k.time);
            var end = curve.keys.Max(k=>k.time);
            
            yield return MoveTowardsEnumerator(start + (end-start)*offset, end, n => onCallOnFrame?.Invoke(curve.Evaluate(n)), time: Mathf.Max(0.01f,time*(1-offset)),
                onFinished: onFinished);
        }
        
        // ReSharper disable once TooManyArguments
        // ReSharper disable once MethodTooLong
        public static IEnumerator MoveTowardsEnumerator(float start = 0f, float end = 1f, Action<float> onCallOnFrame = null, Action onFinished = null,
            float time=1f)
        {
            var speed = Mathf.Abs(end-start)/time;
            if (Math.Abs(start - end) < float.Epsilon)
            {
                onFinished?.Invoke();
                yield break;
            }

            var currentNormalized = start;
            while (true)
            {
                currentNormalized = Mathf.MoveTowards(currentNormalized, end,  speed* Time.deltaTime);

                if (start < end && currentNormalized >= end || start > end && currentNormalized <= end)
                {
                    currentNormalized = end;
                    onCallOnFrame?.Invoke(currentNormalized);
                    break;
                }
                
                onCallOnFrame?.Invoke(currentNormalized);
                yield return null;
            }
            onFinished?.Invoke();
        }
    }
}