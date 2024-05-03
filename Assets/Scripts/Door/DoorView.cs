using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer2D.Level
{
    public class DoorView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer stateColorless;
        [SerializeField] private SpriteRenderer stateColor;
        private DoorController controller;
        public LocationType type;
        public TypeSlot slot;
        
        public Vector2 GetColliderSize()
        {
            BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
            return boxCollider.size;
        }
        // Метод для установки модели платформы
        public virtual void SetModel(DoorModel model)
        {
            controller = new DoorController(model, this);
        }

        // Метод Awake вызывается при инициализации объекта
        private void Awake()
        {
            //
            DoorModel doorModel= new DoorModel(10, 3);
            SetModel(doorModel);
            //
            // ссылка на первый дочерний объект с компонентом SpriteRenderer
            stateColorless = transform.GetChild(0).GetComponent<SpriteRenderer>();

            //ссылка на второй дочерний объект с компонентом SpriteRenderer
            stateColor = transform.GetChild(1).GetComponent<SpriteRenderer>();

            // Изначально активируется состояние без цвета
            stateColor.gameObject.SetActive(false);
        }

        private void Update()
        {
            controller.Update();
        }

        public void ChangeState()
        {   
            // Активация цветного состояния и деактивация состояния без цвета
            Debug.Log("PlatformChangeState");
            stateColor.gameObject.SetActive(true);
            stateColorless.gameObject.SetActive(false);
        }


        public void OnTriggerExit2D(Collider2D collision)
        {
            controller.OnTriggerExit2D(collision);
            
        }
        public void OnTriggerEnter2D(Collider2D collision)
        {
            controller.OnTriggerEnter2D(collision);
            Debug.Log($"Need: {controller.GetCountCrystal().ToString()}");
        }
        public void OnTriggerStay2D(Collider2D collision)
        {
            controller.OnTriggerStay2D(collision);
        }

    }

}
