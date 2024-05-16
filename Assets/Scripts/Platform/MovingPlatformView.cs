using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer2D.Platform
{
    public class MovingPlatformView : PlatformView
    {
        // Переопределение метода для установки модели платформы
        public override void SetModel(PlatformModel model)
        {
            // Создание нового контроллера MovingPlatform
            controller = new MovingPlatformController(model, this);
            Debug.Log($"SetModel MovingPlatformController model.TargetScore = {model.TargetScore}");
        }

        private void OnEnable()
        {
            // Restart the coroutine when the GameObject is set to active
            if (controller != null)
            {
                ((MovingPlatformController)controller).RestartStartMoving();
            }
        }
    }

}

