using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer2D.Level;
using Platformer2D.Inventory;

namespace Platformer2D.Platform
{
    public class DoorController
    {
        protected DoorModel model;
        protected DoorView view;
        protected bool isCorrectActiveSlot = false;
        protected InventorySlot activeslot;
        // Start is called before the first frame update
        public DoorController(DoorModel model, DoorView view)
        {
            this.model = model; 
            this.view = view;
            // Подписка на событие обновления счета
            LevelController.OnScoreUpdatePlatfroms += HandleScoreUpdate;
            
        }

        // Обработка события текущего счета
        private void HandleScoreUpdate(int score)
        {
            if (score >= model.TargetScore) view.ChangeState();
            Debug.Log($"HandleScoreUpdate(int score) PlatformController: score {score}, model.TargetScore = {model.TargetScore}");
        }

        public bool IsColor()
        {
            return model.IsColor;
        }

        public virtual void Update()
        {
            if (!model.IsOpen)
            {
                if (isCorrectActiveSlot && Input.GetKeyDown(KeyCode.F))
                {
                    //Debug.Log("PressF");
                    if (activeslot.Count >= model.CountForOpen)
                    {
                        Debug.Log("Door Opened");
                        model.IsOpen = true;
                        InventoryView.Instance.DecrementSlot(view.type, model.CountForOpen);
                        InventoryView.Instance.IncrementSlot(LocationType.Default);
                    }
                    else
                    {
                        Debug.Log("Недостаточно средств");
                    }
                }
            }
            else
            {
                Debug.Log("Door was open early");
            }
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                activeslot = InventoryView.Instance.GetActiveSlot();
                if (activeslot != null && activeslot.locationType == view.type)
                {
                    Debug.Log("Active correctSlot");
                    isCorrectActiveSlot = true;
                }
                Debug.Log("Player has entered the door trigger");
            }
        }

        public void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                activeslot = InventoryView.Instance.GetActiveSlot();
                if (activeslot != null && activeslot.locationType == view.type)
                {
                    isCorrectActiveSlot = true;
                }
                else
                {
                    isCorrectActiveSlot = false;
                }
            }
        }

        public void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                isCorrectActiveSlot = false;
                Debug.Log("Player has exited the door trigger");
            }
        }

        public int GetCountCrystal()
        {
            return model.CountForOpen;
        }
    }

}
