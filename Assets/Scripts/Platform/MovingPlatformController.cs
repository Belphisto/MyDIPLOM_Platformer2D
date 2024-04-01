using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer2D.Platform
{
    public class MovingPlatformController : PlatformController
    {
        private PlatformModel _model;
        private PlatformView _view;
        private bool isMoving = false;

        private float _direction = 1f;
        
        public MovingPlatformController(PlatformModel model, PlatformView view) : base(model, view)
        {
            _model = model;
            _view = view;
        }

        public override void Update()
        {
            if (!isMoving)
            {
                _view.StartCoroutine(StartMoving());
            }
            else
            {
                float verticalDistance = 4f;
                Vector3 targetPosition = _model.StartPosition + Vector3.up * verticalDistance * _direction;

                // Если достигли верхней или нижней точки, меняем направление движения
                if (_view.transform.position.y >= _model.StartPosition.y + verticalDistance && _direction > 0 ||
                    _view.transform.position.y <= _model.StartPosition.y && _direction < 0)
                {
                    _direction *= -1f; // Изменение направления движения
                }

                // Перемещение платформы
                _view.transform.position = Vector3.MoveTowards(_view.transform.position, targetPosition, _model.Speed * Time.deltaTime);
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

