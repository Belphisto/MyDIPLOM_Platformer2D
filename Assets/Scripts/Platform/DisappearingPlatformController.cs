using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer2D.Platform
{
    public class DisappearingPlatformController : PlatformController
    {
        private float _disappearInterval = 3f; // Интервал исчезновения
        private float _disappearDuration = 2f; // Продолжительность исчезновения
        private PlatformModel _model;
        private PlatformView _view;
        public DisappearingPlatformController(PlatformModel model, PlatformView view) : base(model, view)
        {
            _model = model;
            _view = view;
            _view.StartCoroutine(DisappearRoutine());
        }

        private IEnumerator DisappearRoutine()
        {
            Collider2D collider = _view.GetComponent<Collider2D>();

            // Случайная задержка перед началом цикла
            yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 10f));

            while (true)
            {
                yield return new WaitForSeconds(_disappearInterval);
                collider.enabled = false; // Отключение коллайдера
                _view.Disappear();
                yield return new WaitForSeconds(_disappearDuration);
                collider.enabled = true; // Включение коллайдера
                _view.Appear();
            }
            
        }

    }

}
