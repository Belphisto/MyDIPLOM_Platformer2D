using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer2D.Player;

namespace Platformer2D.Platform
{
    public class GravityPlatformController : PlatformController
    {
        public GravityPlatformController(PlatformModel model, PlatformView view) : base(model, view)
        {
            this.model = model;
            this.view = view;
        }

        // Метод для обработки взаимодействия с платформой
        public void HandleInteraction(bool isOnPlatform)
        {
            PlayerController.Instance.ChangeGravity();
        }
    }
}

