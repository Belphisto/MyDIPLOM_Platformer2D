using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer2D.Platform
{
    public class DisappearingPlatformView : PlatformView
    {
        // Переопределение метода для установки модели платформы
        public override void SetModel(PlatformModel model)
        {
            // Создание нового контроллера Disappearing
            controller = new DisappearingPlatformController(model, this);
            Debug.Log($"SetModel DisappearingPlatformController model.TargetScore = {model.TargetScore}");
        }
    }

}

