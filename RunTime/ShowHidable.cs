using System;
using System.Collections;
using System.Collections.Generic;
using DGames.Essentials.Attributes;
using UnityEngine;

namespace DGames.Essentials.UI
{
    public abstract class ShowHidable : MonoBehaviour, IShowHideable
    {
        public event EventHandler<bool> ShowStateChanged;

        [SerializeField] private bool _animate;
        [Condition(nameof(_animate),true)]
        [SerializeField] protected AnimatorField anim;

        [SerializeField] private bool _addDependObjects;
        [Condition(nameof(_addDependObjects),true)]
        [SerializeField]protected List<GameObject> dependObjects = new();
        
        protected ShowState currentShowState = ShowState.Hide;

        public bool Showing
        {
            get => gameObject.activeSelf;
            protected set
            {
                if (value == gameObject.activeSelf)
                    return;

                gameObject.SetActive(value);
                ShowStateChanged?.Invoke(this, value);
            }
        }


        public ShowState CurrentShowState
        {
            get => currentShowState;
            protected set
            {
                if (currentShowState == value)
                {
                    return;
                }

                if (!Showing && (value == ShowState.ShowAnimation || value == ShowState.Show))
                    Showing = true;
                else if (Showing && (value == ShowState.Hide))
                {
                    Showing = false;
                }

                currentShowState = value;
            }
        }
        
        public IBaseAtomicAnimator Animator { get; private set; }

        protected virtual void Awake()
        {
            Animator = anim.GetAnimator(this);
        }

        // ReSharper disable once FlagArgument
        public virtual void Show(bool animate = true, Action completed = null)
        {
            if (Showing)
                throw new InvalidOperationException();

            CurrentShowState = ShowState.ShowAnimation;

            if (_animate && animate && Animator != null)
            {
                StartCoroutine(WithCallback(Animator.PlayEnumerator("Show"), () =>
                {
                    CurrentShowState = ShowState.Show;
                    OnShowCompleted();
                    completed?.Invoke();
                }));
            }
            else
            {
                CurrentShowState = ShowState.Show;
                OnShowCompleted();
                completed?.Invoke();
            }
        }
        
        protected virtual void OnShowCompleted()
        {
        
        }

        protected virtual void OnEnable()
        {
            dependObjects.ForEach(o => o.SetActive(true));
        }

        protected virtual void OnDisable()
        {
            dependObjects.ForEach(o =>
            {
                if (o != null) o.SetActive(false);
            });
        }

        //     ReSharper disable once FlagArgument
        public virtual void Hide(bool animate = true, Action completed = null)
        {
            if (!Showing)
                throw new InvalidOperationException();
            CurrentShowState = ShowState.HideAnimation;
            if (_animate && animate && Animator != null)
            {
                StartCoroutine(WithCallback(Animator.PlayEnumerator("Show"), () =>
                {
                    CurrentShowState = ShowState.Hide;
                    OnHideCompleted();
                    completed?.Invoke();
                }));
            }
            else
            {
                CurrentShowState = ShowState.Hide;
                completed?.Invoke();
            }
        }

        protected virtual void OnHideCompleted()
        {
        
        }

        public void Exit() => Hide();
        public void Enter()=>Show();
        
        
            public static IEnumerator WithCallback(IEnumerator enumerator, Action action = null)
        {
            yield return enumerator;
            action?.Invoke();
        }
    }
    
    

    [Serializable]
    public struct AnimatorField
    {
        [SerializeField] private bool _useUnityAnimatorInstead;
        [Condition(nameof(_useUnityAnimatorInstead),false)][SerializeField] private AtomicAnimator _animator;
        [Condition(nameof(_useUnityAnimatorInstead),true)][SerializeField] private Animator _unityAnimator;
    
        public IBaseAtomicAnimator GetAnimator(MonoBehaviour runner) => 
            _useUnityAnimatorInstead ? new UnityAnimatorAtomicAnimator(_unityAnimator, runner) : _animator;
    }

    // public class CombinedAnimator : AtomicAnimator
    // {
    //     public override IEnumerable<IAtomicAnimation> Animations { get; }
    // }


    // [Serializable]
    // public struct NormalizationAnimationField:INormalizedAnimation
    // {
    //     [SerializeField] private bool _useScriptable;
    //     [Condition(nameof(_useScriptable),false)]
    //     [SerializeField] private NormalizedAnimation _anim;
    //     [Condition(nameof(_useScriptable),true)]
    //     [SerializeField] private Scriptable.NormalizedAnimation _animScriptable;
    //
    //     public INormalizedAnimation Animation => _useScriptable ? _animScriptable : _anim;
    //     
    //     // ReSharper disable once TooManyArguments
    //     public IEnumerator Anim(Action<float> onFrame, Action onComplete = null, float time = 1,float startNormTime=0f)
    //     {
    //         yield return Animation.Anim(onFrame, onComplete, time,startNormTime);
    //     }
    // }
    
}