using System.Collections;
using System.Collections.Generic;
using Platformer2D.IInterface;
using UnityEngine;

namespace Platformer2D.Player
{
    public class PlayerModel
    {
        private float _speed;
        private float _jumpForce;
        private int _score;
        private bool _isGrounded;

        public float Speed {get => _speed; set => _speed = value;}
        public float JumpForce { get => _jumpForce; private set => _jumpForce = value; }
        public int Score { get => _score; private set => _score = value; }
        public bool IsGrounded { get => _isGrounded; set => _isGrounded = value; }

        public PlayerModel()
        {
            _speed = 5f;
            _jumpForce = 3f;
            _score = 0;
            _isGrounded = false;
        }

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

