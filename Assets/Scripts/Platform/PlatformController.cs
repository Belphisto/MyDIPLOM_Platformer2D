using System.Collections;
using System.Collections.Generic;
using Platformer2D.Level;
using UnityEngine;

/*
Это контроллер, который управляет поведением платформы в игре.
 Он принимает модель и представление платформы в качестве параметров конструктора,
  подписывается на событие обновления счета и обрабатывает это событие,
   изменяя состояние платформы, 
   если счет достиг целевого значения.
*/

namespace Platformer2D.Platform
{
    // Класс контроллера платформы
    public class PlatformController 
    {
        // Ссылки на модель и представление платформы
        protected PlatformModel model;
        protected PlatformView view;
        
        // Конструктор контроллера платформы
        // Принимает модель и представление
        public PlatformController(PlatformModel model, PlatformView view)
        {
            this.model = model;
            this.view = view;
            // Подписка на событие обновления счета
            Bus.Instance.SendPlatformsScore += HandleScoreUpdate;
            //Debug.Log($"evelController.OnScoreUpdate += HandleScoreUpdate;");
        }
        
        // Обработка события текущего счета
        private void HandleScoreUpdate(int score)
        {
            if (score >= model.TargetScore) 
            {
                view.ChangeState();
                model.IsColor = true;
                }

            //Debug.Log($"HandleScoreUpdate(int score) PlatformController: score {score}, model.TargetScore = {model.TargetScore}");
        }

        public int GetTargerScore()
        {
            return model.TargetScore;
        }

        // Метод Update вызывается каждый кадр для обновления состояния платформы
        // Переопределен в производном классе для других механик
        public virtual void Update()
        {
            
        }

        public virtual void OnCollisionEnter2D(Collision2D collision)
        {
            
        }

        public bool IsColor()
        {
            return model.IsColor;
        }
    }

}
