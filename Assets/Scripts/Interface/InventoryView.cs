using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Platformer2D.Inventory
{
    public class InventoryView : MonoBehaviour
    {
        public static InventoryView Instance { get; private set; }
        public TextMeshProUGUI CountRed;
        public TextMeshProUGUI CountGreen;
        public TextMeshProUGUI CountBlue;
        public TextMeshProUGUI CountSky;
        public TextMeshProUGUI CountDoorItem;

        public TextMeshProUGUI TotalScoreInGame;
        public TextMeshProUGUI PersentLevel;

        public InventorySlot[] slots;  // Список слотов

        private Dictionary<LocationType, TextMeshProUGUI> scoreTexts;

        private List<string> itemType;
        private void Awake()
        {
            // Проверка на наличие другого экземпляра InventoryView
            if (Instance != null && Instance != this)
            {
                // Уничтожить объект, если уже существует другой экземпляр
                Destroy(gameObject);
            }
            else
            {
                // Сохранить ссылку на экземпляр
                Instance = this;
            }

        }
        // Start is called before the first frame update
        void Start()
        {
            SoundManager.Instance.PlaySound(0);

            scoreTexts = new Dictionary<LocationType, TextMeshProUGUI>
            {
                { LocationType.Red, CountRed },
                { LocationType.Green, CountGreen },
                { LocationType.Blue, CountBlue },
                { LocationType.Sky, CountSky },
                { LocationType.Default, CountDoorItem}
            };
            
            // Установить начальное значение текстовых полей в 0
            foreach (var text in scoreTexts.Values)
            {
                text.text = "0";
            }

            Bus.Instance.UpdateCrystal += IncrementSlot;
            Bus.Instance.UdateTotalScore += UpdateTotalScore;
            Bus.Instance.UdateLevel +=UpdatePercent;

            // Подписаться на событие активации слота
            foreach (var slot in slots)
            {
                slot.OnActivate += HandleSlotActivation;
            }
        }
        private void OnDestroy()
        {
            Bus.Instance.UpdateCrystal -= IncrementSlot;
            Bus.Instance.UdateTotalScore -= UpdateTotalScore;
            Bus.Instance.UdateLevel -= UpdatePercent;

            foreach (var slot in slots)
            {
                slot.OnActivate -= HandleSlotActivation;
            }
            Instance = null;
        }

        private void UpdateTotalScore(int score)
        {
            TotalScoreInGame.text = score.ToString();
        }

        private void UpdatePercent(int score)
        {
            PersentLevel.text = $"{score.ToString()}%";
        }

        // Обработчик активации слота
        private void HandleSlotActivation(InventorySlot activatedSlot)
        {
            // Деактивировать все слоты, кроме активированного
            foreach (var slot in slots)
            {
                if (slot != activatedSlot)
                {
                    slot.Deactivate();
                }
            }
        }
        private void UpdateText()
        {
            foreach (var slot in slots)
            {
                scoreTexts[slot.locationType].text = slot.Count.ToString();
            }
            
        }
        public void DecrementSlot(LocationType type, int count)
        {
            SoundManager.Instance.PlaySound(2);
            foreach (var slot in slots)
            {
                if (slot.locationType == type)
                {
                    slot.DecrementSlot(count);
                    UpdateText();
                }
            }
        }

        public void IncrementSlot(LocationType type)
        {
            //Debug.LogWarning($"IncrementSlot inventoryview {type}");
            SoundManager.Instance.PlaySound(1);
            foreach (var slot in slots)
            {
                if (slot.locationType == type)
                {
                    slot.IncrementSlot();
                    UpdateText();
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        // Метод для получения текущего активного слота
        public InventorySlot GetActiveSlot()
        {
            foreach (var slot in slots)
            {
                if (slot.IsActive)
                {
                    return slot;
                }
            }

            return null; // Если нет активного слота, вернуть null
        }

        
    }

}
