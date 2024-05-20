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

        private bool isPlayerInside = false;
        protected bool isCorrectActiveSlot = false;
        protected InventorySlot activeslot;

        public DoorController(DoorModel model, DoorView view)
        {
            this.model = model; 
            this.view = view;
            //Bus.Instance.SendPlatformsScore += HandleScoreUpdate;
        }

        //private void HandleScoreUpdate(int score)
        //{
        //    if (score >= model.TargetScore) view.ChangeState();
        //}

        //public bool IsColor()
        //{
        //    return model.IsColor;
        //}

        internal virtual void Update()
        {
            if (isPlayerInside)
            {
                CheckActiveSlot();
                HandleDoorInteraction();
            }
            
        }

        private void CheckActiveSlot()
        {
            activeslot = InventoryView.Instance.GetActiveSlot();
            if (activeslot != null && (activeslot.locationType == model.CurrentLocation.Item2 || activeslot.locationType == model.NextLocation.Item2))
            {
                isCorrectActiveSlot = true;
            }
            else
            {
                isCorrectActiveSlot = false;
                CameraManager.Instance.UpadteText("Incorrect Inventory slot selected");
                CameraManager.Instance.SetActive(true);
            }
        }

        private void HandleDoorInteraction()
        {
            if (!model.IsOpen)
            {
                if (isCorrectActiveSlot) 
                {
                    if(activeslot.Count < model.CountForOpen)
                    {
                        CameraManager.Instance.UpadteText("Not enough crystal");
                        CameraManager.Instance.SetActive(true);
                    }
                    else
                    {
                        OpenDoorWithFKey();
                    }
                }
            }
            else
            {
                EnterNextRoomWithEnterKey();
            }
        }

        private void OpenDoorWithFKey()
        {
            CameraManager.Instance.UpadteText($"Press F to open door in {model.NextLocation.Item1} Room");
            CameraManager.Instance.SetActive(true);
            if(Input.GetKeyDown(KeyCode.F))
            {
                model.IsOpen = true;
                CameraManager.Instance.SetActive(false);
                InventoryView.Instance.DecrementSlot(activeslot.locationType, model.CountForOpen);
                InventoryView.Instance.IncrementSlot(LocationType.Default);
                view.ChangeState();
            }
        }

        private void EnterNextRoomWithEnterKey()
        {
            CameraManager.Instance.UpadteText($"Press Enter to {model.NextLocation.Item1} Room");
            CameraManager.Instance.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Return))
            {
                CameraManager.Instance.SetActive(false);
                int newIndex = model.NextLocation.Item1;
                Debug.Log($"Next location index: {newIndex}");
                Bus.Instance.SendNextIndexLocation(newIndex);
            }
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                isPlayerInside = true;
                CameraManager.Instance.UpadteText($"Need: {model.CountForOpen} to {model.NextLocation.Item1} Room");
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
                isCorrectActiveSlot = false;
                CameraManager.Instance.SetActive(false);
            }
        }

        public int GetCountCrystal()
        {
            return model.CountForOpen;
        }
    }
}
