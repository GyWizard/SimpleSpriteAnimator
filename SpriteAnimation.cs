using System;
using UnityEngine;

namespace SimpleSpriteAnimator
{
    [CreateAssetMenu(fileName = "SpriteAnimation", menuName = "SimpleSpriteAnimator/SpriteAnimator", order = 1)]
    public class SpriteAnimation : ScriptableObject
    {
        public Sprite[] Sprites;
        public string Name;
        public float FrameRate;
        public bool Loop;
    }
}