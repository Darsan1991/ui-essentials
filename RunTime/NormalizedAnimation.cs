using System;
using System.Collections;
using UnityEngine;

namespace DGames.Essentials.UI.Scriptable
{
    public abstract class NormalizedAnimation : ScriptableObject, INormalizedAnimation
    {
        // ReSharper disable once TooManyArguments
        public abstract IEnumerator Anim(Action<float> onFrame, Action onComplete = null, float time = 1,
            float startNormalizedTime = 0f);
    }
}

namespace DGames.Essentials.UI
{
    public abstract class NormalizedAnimation :INormalizedAnimation
    {
        // ReSharper disable once TooManyArguments
        public abstract IEnumerator Anim(Action<float> onFrame, Action onComplete = null, float time = 1,float startTime=0f);
    }
}