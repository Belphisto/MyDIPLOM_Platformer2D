using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer2D.Crystal
{
    public class CrystalModel
    {
        private int _score;
        public int Score {get => _score; set => _score = value;}

        public CrystalModel(int score)
        {
            _score = score;
        }
        public CrystalModel()
        {
            _score = 0;
        }
    }

}

