using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer2D.Player;

namespace Platformer2D.Platform
{
    public class GravityPlatformController : PlatformController
    {
        private PlatformModel _model;
        private PlatformView _view;
        //private bool isMoving = false;

        //private float _direction = 1f;
        
        public GravityPlatformController(PlatformModel model, PlatformView view) : base(model, view)
        {
            _model = model;
            _view = view;
        }

        // Метод для обработки взаимодействия с платформой
        public void HandleInteraction(bool isOnPlatform)
        {
            PlayerController.Instance.ChangeGravity();
        }
    }
}

