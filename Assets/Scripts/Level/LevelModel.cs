using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer2D.Crystal;
using Platformer2D.Background;
using Platformer2D.IInterface;

namespace Platformer2D.Level
{
    public class LevelModel : IScoreLevel
    {
        private CrystalView _crystalPrefab;
        private BackgroundView _backgroundPrefab;
        private List<Vector3> _positions;
        private int _totalScore;
        private int _crystalCount;
        private int _currentScore;

        public CrystalView СrystalPrefab {get => _crystalPrefab; set => _crystalPrefab = value;}
        public BackgroundView BackgroundPrefab {get => _backgroundPrefab; set => _backgroundPrefab = value;}
        public List<Vector3> Positions {get => _positions; set => _positions = value;}
        public int TotalScore {get => _totalScore; set => _totalScore= value;}
        public int CrystalCount {get => _crystalCount; set => _crystalCount = value;}
        public int CurrentScore {get => _currentScore; set => _currentScore = value;}

        public LevelModel(CrystalView crystalPrefab, BackgroundView backgroundPrefab, List<Vector3> positions, int score, int count)
        {
            _crystalPrefab = crystalPrefab;
            _backgroundPrefab = backgroundPrefab;
            _positions = positions;
            _totalScore = score;
            _crystalCount = count;
            _currentScore = 0;
        }
        public LevelModel(){}

        public void IncrementScore(int amount)
        {
            _currentScore += amount;
        }

        public void DecrementScore(int amount)
        {
            _currentScore -= amount;
        }

    }
}

