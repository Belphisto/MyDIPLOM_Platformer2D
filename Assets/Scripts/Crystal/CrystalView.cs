using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer2D.Crystal
{
    // Класс CrystalView представляет визуальное отображение кристалла в игре
    public class CrystalView : MonoBehaviour
    {
        // ССылка на контроллер кристалла
        private CrystalController controller;

        // Start is called before the first frame update
        void Start()
        {
            //controller = new CrystalController(new CrystalModel(), this);
        }

        // Метод для установки модели кристалла
        // Создает новый контроллер кристалла с данной моделью
        public void SetModel(CrystalModel model)
        {
            controller = new CrystalController(model, this);
        }

        // Метод для обработки входа в триггер
        // Вызывается, когда другой объект входит в триггер кристалла
        private void OnTriggerEnter2D(Collider2D other)
        {
            controller.HandleTriggerEnter(other);
        }
        
        // Метод для уничтожения кристалла
        public void DestroyPoint()
        {
            Destroy(gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }

}
