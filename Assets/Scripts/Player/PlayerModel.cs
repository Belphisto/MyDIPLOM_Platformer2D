using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer2D.Player
{
    // Класс модели персонажа
    public class PlayerModel
    {
        // Скорость, сила прыжка, счет очков персонажа и состояние "на земле"
        private float _speed;
        private float _jumpForce;
        private int _score;
        private bool _isGrounded;

        // Свойства доступа к полям
        public float Speed {get => _speed; set => _speed = value;}
        public float JumpForce { get => _jumpForce; private set => _jumpForce = value; }
        public int Score { get => _score; private set => _score = value; }
        public bool IsGrounded { get => _isGrounded; set => _isGrounded = value; }

        private bool _isMooving = true;
        public bool IsMooving { get => _isMooving; set => _isMooving = value;}

        //Конструктор модели персонажа с параметрами по умолчанию
        public PlayerModel()
        {
            _speed = 5f;
            _jumpForce = 4f;
            _score = 0;
            _isGrounded = false;
        }

        //Методы для увеличения и уменьшения счета
        public void IncrementScore(int amount)
        {
            _score += amount;
        }

        public void DecrementScore(int amount)
        {
            _score -= amount;
        }
    }
}

