using System;
using System.Collections.Generic;
using UnityEngine;

namespace DGames.Essentials.Animation
{
}

namespace DGames.Essentials.UI
{
    public class ShowHidableAnimator : AtomicAnimator
    {
        [SerializeField] private AnimationInfo _showInfo;
        [SerializeField] private AnimationInfo _hideInfo;


        private readonly List<IAtomicAnimation> _animations = new();
        public override IEnumerable<IAtomicAnimation> Animations => _animations;

        protected override void Awake()
        {
            base.Awake();
            _animations.Add(new NormalizedAtomicAnimation("Show", new CurveNormalizedAnimation(_showInfo.curve),
                (_) => { }, this, _showInfo.speed));
            _animations.Add(new NormalizedAtomicAnimation("Hide", new CurveNormalizedAnimation(_hideInfo.curve),
                (_) => { }, this, _hideInfo.speed));
        }
        

        [Serializable]
        public struct AnimationInfo
        {
            public AnimationType type;
            public AnimationCurve curve;
            public float speed;
        }

        public enum AnimationType
        {
            LeftSlide,
            RightSlide,
            TopSlide,
            DownSlide,
            ScaleOut,
            ScaleIn
        }
    }
}