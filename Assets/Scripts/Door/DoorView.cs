using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer2D.Platform
{
    public class DoorView : ChangeableState
    {
        protected DoorController controller;
        [SerializeField] private int IndexLocation;
        public LocationType type;
        //public TypeSlot slot;
        
        public Vector2 GetColliderSize()
        {
            BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
            return boxCollider.size;
        }
        // Метод для установки модели платформы
        public virtual void SetModel(DoorModel model)
        {
            controller = new DoorController(model, this);
            IndexLocation = model.IndexLocation; 
        }

        // Метод Awake вызывается при инициализации объекта
        protected override void Awake()
        {
            DoorModel doorModel= new DoorModel(10, 3, 1);
            SetModel(doorModel);
            base.Awake();
        }

        private void Update()
        {
            controller.Update();
        }


        public void OnTriggerExit2D(Collider2D collision)
        {
            controller.OnTriggerExit2D(collision);
            
        }
        public void OnTriggerEnter2D(Collider2D collision)
        {
            controller.OnTriggerEnter2D(collision);
            //Debug.Log($"Need: {controller.GetCountCrystal().ToString()}");
        }
        public void OnTriggerStay2D(Collider2D collision)
        {
            controller.OnTriggerStay2D(collision);
        }

    }

}
