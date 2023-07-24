using System;
using System.Collections;

namespace DGames.Essentials.UI
{
    public interface INormalizedAnimation
    {
        // ReSharper disable once TooManyArguments
        IEnumerator Anim(Action<float> onFrame, Action onComplete = null, float time = 1,float startNormTime = 0f);
    }
}