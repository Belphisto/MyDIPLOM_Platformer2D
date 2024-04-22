using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer2D.Platform
{
    public class MovingPlatformController : PlatformController
    {
        private bool isMoving = false;

        private float _direction = 1f;
        
        public MovingPlatformController(PlatformModel model, PlatformView view) : base(model, view)
        {
            this.model = model;
            this.view = view;
        }

        public override void Update()
        {
            if (!isMoving)
            {
                view.StartCoroutine(StartMoving());
            }
            else
            {
                float verticalDistance = 4f;
                Vector3 targetPosition = model.StartPosition + Vector3.up * verticalDistance * _direction;

                // Если достигли верхней или нижней точки, меняем направление движения
                if (view.transform.position.y >= model.StartPosition.y + verticalDistance && _direction > 0 ||
                    view.transform.position.y <= model.StartPosition.y && _direction < 0)
                {
                    _direction *= -1f; // Изменение направления движения
                }

                // Перемещение платформы
                view.transform.position = Vector3.MoveTowards(view.transform.position, targetPosition, model.Speed * Time.deltaTime);
            }
        }

        private IEnumerator StartMoving()
        {
            // Генерируем случайное время задержки от 0 до 2 секунд
            float delay = Random.Range(0f, 10f);
            //Debug.Log($"delay {delay}");

            // Ждем заданное количество времени
            yield return new WaitForSeconds(delay);

            // Начинаем движение
            isMoving = true;
        }

    }

}

