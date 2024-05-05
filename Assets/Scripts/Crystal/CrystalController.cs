using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer2D.Player;

namespace Platformer2D.Crystal
{
    // Класс CrystalController управляет поведением кристалла в игре
    public class CrystalController
    {
        // Модель данных кристалла
        private CrystalModel model;
        // Представление кристалла
        private CrystalView view;

        // Конструктор класса
        // Принимает модель и представление кристалла
        public CrystalController(CrystalModel model, CrystalView view)
        {
            this.model = model;
            this.view = view;
        }

        // Метод для обработки входа в триггер
        // Вызывается, когда другой объект входит в триггер кристалла
        public void HandleTriggerEnter(Collider2D other)
        {
            // Проверка, что персонаж  PlayerController вошел в триггер
            if (other.gameObject == PlayerController.Instance.View.gameObject) 
            {
                Debug.Log($"HandleTriggerEnter CrystalController: crystal.Score = {model.Score}");

                // Увеличить счет игрока на значение цены кристалла
                PlayerController.Instance.GetScore(model.Score); 

                // Отправить событие об уничтожении кристалла
                Bus.Instance.SendCrystal(model.Type);

                // Уничтожить кристалл
                view.DestroyPoint(); 
            }
        }
    }

}
