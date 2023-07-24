using System;
using System.Collections;
using UnityEngine;

namespace DGames.Essentials.UI
{
    public class NormalizedAtomicAnimation : IAtomicAnimation
    {
        public event Action<IAtomicAnimation> Completed;
        public event Action<IAtomicAnimation,float> RunFrame;

        private readonly INormalizedAnimation _anim;
        private readonly Action<float> _onFrame;
        private readonly MonoBehaviour _runner;
        private Coroutine _coroutine;

        public bool Loop { get; set; }
        public string Name { get; }
        public float Normalized { get; private set; }
        public float Duration { get; set; }
        public IAtomicAnimation.Status CurrentStatus { get; private set; }

        public float SpeedMultiplexer { get; set; }


        // ReSharper disable once TooManyDependencies
        public NormalizedAtomicAnimation(string name, INormalizedAnimation anim, Action<float> onFrame,
            MonoBehaviour runner,float duration ,float speed=1f)
        {
            Name = name;
            SpeedMultiplexer = speed;
            _anim = anim;
            _onFrame = onFrame;
            _runner = runner;
            Duration = duration;
        }
        
        public void Play(float normalized=0f)
        {
            Normalized = normalized;
            CurrentStatus = IAtomicAnimation.Status.Playing;
            _coroutine = _runner.StartCoroutine(StartRunAnim(Normalized));
        }

        private IEnumerator StartRunAnim(float normalized)
        {
            while (Loop)
            {
                yield return _anim.Anim(n =>
                {
                    _onFrame?.Invoke(n);
                    RunFrame?.Invoke(this, n);
                }, startNormTime: normalized,time:Duration/SpeedMultiplexer);
                yield return null;
            }

            CurrentStatus = IAtomicAnimation.Status.None;
            _coroutine = null;
            Completed?.Invoke(this);
        }

        public void Stop()
        {
            if (CurrentStatus == IAtomicAnimation.Status.None)
                return;
            
            StopAnim();
            Normalized = 0;
            CurrentStatus = IAtomicAnimation.Status.None;
        }

        private void StopAnim()
        {
            _runner.StopCoroutine(_coroutine);
            _coroutine = null;
        }

        public void Pause()
        {
            if (CurrentStatus != IAtomicAnimation.Status.Playing)
                return;
            
            StopAnim();
            CurrentStatus = IAtomicAnimation.Status.Paused;
        }

        public void Resume()
        {
            if (CurrentStatus != IAtomicAnimation.Status.Paused)
                return;

            CurrentStatus = IAtomicAnimation.Status.Playing;
            _coroutine = _runner.StartCoroutine(StartRunAnim(Normalized));
        }
    }
}