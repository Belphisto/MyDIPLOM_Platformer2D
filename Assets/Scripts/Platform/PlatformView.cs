using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer2D.Player;

/*
 Это представление, которое отвечает за визуализацию платформы в игре.
  Оно содержит ссылки на компоненты SpriteRenderer для различных состояний платформы и
методы для установки модели платформы и изменения состояния платформы.
*/

namespace Platformer2D.Platform
{
    // Класс PlatformView представляет визуальное отображение платформы в игре.
    public class PlatformView : ChangeableState
    {
        // Контроллер платформы
        protected PlatformController controller;

        public Vector2 GetColliderSize()
        {
            BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
            return boxCollider.size;
        }

        // Метод для установки модели платформы
        public virtual void SetModel(PlatformModel model)
        {
            controller = new PlatformController(model, this);
            Debug.Log($"SetModel PlatformController model.TargetScore = {model.TargetScore}");
        }

        // Метод Update вызывается каждый кадр
        public void Update()
        {
            controller.Update();
        }

        public void Disappear()
        {
            stateColor.gameObject.SetActive(false);
            stateColorless.gameObject.SetActive(false);
        }

        public void Appear()
        {
            stateColor.gameObject.SetActive(controller.IsColor());
            stateColorless.gameObject.SetActive(!controller.IsColor());
        }

    }
    
}
