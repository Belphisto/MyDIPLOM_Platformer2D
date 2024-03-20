using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer2D.Background
{
    public class BackgroundModel
    {
        public float TransitionSpeed { get; set; } = 1f;
        public Color TargetColor { get; set; } = Color.white;
        public float LighteningPercentagePerPoint { get; set; } = 0.01f;
    }
}

