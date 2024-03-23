using System;
using System.Collections;
using System.Collections.Generic;
using Platformer2D.IInterface;
using UnityEngine;

namespace Platformer2D.Player
{
    // Класс контроллера игрока
    public class PlayerController
    {
        // Ссылки на модель и представление игрока
        private PlayerModel model;
        private PlayerView view;
        public PlayerView View {get => view;}

        // Определение события увеличения счетчика очков
        public static event Action<int> OnScoreUpdate;

        // Синглтон контроллера игрока
        private static PlayerController _instance;
        public static PlayerController Instance
        {
            get { return _instance; }
            private set { _instance = value; }
        }

        // Конструктор контроллера игрока
        public PlayerController(PlayerModel model, PlayerView view)
        {
            this.model = model;
            this.view = view;
            Instance = this;
        }

        // Метод, вызываемый каждый кадр
        public void Update()
        {
            // Управление анимацией и движением персонажа
            // Обработка пользовательского ввода
            if (model.IsGrounded) view.Animator.SetInteger("state", (int)States.idle);
        
            if (Input.GetButton("Horizontal"))
                view.Run(view.transform.right * Input.GetAxis("Horizontal"), model.Speed);
                Debug.Log("move!! Horizontal");
            if (model.IsGrounded && Input.GetButtonDown("Jump"))
                view.Jump(view.GetComponent<Rigidbody2D>(), model.JumpForce);
        }

        public void CheckGround()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(view.transform.position, 0.9f);
            model.IsGrounded = colliders.Length > 1;
        
            if (!model.IsGrounded) view.Animator.SetInteger("state", (int)States.jump);
        }

        //Метод обработки получения очков
        public void GetScore(int point)
        {
            model.IncrementScore(point);
            Debug.Log($"Current PlayerScore = {model.Score}");
            
            // Вызов события обновления счета, если оно не пустое
            OnScoreUpdate?.Invoke(point);
            Bus.Instance.Send(point);
        }                
    }
    
}
