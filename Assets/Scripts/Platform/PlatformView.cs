using System.Collections;
using System.Collections.Generic;
using Platformer2D.IInterface;
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
    public class PlatformView : MonoBehaviour
    {
        // Ссылки на компоненты SpriteRenderer для различных состояний платформы
        [SerializeField] private SpriteRenderer stateColorless;
        [SerializeField] private SpriteRenderer stateColor;

        // Контроллер платформы
        protected PlatformController controller;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Метод для установки модели платформы
        public virtual void SetModel(PlatformModel model)
        {
            controller = new PlatformController(model, this);
            Debug.Log($"SetModel PlatformController model.TargetScore = {model.TargetScore}");
        }

        // Метод Awake вызывается при инициализации объекта
        private void Awake()
        {
            // ссылка на первый дочерний объект с компонентом SpriteRenderer
            stateColorless = transform.GetChild(0).GetComponent<SpriteRenderer>();

            //ссылка на второй дочерний объект с компонентом SpriteRenderer
            stateColor = transform.GetChild(1).GetComponent<SpriteRenderer>();

            // Изначально активируется состояние без цвета
            stateColor.gameObject.SetActive(false);
        }

        // Метод для изменения состояния платформы
        public void ChangeState()
        {   
            // Активация цветного состояния и деактивация состояния без цвета
            Debug.Log("PlatformChangeState");
            stateColor.gameObject.SetActive(true);
            stateColorless.gameObject.SetActive(false);
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
