using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer2D.Platform
{
    public class MovingPlatformController : PlatformController
    {
        private PlatformModel _model;
        private PlatformView _view;

        private float _direction = 1f;
        
        public MovingPlatformController(PlatformModel model, PlatformView view) : base(model, view)
        {
            _model = model;
            _view = view;
        }

        public override void Update()
        {
            // Перемещение платформы между начальной и конечной точками
            /*float distance = Vector3.Distance(_model.StartPosition, _model.EndPosition);
            if (distance <= 0.1f)
            {
                _direction *= -1f; // Изменение направления движения
            }
            _view.transform.position = Vector3.MoveTowards(_view.transform.position, _direction > 0 ? _model.EndPosition : _model.StartPosition, _model.Speed * Time.deltaTime);*/
            // Перемещение платформы вверх-вниз на 4 единицы
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

}

