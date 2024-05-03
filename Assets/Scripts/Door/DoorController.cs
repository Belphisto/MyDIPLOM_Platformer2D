using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer2D.Level
{
    public class DoorController
    {
        protected DoorModel model;
        protected DoorView view;
        private bool isCorrectActiveSlot = false;
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

        public void Update()
        {
            if (isCorrectActiveSlot && Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("PressF");
            }
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                InventorySlot activeslot = InventoryView.Instance.GetActiveSlot();
                if (activeslot != null && activeslot.type == view.slot)
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
                InventorySlot activeslot = InventoryView.Instance.GetActiveSlot();
                if (activeslot != null && activeslot.type == view.slot)
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
            return model.CountCrystalForOpen;
        }
    }

}
