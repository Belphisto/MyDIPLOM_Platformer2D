using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer2D.Player;

namespace Platformer2D.Platform
{
    public class InvertPlatformController : PlatformController
    {
        public InvertPlatformController(PlatformModel model, PlatformView view) : base(model, view)
        {
            this.model = model;
            this.view = view;
        }

        // Метод для обработки столкновения с игроком
        public override void OnCollisionEnter2D(Collision2D collision)
        {
            // Проверка тега столкнувшегося объекта
            if (collision.gameObject.tag == "Player")
            {
                // Изменение управления игрока
                Bus.Instance.SendInvertControls();
            }
        }
    }
}

