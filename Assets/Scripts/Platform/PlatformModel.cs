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
        public int TargetScore {get => _targetScore; set => _targetScore = value;}

        // Конструктор класса PlatformModel
        // Принимает целевой счет
        public PlatformModel(int score)
        {
            _targetScore = score;
        }
    }

}
