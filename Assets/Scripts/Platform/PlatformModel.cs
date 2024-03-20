using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer2D.Platform
{
    public class PlatformModel
    {
        private int _targetScore;
        public int TargetScore {get => _targetScore; set => _targetScore = value;}

        public PlatformModel(int score)
        {
            _targetScore = score;
        }
    }

}
