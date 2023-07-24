using System;
using System.Collections;
using UnityEngine;

namespace DGames.Essentials.UI
{
    public class UnityAnimatorAtomicAnimator : IBaseAtomicAnimator
    {
        public event Action<string> Completed;
        
        private readonly Animator _anim;
        private readonly MonoBehaviour _runner;


        public UnityAnimatorAtomicAnimator(Animator anim,MonoBehaviour runner)
        {
            _anim = anim;
            _runner = runner;
        }
        
        public void Play(string animationName)
        {
            _runner.StartCoroutine(PlayEnumerator(animationName));
        }

        public IEnumerator PlayEnumerator(string animationName)
        {
            _anim.enabled = true;
            _anim.Play(animationName);
            yield return new WaitUntil(() =>
            {
                var stateInfo = _anim.GetCurrentAnimatorStateInfo(0);
                return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= .99f;
            });
            Completed?.Invoke(animationName);
        }

        public void Stop()
        {
            _anim.enabled = false;
        }
    }
}