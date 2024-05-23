using System.Collections;
using System.Collections.Generic;
using Platformer2D.Inventory;

using UnityEngine;
namespace Platformer2D.Platform
{
    public class ChestController 
    {
        ChestModel model;
        ChestView view;
        protected bool isCorrectActiveSlot = false;
        protected bool isPlayerInside = false;
        protected InventorySlot activeslot;
        public ChestController(ChestModel model, ChestView view)
        {
            this.model = model;
            this.view = view;

        }
        
        internal void Update()
        {
            if (isPlayerInside)
            {
                CheckActiveSlot();
                HandleChestInteraction();
            }
        }

        private void CheckActiveSlot()
        {
            activeslot = InventoryView.Instance.GetActiveSlot();
            if (activeslot != null && (activeslot.locationType == LocationType.Default))
            {
                isCorrectActiveSlot = true;
            }
            else
            {
                isCorrectActiveSlot = false;
                CameraManager.Instance.UpadteText("Incorrect Inventory slot selected");
            }
        }

        private void HandleChestInteraction()
        {
            if (!model.IsOpen)
            {
                if (isCorrectActiveSlot)
                {
                    if(activeslot.Count < model.TargetScore)
                    {
                        CameraManager.Instance.UpadteText($"Недостаточно. Нужно: {model.TargetScore} для открытия сундука");
                    }
                    else
                    {
                        OpenChestWithFKey();
                    }
                }
            }
            else
            {
                EndGameWithEnterKey();
            }
        }

        private void OpenChestWithFKey()
        {
            CameraManager.Instance.UpadteText($"Press F for chest");
            if(Input.GetKeyDown(KeyCode.F))
            {
                model.IsOpen = true;
                InventoryView.Instance.DecrementSlot(view.type, model.TargetScore);
            }
        }

        private void EndGameWithEnterKey()
        {
            CameraManager.Instance.UpadteText($"Press Enter");
            if (Input.GetKeyDown(KeyCode.Return))
            {
                CameraManager.Instance.SetActive(false);
                Debug.Log($"GameWin");
                EndGame();
            }
        }

        public void EndGame()
        {
            Bus.Instance.SendGameWin(Player.PlayerController.Instance.SendTotalScore());
        }

        public  void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                isPlayerInside = true;
                CameraManager.Instance.UpadteText($"Need: {model.TargetScore} to open chest");
                CameraManager.Instance.SetActive(true);
            }
        }

        public void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                isPlayerInside = true;
            }
        }

        public void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                isPlayerInside = false;
                CameraManager.Instance.SetActive(false);
            }
        }

    }

}
