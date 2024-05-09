using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer2D.Player;

namespace Platformer2D.Platform
{
    public class SlidingPlatformController: PlatformController
    {
        private float slideImpulse = 10f; // Сила скольжения
        public SlidingPlatformController(PlatformModel model, PlatformView view) : base(model, view)
        {
            this.model = model;
            this.view = view;
        }

        // Метод для обработки столкновения с персонажем
        public override void OnCollisionEnter2D(Collision2D collision)
        {
            // Проверка тега столкнувшегося объекта
            if (collision.gameObject.tag == "Player")
            {
                // Применение импульса скольжения к игроку
                Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
                Vector2 direction = playerRigidbody.velocity.normalized;
                playerRigidbody.AddForce(direction * slideImpulse, ForceMode2D.Impulse);
                Debug.Log($"Add Impulse for Player = {slideImpulse}");
            }
        }

    }
}

