using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    public Text CountRed;
    public Text CountGreen;
    public Text CountBlue;
    public Text CountSky;

    public InventorySlot[] slots;  // Список слотов

    private Dictionary<LocationType, Text> scoreTexts;

    private List<string> itemType;
    // Start is called before the first frame update
    void Start()
    {
        scoreTexts = new Dictionary<LocationType, Text>
        {
            { LocationType.Red, CountRed },
            { LocationType.Green, CountGreen },
            { LocationType.Blue, CountBlue },
            { LocationType.Sky, CountSky }
        };

        // Установить начальное значение текстовых полей в 0
        foreach (var text in scoreTexts.Values)
        {
            text.text = "0";
        }

        Bus.Instance.UpdateCrystal += UpdateText;
        // Подписаться на событие активации слота
        foreach (var slot in slots)
        {
            slot.OnActivate += HandleSlotActivation;
        }
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
    private void UpdateText(int score, LocationType type)
    {
        if (scoreTexts.ContainsKey(type))
            scoreTexts[type].text = score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
