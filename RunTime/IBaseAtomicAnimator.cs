using System;
using System.Collections;

namespace DGames.Essentials.UI
{
    public interface IBaseAtomicAnimator
    {
        event Action<string> Completed;
        void Play(string animationName);
        IEnumerator PlayEnumerator(string animationName);
        void Stop();
    }
}