using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DGames.Essentials.UI
{
    public abstract class AtomicAnimator : MonoBehaviour,IBaseAtomicAnimator
    {
        public event Action<string> Completed; 
        public abstract IEnumerable<IAtomicAnimation> Animations { get; }
        
        public IAtomicAnimation Animation { get; private set; }

        protected virtual void Awake()
        {
            
        }

        public void Play(string animationName)
        {
            Animation = Animations.FirstOrDefault(a => a.Name == animationName);
            if (Animation == null) return;
            
            Animation.Completed += AnimationOnCompleted;
            Animation.Play();
        }
        

        public IEnumerator PlayEnumerator(string animationName)
        {
            Play(animationName);
            yield return new WaitUntil(() => Animation == null);
        }

        private void AnimationOnCompleted(IAtomicAnimation anim)
        {
            anim.Completed -= AnimationOnCompleted;
            Completed?.Invoke(anim.Name);
            Animation = null;
        }

        public void Stop()
        {
            if (Animation == null) return;
            Animation.Stop();
            Animation.Completed -= AnimationOnCompleted;
            Animation = null;
        }
    }
}