using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer2D.Player;
namespace Platformer2D.Model
{
    public class HeroModel
    {
        public float Speed { get; set; } = 3f;
        public float JumpForce { get; set; } = 5f;
        public int Score { get; set; } = 0;
        public void IncrementScore(int amount)
        {
            Score += amount;
        }
        public void DecrementScore(int amount)
        {
            Score -= amount;
        }
        //public States State{ get; set; } = States.idle;
    }
}
