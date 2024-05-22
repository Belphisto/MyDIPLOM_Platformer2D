using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;

/*
Класс BackgroundView представляет визуальное отображение фона. 
Он содержит ссылку на компонент Image, который используется для отображения фона, текущий цвет фона и текущий прогресс в достижении целевого цвета.
В методе Start он получает ссылку на компонент Image, устанавливает текущий цвет и создает модель и контроллер фона.
Содержит методы для изменения цвета фона и установки контроллера фона.
*/

namespace Platformer2D.Background
{
    // Класс BackgroundView представляет визуальное отображение фона.
    public class BackgroundView : MonoBehaviour
    {
        // Ссылка на компонент Image, который используется для отображения фона
        private Image _image;
        // Текущий цвет фона
        private Color _currentColor;
        // Текущий прогресс в достижении целевого цвета
        private float _t = 0f;

        // Свойства для доступа к приватным полям
        public Image Image {get => _image; set => _image = value;}
        public Color CurrentColor {get => _currentColor; set => _currentColor = value;}
        public float T {get => _t; set => _t = value;}

        // Ссылка на контроллер фона
        private BackgroundControlller _backgroundController;
    
        void Start()
        {
            
        }
        void Awake()
        {
            _image = GetComponentInChildren<Image>(); 
            _currentColor = _image.color;
            // Создание модели и контроллера фона
            BackgroundModel backgroundModel = new BackgroundModel();
            _backgroundController = new BackgroundControlller(backgroundModel, this);
            var canvas = GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
        }
        // Метод для изменения цвета фона
        public void ChangeColor(Color newColor)
        {
            _image.color = newColor;
        }

        // Метод для установки контроллера фона
        public void SetController(BackgroundControlller controlller)
        {
            this._backgroundController = controlller;
        }
        private void OnDestroy()
        {
            _backgroundController.OnDestroy();
        }
    }
    
}
