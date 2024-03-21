using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Класс BackgroundModel представляет модель данных для фона, 
    включая скорость перехода между цветами, 
    целевой цвет, 
    процент осветления за каждый полученный очко
    и целевое количество очков для достижения.
*/

namespace Platformer2D.Background
{
    // Класс BackgroundModel представляет модель данных для фона.
    public class BackgroundModel
    {
        // Скорость перехода между цветами
        public float TransitionSpeed { get; set; } = 1f;
         // Целевой цвет для фона
        public Color TargetColor { get; set; } = Color.white;
        // Процент осветления за каждое увеличение количества очков
        public float LighteningPercentagePerPoint { get; set; } = 0.01f;
        // Целевое количество очков для достижения
        public int TargetScore {get;set;} = 0;
    }
}

