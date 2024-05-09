using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Platformer2D.Platform
{
    public class SlidingPlatformView : PlatformView
    {
        // Переопределение метода для установки модели платформы
        public override void SetModel(PlatformModel model)
        {
            // Создание нового контроллера Sliding
            controller = new SlidingPlatformController(model, this);
            Debug.Log($"SetModel SlidingPlatformController model.TargetScore = {model.TargetScore}");
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            controller.OnCollisionEnter2D(collision);
        }
    }
}

