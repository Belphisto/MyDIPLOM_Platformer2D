using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Platformer2D.Crystal;
using Platformer2D.Platform;
using Platformer2D.Background;

/*
Класс LevelModel представляет модель данных для уровня игры.
 Он содержит информацию о префабах для кристаллов, платформ и фона, 
 позициях для размещения кристаллов и платформ, общем счете, количестве кристаллов и текущем счете. 
 Класс также предоставляет методы для управления моделью текущего счета: увеличения и уменьшения текущего счета. 
 Класс связывает данные об уровне.
*/

namespace Platformer2D.Level
{
    // Класс LevelModel представляет модель данных для игрового уровня
    public class LevelModel 
    {
        // Конкретные префабы для кристаллов, платформ и фона
        private CrystalView _crystalPrefab;
        private PlatformView _platformPrefab;
        private BackgroundView _backgroundPrefab;
        // Позиции размещения кристаллов, платформ
        private List<Vector3> _positionsCrystal;
        private List<Vector3> _positionsPlatforms;
        // Общий счет, количество кристаллов и текущий счет игрока на конкретном уровне
        private int _totalScore;
        private int _crystalCount;
        private int _currentScore;

        // Свойства для доступа к приватным полям
        public CrystalView СrystalPrefab {get => _crystalPrefab; set => _crystalPrefab = value;}
        public PlatformView PlatformPrefab {get => _platformPrefab; set => _platformPrefab = value;}
        public BackgroundView BackgroundPrefab {get => _backgroundPrefab; set => _backgroundPrefab = value;}
        public List<Vector3> PositionsCrystal {get => _positionsCrystal; set => _positionsCrystal = value;}
        public List<Vector3> PositionsPlatfroms {get => _positionsPlatforms; set => _positionsPlatforms = value;}
        public int TotalScore {get => _totalScore; set => _totalScore= value;}
        public int CrystalCount {get => _crystalCount; set => _crystalCount = value;}
        public int CurrentScore {get => _currentScore; set => _currentScore = value;}

        // Конструктор класса LevelModel
        // Принимает префабы: кристалл, платформа, фон
        //  Позиции для размещения: кристаллов, платформ
        //  Целевой счет на уровне и количество кристаллов
        //  Устанавилвает текущий счет уровня в 0
        public LevelModel(CrystalView crystalPrefab, PlatformView platform , BackgroundView backgroundPrefab, List<Vector3> positions, List<Vector3> positionsPLatforms, int score, int count)
        {
            _crystalPrefab = crystalPrefab;
            _platformPrefab = platform;
            _backgroundPrefab = backgroundPrefab;
            _positionsCrystal = positions;
            _positionsPlatforms = positionsPLatforms;
            _totalScore = score;
            _crystalCount = count;
            _currentScore = 0;
        }

        // Метод для увеличения текущего счета
        public void IncrementScore(int amount)
        {
            _currentScore += amount;
        }

        // Метод для уменьшения текущего счета
        public void DecrementScore(int amount)
        {
            _currentScore -= amount;
        }

    }
}

