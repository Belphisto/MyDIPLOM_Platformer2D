using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Platformer2D.Platform
{
    public class GravityPlatformView : PlatformView
    {
        // Переопределение метода для установки модели платформы
        public override void SetModel(PlatformModel model)
        {
            // Создание нового контроллера GravityPlatform
            controller = new GravityPlatformController(model, this);
            Debug.Log($"SetModel GravityPlatformController model.TargetScore = {model.TargetScore}");
        }

        // Метод, вызываемый при входе в триггер
        void OnTriggerEnter2D(Collider2D other)
        {
            // Проверяем, является ли столкнувшийся объект игроком
            if (other.gameObject.CompareTag("Player"))
            {
                // Если игрок вошел в область действия платформы, меняем гравитацию
                ((GravityPlatformController)controller).HandleInteraction(true);
            }
        }

        // Метод, вызываемый при выходе из триггера
        void OnTriggerExit2D(Collider2D other)
        {
            // Проверяем, является ли столкнувшийся объект игроком
            if (other.gameObject.CompareTag("Player"))
            {
                // Если игрок покинул область действия платформы, возвращаем гравитацию в нормальное состояние
                ((GravityPlatformController)controller).HandleInteraction(false);
            }
        }
    }

}
