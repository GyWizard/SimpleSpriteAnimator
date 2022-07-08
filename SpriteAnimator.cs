using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SimpleSpriteAnimator
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimator : MonoBehaviour
    {
        [SerializeField] private List<SpriteAnimation> SpriteAnimationsList;
        
        private SpriteRenderer _spriteRenderer;
        private SpriteAnimation _currentAnimation;
        private int _currentAnimationIndex;
        private float _timer;
        private bool _isPlaying;

        public event Action<string> OnAnimationStarted;
        public event Action<string> OnAnimationEnded;
        public event Action<(string oldAnimation, string newAnimation)> OnAnimationChanged;
        public event Action<(string animationName, int frameIndex)> OnFrameChanged;
        public void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void PlayAnimation(string animationName)
        {
            var newAnimation = TryGetAnimation(animationName);
            
            if (_currentAnimation == newAnimation)
            {
                return;
            }
            if (_currentAnimation)
            {
                OnAnimationChanged?.Invoke((_currentAnimation.Name, animationName));
            }

            _isPlaying = true;
            _currentAnimationIndex = 0;
            _currentAnimation = newAnimation;
            _timer = 0;
            OnAnimationStarted?.Invoke(_currentAnimation.Name);
        }

        public void StopAnimation()
        {
            _isPlaying = false;
        }

        SpriteAnimation TryGetAnimation(string animationName)
        {
            var anim = SpriteAnimationsList.Find(x => x.Name.Equals(animationName));
            if (!anim)
            {
                throw new Exception($"Animation {animationName} not found");
            }

            return anim;
        }

        void Update()
        {
            if (!_currentAnimation || !_isPlaying)
            {
                return;
            }

            if (_timer < _currentAnimation.FrameRate)
            {
                _timer += Time.deltaTime;
                return;
            }

            SetNextAnimationFrame();
            _timer = 0;
        }

        void SetNextAnimationFrame()
        {
            _currentAnimationIndex++;
            if (_currentAnimationIndex >= _currentAnimation.Sprites.Length)
            {
                _currentAnimationIndex = 0;
                OnAnimationEnded?.Invoke(_currentAnimation.Name);
                if (!_currentAnimation.Loop)
                {
                    _currentAnimation = null;
                    _timer = 0;
                    return;
                }
            }
            _spriteRenderer.sprite = _currentAnimation.Sprites[_currentAnimationIndex];
            OnFrameChanged?.Invoke((_currentAnimation.Name,_currentAnimationIndex));
        }
    }
}