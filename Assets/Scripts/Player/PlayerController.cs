using System;
using System.Collections;
using System.Collections.Generic;
using Platformer2D.IInterface;
using UnityEngine;

namespace Platformer2D.Player
{
    public class PlayerController : MonoBehaviour, IScoreUpdate
    {
        private PlayerModel model;
        private PlayerView view;

        // Определение события делегата увеличения счетчика очков
        //public delegate void ScoreUpdateEvent(int score);
        //public static event ScoreUpdateEvent OnScoreUpdate;
        public event Action<int> OnScoreUpdate;

        private static PlayerController _instance;
        public static PlayerController Instance
        {
            get { return _instance; }
            private set { _instance = value; }
        }

        public PlayerController(PlayerModel model, PlayerView view)
        {
            this.model = model;
            this.view = view;
        }
        public void Update()
        {
            if (model.IsGrounded) view.Animator.SetInteger("state", (int)States.idle);
        
            if (Input.GetButton("Horizontal"))
                view.Run(transform.right * Input.GetAxis("Horizontal"), model.Speed);
            if (model.IsGrounded && Input.GetButtonDown("Jump"))
                view.Jump(view.GetComponent<Rigidbody2D>(), model.JumpForce);
        }

        public void CheckGround()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(view.transform.position, 0.9f);
            model.IsGrounded = colliders.Length > 1;
        
            if (!model.IsGrounded) view.Animator.SetInteger("state", (int)States.jump);
        }

        public void GetScore(int point)
        {
            model.IncrementScore(point);
            Debug.Log(model.Score);
            OnScoreUpdate?.Invoke(point);
        }                
    }
    
}
