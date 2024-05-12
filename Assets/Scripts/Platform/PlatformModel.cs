using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Это модель данных, которая хранит информацию о платформе, такую как целевой счет для изменения состояния платформы.
*/

namespace Platformer2D.Platform
{
    // Класс PlatformModel представляет модель данных для платформы
    public class PlatformModel
    {
        // Целевой счет для изменения состояния платформы
        private int _targetScore;
        private Vector3 _startPosition;
        private float _speed;
        public Vector3 StartPosition { get => _startPosition; set => _startPosition = value; }
        public int TargetScore {get => _targetScore; set => _targetScore = value;}
        public float Speed {get => _speed; set => _speed = value;}

        public bool IsColor {get; set;}

        // Конструктор класса PlatformModel
        // Принимает целевой счет
        public PlatformModel(int score)
        {
            _targetScore = score;
            _speed = 0;
            IsColor = false;
        }
        public PlatformModel()
        {
            IsColor = false;
            _targetScore = 0;
            _speed = 0;
        }
    }

}
