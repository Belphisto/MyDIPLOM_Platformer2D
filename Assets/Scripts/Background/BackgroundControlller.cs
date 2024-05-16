using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
класс BackgroundController управляет поведением фона в игре. 
Он принимает модель и представление фона в качестве параметров конструктора.
Метод SetTargetScore устанавливает целевой счет, который должен быть достигнут.
Метод HandleScoreUpdate обрабатывает обновление счета, 
    вычисляет прогресс в достижении целевого счета, 
    вычисляет новый цвет фона на основе прогресса 
    и применяет этот новый цвет к представлению фона.
*/

namespace Platformer2D.Background
{
    // Класс BackgroundControlller управляет поведением фона в игре
    public class BackgroundControlller
    {
        // модель данных фона
        private BackgroundModel model;
        //модель представления фона
        private BackgroundView view;

        // Конструктор класса, принимает модель и представление
        public BackgroundControlller(BackgroundModel model, BackgroundView view)
        {
            this.model = model;
            this.view = view;
            Bus.Instance.SendBackgroundScore += HandleScoreUpdate;
        }

        // Метод установки целевого количества очков
        public void SetTargerScore(int targetScore)
        {
            model.TargetScore = targetScore;
            Debug.Log($"BackgroundControlller set targetScore: { model.TargetScore}");
        }
        
        // Метод для обработки обновления счета
        public void HandleScoreUpdate(int score)
        {
            if (model.TargetScore == 0)
            {
                //Debug.Log("TargetScore is zero, cannot calculate progress");
                return;
            }
            // Вычисление прогресса в достижении целевого количества очков
            float progress = (float)score /model.TargetScore;

            // Ограничение прогресса значением 1
            if (progress >= 1f)
            {
                progress = 1f;
            }

            // Установка прогресса в представлении
            view.T = progress;

            // Вычисление нового цвета фона на основе прогресса
            Color newColor = new Color(
                Mathf.Lerp(view.CurrentColor.r, model.TargetColor.r, view.T),
                Mathf.Lerp(view.CurrentColor.g, model.TargetColor.g, view.T),
                Mathf.Lerp(view.CurrentColor.b, model.TargetColor.b, view.T),
                view.CurrentColor.a
            );
            //Debug.Log($"Update Color {newColor}");

            // Применение нового цвета к представлению
            view.ChangeColor(newColor);
        }

        public void OnDestroy()
        {
            Bus.Instance.SendBackgroundScore -= HandleScoreUpdate;
        }
    }
    
}
