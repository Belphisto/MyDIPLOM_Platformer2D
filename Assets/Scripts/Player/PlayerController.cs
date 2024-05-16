using System;
using System.Collections;
using System.Collections.Generic;
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

        // Флаг для отслеживания инвертированного управления
        private bool isControlsInverted = false;
        private int jumpCount = 0;

        // Определение события увеличения счетчика очков
        //public static event Action<int> OnScoreUpdate;

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

            // Подписка на событие инвертирования управления
            Bus.Instance.InvertControls += InvertControls;
        }

        public void OnDestroy()
        {
            //Bus.Instance.InvertControls -= InvertControls;
            _instance = null;
        }

        // Метод, вызываемый каждый кадр
        public void Update()
        {
            // Управление анимацией и движением персонажа
            // Обработка пользовательского ввода
            if (model.IsGrounded)
            {
                view.Animator.SetInteger("state", (int)States.idle);
                jumpCount = 0;
            } 
            float input = Input.GetAxis("Horizontal");

            // Инвертирование ввода, если управление инвертировано
            if (isControlsInverted)
            {
                input *= -1;
            }
            if (input != 0)
            {    
                view.Run(view.transform.right * input, model.Speed);
            }

            //if (model.IsGrounded && Input.GetButtonDown("Jump"))
            if (jumpCount <2 && Input.GetButtonDown("Jump"))
            {
                view.Jump(view.GetComponent<Rigidbody2D>(), model.JumpForce);
                jumpCount++; 
            }
        }

        public void CheckGround(bool getIsGrounded)
        {
            model.IsGrounded = getIsGrounded;
            // Если персонаж не на земле, устанавливаем состояние анимации на прыжок
            if (!model.IsGrounded) view.Animator.SetInteger("state", (int)States.jump);
        }

        //Метод обработки получения очков
        public void GetScore(int point)
        {
            model.IncrementScore(point);
            //Debug.Log($"Current PlayerScore = {model.Score}");
            Bus.Instance.SendAllScore(model.Score);
            // Вызов события обновления счета, если оно не пустое
            //OnScoreUpdate?.Invoke(point);
            Bus.Instance.Send(point);
            //Bus.Instance.SendBackground(model.Score);
            //Bus.Instance.SendAllScore(model.Score);
        }
        public int SendTotalScore()
        {
            return model.Score;
        }

        //Метод для управления гравитацией
        public void ChangeGravity()
        {
            Physics2D.gravity = Physics2D.gravity *-1;
            view.FlipY();
        }

        public void InvertControls()
        {
            isControlsInverted = !isControlsInverted;
        }

        public IEnumerator PlayerFell(bool isFell)
        {
            yield return new WaitForSeconds(3);
            Debug.Log("Bus.Instance.SendMessage(isFell);");
            Bus.Instance.SendMessage(isFell);
        }            
    }
    
}
