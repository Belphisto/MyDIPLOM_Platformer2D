using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer2D.Platform
{
    public class MovingPlatformController : PlatformController
    {
        private MovingPlatformModel _model;
        private PlatformView _view;

        private float _direction = 1f;
        
        public MovingPlatformController(MovingPlatformModel model, PlatformView view) : base(model, view)
        {
            _model = model;
            _view = view;
        }

        public override void Update()
        {
            // Перемещение платформы между начальной и конечной точками
            float distance = Vector3.Distance(_model.StartPosition, _model.EndPosition);
            if (distance <= 0.1f)
            {
                _direction *= -1f; // Изменение направления движения
            }
            _view.transform.position = Vector3.MoveTowards(_view.transform.position, _direction > 0 ? _model.EndPosition : _model.StartPosition, _model.Speed * Time.deltaTime);
        }

    }

}

