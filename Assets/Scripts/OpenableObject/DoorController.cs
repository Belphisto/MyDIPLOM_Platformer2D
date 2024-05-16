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
            Bus.Instance.SendPlatformsScore += HandleScoreUpdate;
            
        }

        // Обработка события текущего счета
        private void HandleScoreUpdate(int score)
        {
            if (score >= model.TargetScore) view.ChangeState();
        }

        public bool IsColor()
        {
            return model.IsColor;
        }

        public virtual void Update()
        {
            if (!model.IsOpen)
            {
                if (isCorrectActiveSlot) 
                {
                    CameraManager.Instance.UpadteText($"Press F");
                    CameraManager.Instance.SetActive(true);
                    if(Input.GetKeyDown(KeyCode.F))
                    {
                        if (activeslot.Count >= model.CountForOpen)
                        {
                            Debug.Log("Door Opened");
                            model.IsOpen = true;
                            CameraManager.Instance.SetActive(false);
                            InventoryView.Instance.DecrementSlot(activeslot.locationType, model.CountForOpen);
                            InventoryView.Instance.IncrementSlot(LocationType.Default);
                        }
                        else
                        {
                            CameraManager.Instance.UpadteText($"not enough crystals");
                            CameraManager.Instance.SetActive(true);
                            Debug.Log("not enough crystals");
                        }
                    }
                }
            }
            else
            {
                if(Input.GetKeyDown(KeyCode.Return))
                {
                    CameraManager.Instance.SetActive(false);
                    int newIndex = model.NextLocation.Item1;
                        
                    Debug.Log($"New location index: {newIndex}");

                    Bus.Instance.SendNextIndexLocation(newIndex);
                }
            }
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                activeslot = InventoryView.Instance.GetActiveSlot();
                CameraManager.Instance.UpadteText($"Need: {model.CountForOpen}");
                CameraManager.Instance.SetActive(true);
                if (activeslot != null && (activeslot.locationType == model.CurrentLocation.Item2 || activeslot.locationType == model.NextLocation.Item2))
                {
                    Debug.Log("Active correctSlot");
                    
                    isCorrectActiveSlot = true;
                }
            }
        }

        public void OnTriggerStay2D(Collider2D collision)
        {
            if (model.IsOpen)
            {
                CameraManager.Instance.UpadteText($"Press Enter");
                CameraManager.Instance.SetActive(true);
            }
        }

        public void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("Deactive correctSlot");
                CameraManager.Instance.SetActive(false);
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
