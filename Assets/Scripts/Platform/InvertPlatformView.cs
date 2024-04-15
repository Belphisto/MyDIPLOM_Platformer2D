using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Platformer2D.Platform
{
    public class InvertPlatformView : PlatformView
    {
        // Переопределение метода для установки модели платформы
        public override void SetModel(PlatformModel model)
        {
            // Создание нового контроллера Invert
            controller = new InvertPlatformController(model, this);
            Debug.Log($"SetModel InvertPlatformController model.TargetScore = {model.TargetScore}");
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            controller.OnCollisionEnter2D(collision);
        }

        void OnCollisionExit2D(Collision2D collision)
        {
            controller.OnCollisionEnter2D(collision);
        }
    }

}
